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
	public PolygonCollider2D damageCollider;		//the collider that checks for new damage taken
	public GameObject healingFX;					//the particle effect of the sword in the ground when healing
	public GameObject healedFX;						//the particle effect of a successful heal

	//variables
	public int maxHealth = 100;						//the maximum health the player can have
	private int health = 100;						//the amount of health the player has
	public float pushBackForce = 3f;				//how much force is the player pushed back with?

	// Use this for initialization
	void Start () {
		pm = GetComponent<PlayerMovement> ();
		pc = GetComponent<PlayerCombat> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
		health = maxHealth;
		healingFX.SetActive (false);
	}

	/// <summary>
	/// Subtract the damage dealt from the player's total health and update the health bar.
	/// </summary>
	/// <param name="damage">Damage dealt.</param>
	public void TakeDamage (int damage, Vector2 dirHit) {
		if (health > 0) {
			health -= damage;
			if (health < 0) {
				health = 0;
			}
			damageCollider.enabled = false;
			ph.ShowHealth (health);
			an.SetTrigger ("Hurt");
			pc.EndAttack ();			//stop player attacks
			pc.canAttack = false;
			pm.canMove = false;
			healingFX.SetActive (false);
			pc.canDash = false;
			pc.canCommand = false;
			rb.velocity = Vector2.zero;
			rb.AddForce (dirHit.normalized * pushBackForce);
		} else {                        //only die if you already had just a "sliver of health" left
            //YOU DIED!!!!
            an.SetTrigger("Death");
		}
	}

	/// <summary>
	/// Called by animation, Allow the player to move and attack again.
	/// </summary>
	public void HurtFinished () {
		pm.canMove = true;
		pc.canAttack = true;
		rb.velocity = Vector2.zero;
		damageCollider.enabled = true;
		pc.canDash = true;
		pc.canCommand = true;
	}

    public void PlayerIsDead() {
        Time.timeScale = 0f;
        //pausemenu.SetActive(true);
        //gameover.text = true;
        Destroy(gameObject);
    }

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
