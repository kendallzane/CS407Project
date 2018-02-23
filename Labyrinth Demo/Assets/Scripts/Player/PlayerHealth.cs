using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public PolygonCollider2D normalCollider;		//the collider that prevents the player from running into walls
	public PolygonCollider2D damageCollider;		//the collider that checks for new damage taken
	public GameObject healingFX;					//the particle effect of the sword in the ground when healing
	public GameObject healedFX;						//the particle effect of a successful heal

	//variables
	public int maxHealth = 100;						//the maximum health the player can have
	private int health = 100;						//the amount of health the player has
	public float pushBackForce = 3f;				//how much force is the player pushed back with?
    public bool isDead = false;                     //whether player is dead or not

	private int tmpFallDamage;						//assigned to apply damage after fall finishes
	private Vector2 tmpFallLoc;						//por donde el player cayo


	// Use this for initialization
	void Start () {
		pm = GetComponent<PlayerMovement> ();
		pc = GetComponent<PlayerCombat> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
		health = maxHealth;
		healingFX.SetActive (false);
	}

	#region HelpfulFunctions

	/// <summary>
	/// Subtract the damage dealt from the player's total health and update the health bar.
	/// Pushes the player back in the direction he was hit from, gives invincability frames.
	/// </summary>
	/// <param name="damage">Damage Taken.</param>
	/// <param name="dirHit">Direction player was hit from.</param>
	public void TakeDamage (int damage, Vector2 dirHit) {
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
        if (health <= 0 && !isDead) {                        //only die if you already had just a "sliver of health" left
            //YOU DIED!!!!
			damageCollider.enabled = false;
			pc.DisablePlayer ();
            an.SetTrigger("Death");
            PlayerIsDead();
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
		if (health <= 0 && !isDead) {                        //only die if you already had just a "sliver of health" left
			//YOU DIED!!!!
			damageCollider.enabled = false;
			pc.DisablePlayer ();
			an.SetTrigger("Death");
			PlayerIsDead();
		}
	}

	/// <summary>
	/// Call when you want the player to fall down a bottomless pit. The fall animation will trigger, 
	/// and then the player will be respawned at the specified location.
	/// </summary>
	/// <param name="damage">Damage dealt to the player for falling down.</param>
	/// <param name="locationToRespawn">Location to respawn player after falling.</param>
	public void Fall (int damage, Vector2 locationToRespawn) {
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
	}

	/// <summary>
	/// Called by animation, Allow the player to move and attack again.
	/// </summary>
	public void HurtFinished () {
		rb.velocity = Vector2.zero;
		damageCollider.enabled = true;
		pc.EnablePlayer ();
	}

    public void PlayerIsDead() {
        Time.timeScale = 0f;
        isDead = true;
        //pausemenu.SetActive(true);
        //gameover.text = true;
        Destroy(gameObject);
    }
	#endregion

	#region Heal
	/// <summary>
	/// Starts the heal command, halt user input for about a second while the player focuses on healing.
	/// </summary>
	public void StartHeal () {
		an.SetTrigger ("Heal");
		pm.canMove = false;
		pc.canAttack = false;
		rb.velocity = Vector2.zero;
		pc.canCommand = false;
		pc.canDash = false;
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
		pm.canMove = true;
		pc.canAttack = true;
		pc.canCommand = true;
		pc.canDash = true;
	}
	#endregion
}
