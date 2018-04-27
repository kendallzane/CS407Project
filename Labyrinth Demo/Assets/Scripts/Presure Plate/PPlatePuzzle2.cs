using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlatePuzzle2 : MonoBehaviour {

	GameObject container;
	PPlate[] pPlates;
	GameObject cube;
	int currentPPlate;

	bool done;

	private GameObject gc;

	// Use this for initialization
	void Start () {
		container = GameObject.Find ("Puzzle 2");
		pPlates = GetComponentsInChildren<PPlate> ();
		for (int i = 1; i < pPlates.Length; i++) {
			pPlates [i].gameObject.SetActive (false);
		}
		currentPPlate = 0;

		cube = container.transform.Find ("cube").gameObject;
		gc = GameObject.Find ("GameController");


		done = true;
		if (gc.GetComponent<GameController> ().roomPuzzle [9] >= 2) {
			cube.SetActive (false);
			done = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//pPlates [currentPPlate].gameObject.SetActive (false);
		if (currentPPlate < pPlates.Length - 1 && pPlates [currentPPlate].getIsOn ()) {
			//pPlates [currentPPlate].off ();
			pPlates [currentPPlate + 1].gameObject.SetActive (true);
			currentPPlate++;
		}
		if (pPlates[currentPPlate].getIsOn() && done) {
			gc.GetComponent<GameController> ().roomPuzzle [9] = 2;
			done = false;
		}
	}
}
