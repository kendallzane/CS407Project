using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {

	//constants

	//references
	public Sprite[] upgradeSprites;								//the look of each upgrade
	private SpriteRenderer sr;

	//variables
	public int[] baseDamage;									//how much base damage does each attack do? (Based on equipped weapon)
	public int upgrade = 0;										//which upgrade of the weapon is it? 
	[HideInInspector] public float comboMultiplier = 1f;		//multiplier depending on what part of the combo the player is in

	//make sure the weapon look matches the given upgrade
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = upgradeSprites [upgrade];
	}

	/// <summary>
	/// Upgrades to the sword to its next level.
	/// </summary>
	public void UpgradeToNextLevel () {
		upgrade++;
		if (upgrade < upgradeSprites.Length) {
			GetComponent<SpriteRenderer> ().sprite = upgradeSprites [upgrade];
		} else {
			Debug.Log ("Sword already at max level!");
		}
	}

	//Damage enemies on contact
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Enemy") {
			coll.GetComponent<EnemyAI> ().TakeDamage ((int) (baseDamage[upgrade] * comboMultiplier), (Vector2) coll.transform.position - (Vector2) transform.position);
		}
	}
}
