using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleFalling : MonoBehaviour {

	public int fallingDamage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerHealth>().TakeDamage (fallingDamage, new Vector2(0,0));
			coll.transform.position = new Vector3 (0,0,0);
		} 
	}

}
