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

	//variables
	public int health = 100;						//the amount of health the player has
	public float pushBackForce = 3f;				//how much force is the player pushed back with?

	// Use this for initialization
	void Start () {
		pm = GetComponent<PlayerMovement> ();
		pc = GetComponent<PlayerCombat> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
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
			rb.velocity = Vector2.zero;
			rb.AddForce (dirHit.normalized * pushBackForce);
		} else {						//only die if you already had just a "sliver of health" left
			//YOU DIED!!!!
		}
	}

	/// <summary>
	/// Allow the player to move and attack again.
	/// </summary>
	public void HurtFinished () {
		pm.canMove = true;
		pc.canAttack = true;
		rb.velocity = Vector2.zero;
		damageCollider.enabled = true;
	}
}
