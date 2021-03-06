﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlatePuzzle1 : MonoBehaviour {

	GameObject container;

	PPlate pPlate1;
	PPlate pPlate2;
	PPlate pPlate3;

	GameObject cube;

	bool done;

	private GameObject gc;

	// Use this for initialization
	void Start () {
		container = GameObject.Find ("Puzzle 1");

		PPlate[] temp = GetComponentsInChildren<PPlate> ();
		pPlate1 = temp [0];
		pPlate2 = temp [1];
		pPlate3 = temp [2];

		//cube = GameObject.Find ("cube");

		cube = container.transform.Find ("cube").gameObject;
		gc = GameObject.Find ("GameController");

		done = true;
		if (gc.GetComponent<GameController> ().roomPuzzle [9] >= 1) {
			cube.SetActive (false);
			done = false;
		}

	}

	// Update is called once per frame
	void Update () {
		if (pPlate2.getIsOn()) {
			if (pPlate1.getIsOn () && !(pPlate3.getIsOn ())) {
				pPlate1.off ();
				pPlate2.off ();
			}
			if (pPlate3.getIsOn () && pPlate1.getIsOn () && done) {
				Destroy (cube);
				gc.GetComponent<GameController> ().roomPuzzle [9] = 1;
				done = false;
			}
		}
		if (pPlate1.getIsOn() && !(pPlate2.getIsOn())) {
			pPlate1.off ();
		}
		if (pPlate3.getIsOn() && !(pPlate2.getIsOn())) {
			pPlate3.off ();
		}
	}
}



