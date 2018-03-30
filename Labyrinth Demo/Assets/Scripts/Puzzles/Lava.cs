using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour {

	public int damage = 10;	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && coll.isTrigger) {
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
	}
}
