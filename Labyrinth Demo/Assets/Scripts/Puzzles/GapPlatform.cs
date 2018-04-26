using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapPlatform : MonoBehaviour {

	public Sprite s1, s2;
	public bool active = true;
	
	void Start () {
		if (active) {
			GetComponent<SpriteRenderer>().sprite = s1;
		} else {
			GetComponent<SpriteRenderer>().sprite = s2;
		}
	}
	
	public void Flip () {
		active = !active;
		if (active) {
			GetComponent<SpriteRenderer>().sprite = s1;
			GetComponent<BoxCollider2D>().enabled = true;
		} else {
			GetComponent<SpriteRenderer>().sprite = s2;
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
		
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Player" && active) {
			coll.GetComponent<PlayerMovement> ().platformFallSafe = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.tag == "Player" && !coll.isTrigger) {
			coll.GetComponent<PlayerMovement> ().platformFallSafe = false;
		}
	}
}
