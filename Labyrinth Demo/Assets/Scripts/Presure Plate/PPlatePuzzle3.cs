﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlatePuzzle3 : MonoBehaviour {

	PPlate pPlateView;
	PPlate[] pPlates;
	GameObject container;
	GameObject cube;

	bool done;

	private GameObject gc;


	// Use this for initialization
	void Start () {
		container = GameObject.Find ("Puzzle 3");
		pPlates = GetComponentsInChildren<PPlate> ();
		pPlateView = pPlates [0];

		/* initial set invisible is throwing a null exception -- unknown why.
		for (int i = 1; i < pPlates.Length - 1; i++) {
			pPlates [i].invisible ();
		}
		*/

		pPlates [5].gameObject.SetActive (false);

		cube = container.transform.Find ("cube").gameObject;

		gc = GameObject.Find ("GameController");

		done = true;
		if (gc.GetComponent<GameController> ().roomPuzzle [9] >= 3) {
			cube.SetActive (false);
			done = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (pPlateView.getIsOn ()) {
			for (int i = 1; i < pPlates.Length - 1; i++) {
				if (!pPlates [i].getIsOn ()) {
					pPlates [i].visible ();
				}
			}
		} else {
			for (int i = 1; i < pPlates.Length - 1; i++) {
				if (!pPlates [i].getIsOn ()) {
					pPlates [i].invisible ();
				}
			}
		}

		if (pPlates [1].getIsOn () && pPlates [2].getIsOn () && pPlates [3].getIsOn () && pPlates [4].getIsOn ()) {
			pPlates [5].gameObject.SetActive (true);
		}

		if (pPlates [5].getIsOn () && done) {
			gc.GetComponent<GameController> ().roomPuzzle [9] = 3;
			done = false;
		}
	}
}
