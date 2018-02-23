using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

	//constants
	private const int COMBONUM = 3;					//maximum attacks in a combo
	private const int DOWN = 0;						//direction
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references
	private Rigidbody2D rb;
	private Animator an;
	private PlayerMovement pm;
	private GhostSprites gs;
	private PlayerHUD ph;
	private PlayerHealth php;
	private PlayerSword ps;
	public GameObject[] HUD;						//the proper HUD to be instantiated
	public GameObject[] commandsOriginals;			//the array of commands to be instantiated (order editable in menu)
	public PlayerCommand[] commands;//the array of usable commands

	//variables
	public float[] attackSpeed;						//attacking speeds of various combo attacks
	public float[] damageMultiplier;				//attacking damage of various combo attacks
	public float dashTime = 0.35f;					//how long does the player dash?
	public float dashSpeed = 3.0f;					//how fast does the player dash?
	public float dashDelay = 2.5f;					//how much time until the player can dash again.
	public int numDashes = 2;						//how many dashes does the player have available?
	public int numCommands = 3;						//how many commands does the player have available?
	public float damageColliderOffset = 0.03f;		//how far back does the damageCollider move when attacking?
	public int[] chosenCommands;					//which commands were selected in the menu?

	[HideInInspector] public int selectedCommand = 1;//which command is currently selected?
	[HideInInspector] public bool canCommand = true;//can the player currently use a command? (don't let them when their changing commands)
	[HideInInspector] public bool canAttack = true;	//is the player currently able to attack? (times out the combos)
	private bool[] attackCombo;						//Keeps track of what stage of the combo we're in
	[HideInInspector] public bool attackLock = false;//prevents player spam from skipping attacks
	[HideInInspector] public bool hasSword = true;	//does the player have access to his sword?
	private bool attacking = false;					//is the player currently attacking?
	private float horiz;
	private float vert;								//used for slight movement available during attacks
	public bool canDash = true;						//is the player currently able to dash?
	private bool dashing = false;					//is the player currently dashing?
	private float dashCounter = 0;					//how long has it been since you started dashing?
	private float delayCounter = 0;					//how long has it been since you finished dashing?
	private int dashCharges;						//how many dashes does the player have remaining
	private float tmpDashCalc;						//remove a calculation each frame of dash by storing it as a tmp variable ((delayCounter % dashDelay) / dashDelay)
	private float tmpDashCalc2;						//same as above but instead by a factor of O(1) instead of O(n) (1 - (dashTime - dashCounter) - tmpDashCalc)

	// Use this for initialization
	void Start () {
		switch (numCommands) {
		case 2:
			Instantiate (HUD [0]);
			break;
		case 3:
			Instantiate (HUD [1]);
			break;
			//etc
		}
		canDash = true;
		hasSword = true;
		dashing = false;
		attackCombo = new bool[COMBONUM];
		attacking = false;
		attackLock = true;
		dashCharges = numDashes;
		Physics2D.IgnoreLayerCollision(8, 9, false);				//prevent invincability by dashing into transition
		for (int i = 0; i < COMBONUM; i++) {
			attackCombo[i] = false;
		}
		canAttack = true;
		pm = GetComponent<PlayerMovement> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
		gs = GetComponent<GhostSprites> ();
		ps = GetComponentInChildren<PlayerSword> ();
		ph = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerHUD> ();
		php = GetComponent<PlayerHealth> ();
		php.ph = ph;
		for (int i = 0; i < numCommands; i++) {
			commands [i] = Instantiate (commandsOriginals [chosenCommands[i]]).GetComponent<PlayerCommand>();
		}
		gs.ClearTrail ();
		selectedCommand = 1;
		canCommand = true;
	}
	
	// Update is called once per frame
	void Update () {

		#region Dash
		//End Dash (once dashTime is up)
		if (dashing) {
			dashCounter += Time.deltaTime;
			if (dashCounter >= dashTime) {
				//Dash Finished
				dashing = false;
				php.damageCollider.enabled = true;
				Physics2D.IgnoreLayerCollision(8, 9, false);
				dashCounter = 0;
				rb.velocity = Vector2.zero;
				pm.canMove = true;
				delayCounter += dashDelay;
				if (delayCounter > dashDelay * numDashes) {
					delayCounter = dashDelay * numDashes;
				}
				gs.ClearTrail ();
				gs.KillSwitchEngage ();
				an.SetBool("Dash", false);
			} else {
				//show dashBar being used up in the HUD
				tmpDashCalc2 = tmpDashCalc - (1 - ((dashTime - dashCounter) / dashTime));
				if (dashCharges == numDashes - 1) {
					ph.ShowDash(dashCharges, 1 - tmpDashCalc2);
				} else {
					if (tmpDashCalc2 > 0) {
						ph.ShowDash(dashCharges + 1, 1 - tmpDashCalc2);
					} else {
						ph.ShowDash(dashCharges, 1 - (1 + tmpDashCalc2));
					}
				}
			}
			return;
		} else if (delayCounter > 0) {				//dash must recharge
			delayCounter -= Time.deltaTime;
			for (int i = 1; i <= numDashes; i++) {	//account for overcharge
				if (delayCounter < (numDashes - i) * dashDelay && delayCounter >= (numDashes - (i + 1)) * dashDelay) {
					dashCharges = i;
				}
			}
			if (dashCharges < numDashes) {
				ph.ShowDash(dashCharges, (delayCounter % dashDelay) / dashDelay);
			} else {
				ph.ShowDash(dashCharges - 1, 0);
			}
		}

		//Start Dash
		if (canDash && !attacking && dashCharges > 0 && Input.GetButtonDown ("Dash")) {
			horiz = Input.GetAxis("Horizontal");
			vert = Input.GetAxis("Vertical");
			pm.canMove = false;
			dashing = true;
			Physics2D.IgnoreLayerCollision(8, 9, true);
			php.damageCollider.enabled = false;
			dashCounter = 0;
			dashCharges--;
			if (dashCharges == numDashes - 1) {
				//ph.ShowDash(dashCharges, 1);
				tmpDashCalc = 1f;
			} else {
				//ph.ShowDash(dashCharges, (delayCounter % dashDelay) / dashDelay);
				tmpDashCalc = 1 - ((delayCounter % dashDelay) / dashDelay);
			}
			//account for dash with no movement input
			if (horiz == 0 && vert == 0) {
				switch (GetDirection ()) {
				case DOWN:
					rb.velocity = Vector2.down * dashSpeed;
					break;
				case LEFT:
					rb.velocity = Vector2.left * dashSpeed;
					break;
				case UP:
					rb.velocity = Vector2.up * dashSpeed;
					break;
				case RIGHT:
					rb.velocity = Vector2.right * dashSpeed;
					break;
				}
			//normal dash
			} else {
				rb.velocity = new Vector2(horiz, vert).normalized * dashSpeed;
			}
			//Dash ghost trail animation
			gs.killSwitch = false;
			an.SetBool("Dash", true);
			return;
		} 
		#endregion

		#region BasicAttack
		if (hasSword) {
			//check for attacking spam input
			if (!attackLock && attacking && Input.GetButtonDown("Attack")) {
				attackLock = true;
				int i = 0;
				for (i = 0; i < COMBONUM; i++) {
					if (attackCombo[i] == true) {
						break;
					}
				}
				attackCombo [i] = false;
				if (i < COMBONUM - 1) {
					attackCombo [i + 1] = true;				//make sure final combo attack doesn't set attackLock to false, else this will break
				}
			}

			//1st Attack
			if (!attacking && canAttack && Input.GetButtonDown("Attack")) {
				attackLock = false;
				attacking = true;
				canAttack = false;
				attackCombo [0] = true;
				pm.canMove = false;
				ps.comboMultiplier = damageMultiplier[0];
				an.SetTrigger ("Attack");
				switch (GetDirection ()) {
				case DOWN:
					rb.velocity = Vector2.down * attackSpeed[0];
					php.damageCollider.offset = new Vector2 (0, damageColliderOffset);
					break;
				case LEFT:
					rb.velocity = Vector2.left * attackSpeed [0];
					php.damageCollider.offset = new Vector2 (damageColliderOffset, 0);
					break;
				case UP:
					rb.velocity = Vector2.up * attackSpeed [0];
					php.damageCollider.offset = new Vector2 (0, -damageColliderOffset);
					break;
				case RIGHT:
					rb.velocity = Vector2.right * attackSpeed [0];
					php.damageCollider.offset = new Vector2 (-damageColliderOffset, 0);
					break;
				}
			} 
					
			//get diagonal input
			if (attacking) {
				horiz = Input.GetAxis("Horizontal");
				vert = Input.GetAxis("Vertical");
			}
	
			//2nd Attack
			if (attackLock && canAttack && attackCombo[1]) {
				attackLock = false;
				canAttack = false;
				ps.comboMultiplier = damageMultiplier[1];
				an.SetTrigger ("Attack");
				switch (GetDirection ()) {
				case DOWN:
						rb.velocity = new Vector2(horiz, -1).normalized * attackSpeed[1];
					break;
				case LEFT:
					rb.velocity = new Vector2(-1, vert).normalized * attackSpeed [1];
					break;
				case UP:
						rb.velocity = new Vector2(horiz, 1).normalized * attackSpeed [1];
					break;
				case RIGHT:
					rb.velocity = new Vector2(1, vert).normalized * attackSpeed [1];
					break;
				}
			}

			//3rd Attack
			if (attackLock && canAttack && attackCombo[2]) {
				attackLock = false;				
					canAttack = false;
				an.SetTrigger ("Attack");
				ps.comboMultiplier = damageMultiplier[2];
					switch (GetDirection ()) {
				case DOWN:
					rb.velocity = new Vector2(horiz / 2, -1).normalized * attackSpeed[2];
					break;
				case LEFT:
					rb.velocity = new Vector2(-1, vert / 2).normalized * attackSpeed [2];
					break;
				case UP:
					rb.velocity = new Vector2(horiz / 2, 1).normalized * attackSpeed [2];
					break;
				case RIGHT:
					rb.velocity = new Vector2(1, vert / 2).normalized * attackSpeed [2];
					break;
				}
			}
		}
		#endregion

		#region Commands
		//check for command input
		if (canCommand && Input.GetButtonDown("UseCommand")) {
			if (commands[selectedCommand].canUse) {
				//Use the selected command
				commands[selectedCommand].UseCommand();
				ph.ChangeCommand();
			} else {
				//show error with sound effect and/or quick animation
			}
		}
		#endregion
	}

	#region HelpfulFunctions

	/// <summary>
	/// Called by animation when the attack combo is ready for the next animation.
	/// </summary>
	public void NextAttackReady () {
		canAttack = true;
	}

	/// <summary>
	/// Changes the current command selection.
	/// </summary>
	public void ChangeCommand () {
		canCommand = true;
		selectedCommand--;
		if (selectedCommand < 0) {
			selectedCommand = commands.Length - 1;
		}
	}

	/// <summary>
	/// Called by the animation to stop the player's forward momentum.
	/// </summary>
	public void StopMovement () {
		rb.velocity = Vector2.zero;
	}

	/// <summary>
	/// Called by animations when an attack completely expires.
	/// </summary>
	public void EndAttack () {
		attacking = false;
		attackLock = true;
		canAttack = true;
		pm.canMove = true;
		ps.comboMultiplier = 1f;
		php.damageCollider.offset = Vector2.zero;
		for (int i = 0; i < COMBONUM; i++) {
			attackCombo [i] = false;
		}
	}

	/// <summary>
	/// Called whenever the player loses his sword, so that he can't attack anymore
	/// </summary>
	public void LostSword () {
		hasSword = false;
		attacking = false;
		attackLock = true;
		canAttack = true;
		pm.canMove = true;
		ps.comboMultiplier = 1f;
		php.damageCollider.offset = Vector2.zero;
		for (int i = 0; i < COMBONUM; i++) {
			attackCombo [i] = false;
		}
	}

	/// <summary>
	/// Call this whenever you want the user to lose all control of the player.
	/// For example, falling down a bottomless pit, the player is dead, etc.
	/// </summary>
	public void DisablePlayer () {
		//Disable Player
		canAttack = false;
		canDash = false;
		canCommand = false;
		pm.canMove = false;

		//Reset everything that might break when player is suddenly disabled
		rb.velocity = Vector2.zero;
		attacking = false;
		attackLock = true;
		php.damageCollider.offset = Vector2.zero;
		ps.comboMultiplier = 1f;
		for (int i = 0; i < COMBONUM; i++) {
			attackCombo [i] = false;
		}
	}

	/// <summary>
	/// Call this whenever you want the user to regain control of the player.
	/// Typically called after DisablePlayer()
	/// </summary>
	public void EnablePlayer () {
		pm.canMove = true;
		canDash = true;
		canAttack = true;
		canCommand = true;
	}

	/// <summary>
	/// Called whenever the player gets back his sword, allowing attacking again
	/// </summary>
	public void RetrieveSword () {
		hasSword = true;
	}

	/// <summary>
	/// Gets the direction the player was last facing from the animator combonent (which is set by PrototypeMovement).
	/// </summary>
	/// <returns>The direction.</returns>
	public int GetDirection() {
		return an.GetInteger ("Direction");
	}
	#endregion
}
