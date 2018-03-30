using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapPlatform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerMovement> ().platformFallSafe = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerMovement> ().platformFallSafe = false;
		}
	}
}
