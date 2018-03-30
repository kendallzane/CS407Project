﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSwitch : MonoBehaviour {

	//constants

	//references
	private Animator an;
	private GameController gc;

	//variables
	public int switchNum = 0;
	private bool switchState = false;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator> ();
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj == null) {
			return;
		}
		gc = gcObj.GetComponent<GameController> ();
		switchState = gc.rotationSwitchStates[switchNum];
		if (switchState == true) {
			an.SetTrigger ("Other");
		}
	}

	/// <summary>
	/// Switch this the state of this switch, and rotate the wind bridge.
	/// </summary>
	void Switch () {
		if (switchState) {
			//CLOCKWISE

		} else {
			//COUNTERCLOCKWISE

		}
		switchState = !switchState;
		if (gc != null) {
			gc.rotationSwitchStates [switchNum] = switchState;
		}
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.gameObject.GetComponent<PlayerSword>() != null) {
			an.SetTrigger ("Switch");
		}
	}
}