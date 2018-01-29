using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraClamper : MonoBehaviour {

	//references
	public PrototypeFollow theCamera;

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player") {
			theCamera.clamped = true;
		}
	}

	void OnTriggerExit2D (Collider2D coll) {
		if (coll.tag == "Player") {
			theCamera.clamped = false;
		}
	}
}
