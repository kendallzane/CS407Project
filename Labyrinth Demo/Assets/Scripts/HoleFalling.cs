using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleFalling : MonoBehaviour {

	public int fallingDamage;
	public Vector2 locationToRespawn;

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerHealth>().Fall (fallingDamage, locationToRespawn);
		} 
	}

}
