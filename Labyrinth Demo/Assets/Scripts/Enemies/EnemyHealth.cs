using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to be put on every enemy so that they can be damaged
public class EnemyHealth : MonoBehaviour {

	//variables
	public float health = 100;									//how much health does the enemy have?

	
	public void OnDeath() {
		Debug.Log("enemy est mort");
	}
	
	/// <summary>
	/// Inflicts damage on the enemy.
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void TakeDamage (int damage) {
		health -= damage;
		if (health <= 0) {
			//ENEMY KILLED! TODO: Make death animations for enemies
			EnemyAI ai = gameObject.GetComponent<EnemyAI>();
			StartCoroutine(ai.OnDeath());
		}
	}
}
