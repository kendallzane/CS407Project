using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSwitch : MonoBehaviour {

	//constants

	//references
	private Animator an;

	//variables
	private bool switchState = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Switch () {
		
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Sword") {
			
		}
	}
}
