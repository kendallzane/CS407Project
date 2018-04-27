using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHealth : MonoBehaviour {

	//constants
	private const int DOWN = 0;						//direction
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references
	private Rigidbody2D rb;
	private Animator an;
	private PlayerMovement pm;
	[HideInInspector] public PlayerHUD ph;
	private PlayerCombat pc;
	//public PolygonCollider2D normalCollider;		//the collider that prevents the player from running into walls
	//public PolygonCollider2D damageCollider;		//the collider that checks for new damage taken
	public BoxCollider2D normalCollider;		//the collider that prevents the player from running into walls
	public BoxCollider2D damageCollider;		//the collider that checks for new damage taken
	
	public GameObject healingFX;					//the particle effect of the sword in the ground when healing
	public GameObject healedFX;						//the particle effect of a successful heal
	public GameObject soul;							//GameObject to spawn after death

	//variables
	public int maxHealth = 100;						//the maximum health the player can have
	public int extraHealthOnUpgrade = 50;			//the increase to maximum health per upgrade given
	private int health = 100;						//the amount of health the player has
	public float pushBackForce = 3f;				//how much force is the player pushed back with?
    public bool isDead = false;                     //whether player is dead or not
	public float invincibilityFramesTime = 1.0f;	//how much extra time does the player have to work with?

	private int tmpFallDamage;						//assigned to apply damage after fall finishes
	private Vector2 tmpFallLoc;						//por donde el player cayo
	[HideInInspector] public bool invincibilityFrames;	//is the player invincibal right after getting hit? 
	public float timeDelay = 0;
	
	public Tilemap tm;
	public Grid gr;
	public string lastTile;
	private Vector2 lastGoodPosition;
	public int fallDamage = 10;
	public bool falling = false;


	// Use this for initialization
	void Start () {
		pm = GetComponent<PlayerMovement> ();
		pc = GetComponent<PlayerCombat> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
		tm = GameObject.Find("Grid/Floor").GetComponent<Tilemap>();
		gr = GameObject.Find("Grid").GetComponent<Grid>();
		//health = maxHealth;
		ph.SetMaxHealth (maxHealth);
		ph.ShowHealth(health);
		healingFX.SetActive (false);
		invincibilityFrames = false;
		timeDelay = invincibilityFramesTime;
	}

	// Called every frame
	void Update()
	{
		if (invincibilityFrames) {
			timeDelay -= Time.deltaTime;
			if (timeDelay <= 0) {
				damageCollider.enabled = true;
				invincibilityFrames = false;
				timeDelay = invincibilityFramesTime;
			}
		}
		string currentTile = "";
		if (gr != null && tm != null && transform != null) {
			if (tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0f, -0.18f, 0f))) != null) {
				currentTile = tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0f, -0.18f, 0f))).name; 
			}
		} 
		
		if (currentTile != null && lastTile != currentTile) {
			//Debug.Log("Player tile change: " + lastTile + " to " + currentTile + "		" + Time.time);
			lastTile = currentTile;
		}
			
		if (lastTile.Contains("Hole") || lastTile == "") {
				//check at some other locations on the sprite
				string lastName = tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0f, 0.07f, 0f))).name;
				if ((tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0f, 0.07f, 0f))).name.Contains("Hole")
				|| tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0f, 0.07f, 0f))).name.Contains("mb_"))
				&& tm.GetTile(gr.WorldToCell(transform.position + new Vector3(0.06f, 0.00f, 0f))).name.Contains("Hole") 
				&& tm.GetTile(gr.WorldToCell(transform.position + new Vector3(-0.06f, 0.00f, 0f))).name.Contains("Hole")) 
				{
					Fall(fallDamage, lastGoodPosition);
				} else {
					//lastGoodPosition = transform.position;
				}
		} else {
			lastGoodPosition = transform.position;
		}
	}
		
	

	#region HelpfulFunctions

	/// <summary>
	/// Subtract the damage dealt from the player's total health and update the health bar.
	/// Pushes the player back in the direction he was hit from, gives invincability frames.
	/// </summary>
	/// <param name="damage">Damage Taken.</param>
	/// <param name="dirHit">Direction player was hit from.</param>
	public void TakeDamage (int damage, Vector2 dirHit) {
		if (falling) {
			return;
		}
		if (damageCollider.enabled) {
			if (health > 0) {
				health -= damage;
				if (health < 0) {
					health = 0;
				}
				damageCollider.enabled = false;
				ph.ShowHealth (health);
				pc.DisablePlayer ();
				an.SetTrigger ("Hurt");
				healingFX.SetActive (false);
				rb.AddForce (dirHit.normalized * pushBackForce);
			}
			else if (health <= 0 && !isDead) {                        //only die if you already had just a "sliver of health" left
				//YOU DIED!!!!
				/*
				damageCollider.enabled = false;
				pc.DisablePlayer ();
				an.SetTrigger("Death");
				rb.isKinematic = true;
				*/
				damageCollider.enabled = false;
				ph.ShowHealth (health);
				pc.DisablePlayer ();
				an.SetTrigger ("Hurt");
				healingFX.SetActive (false);
				rb.AddForce (dirHit.normalized * pushBackForce);
			}
		}
	}

	/// <summary>
	/// DOES NOT give invinciblity frames, NOT intended for normal damage. Overloaded version.
	/// Subtract the damage dealt from the player's total health and update the health bar.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void TakeDamage (int damage) {
		
		if (health > 0) {
			health -= damage;
			if (health < 0) {
				health = 0;
			}
			ph.ShowHealth (health);
			healingFX.SetActive (false);
		}
		//please keep this else
		else if (!isDead) {                        //ONLY die if you already had just a "sliver of health" left
			//YOU DIED!!!!
			
			/*
			damageCollider.enabled = false;
			pc.DisablePlayer ();
			an.SetTrigger("Death");
			rb.isKinematic = true;
			*/
			ph.ShowHealth (health);
			healingFX.SetActive (false);
		}
	}

	/// <summary>
	/// Call when you want the player to fall down a bottomless pit. The fall animation will trigger, 
	/// and then the player will be respawned at the specified location.
	/// </summary>
	/// <param name="damage">Damage dealt to the player for falling down.</param>
	/// <param name="locationToRespawn">Location to respawn player after falling.</param>
	public void Fall (int damage, Vector2 locationToRespawn) {
		if (falling || pc.dashing || pm.platformFallSafe) {
			return;
		}
		falling = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll; //so you don't dash into a hole and "fall" while moving
		pc.DisablePlayer ();
		normalCollider.enabled = false;
		damageCollider.enabled = false;
		an.SetTrigger ("Fall");
		tmpFallDamage = damage;
		tmpFallLoc = locationToRespawn;
	}

	/// <summary>
	/// Finishes the fall animation, ONLY CALLED BY ANIMATION.
	/// </summary>
	public void FallFinished () {
		transform.position = new Vector3 (tmpFallLoc.x, tmpFallLoc.y, transform.position.z);
		pc.EnablePlayer ();
		damageCollider.enabled = true;
		normalCollider.enabled = true;
		TakeDamage (tmpFallDamage);
		falling = false;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		invincibilityFrames = true;
	}

	/// <summary>
	/// Called by animation, Allow the player to move and attack again.
	/// </summary>
	public void HurtFinished () {
		rb.velocity = Vector2.zero;
		invincibilityFrames = true;
		pc.EnablePlayer ();
	}

	/// <summary>
	/// Upgrades the maximum health of the player.
	/// </summary>
	public void UpgradeHealth () {
		maxHealth += extraHealthOnUpgrade;
		health = maxHealth;
		ph.SetMaxHealth (maxHealth);
		ph.ShowHealth (health);
	}

	/// <summary>
	/// Gets the current health of the player.
	/// </summary>
	/// <returns>The health.</returns>
	public int GetHealth () {
		return health;
	}

	/// <summary>
	/// Sets the current health of the player.
	/// </summary>
	/// <param name="newHealth">New health.</param>
	public void SetHealth (int newHealth) {
		health = newHealth;
	}

    public void PlayerIsDead() {
		isDead = true;
        Time.timeScale = 0f;
		Instantiate (soul, gameObject.transform.position, Quaternion.identity);
		Destroy (ph.gameObject);
        Destroy(gameObject);
    }
	#endregion

	#region Heal
	/// <summary>
	/// Starts the heal command, halt user input for about a second while the player focuses on healing.
	/// </summary>
	public void StartHeal () {
		an.SetTrigger ("Heal");
		pc.DisablePlayer ();
	}

	/// <summary>
	/// Called by animation, Starts the healing particle effect.
	/// </summary>
	public void StartHealingFX () {
		healingFX.SetActive (true);
	}

	/// <summary>
	/// Called by animation, The player has healed correctly, restore player health, user can input as soon as the player gets back up.
	/// </summary>
	public void Heal () {
		health = maxHealth;
		ph.ShowHealth (health);
		healingFX.SetActive (false);
		Instantiate (healedFX, transform.position, Quaternion.identity);
	}

	/// <summary>
	/// Called by animation, The heal animation is finished, allow player input
	/// </summary>
	public void HealFinished () {
		pc.EnablePlayer ();
	}
	#endregion
}
