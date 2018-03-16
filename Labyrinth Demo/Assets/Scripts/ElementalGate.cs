using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalGate : MonoBehaviour {

	//constants
	const int BASE = 0;
	const int EARTH = 1;
	const int FIRE = 2;
	const int WATER = 3;
	const int WIND = 4;

	//references

	//variables
	public int elementalType = 1;				//Based on the constants above

	// Use this for initialization
	void Start () {
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			if (gcObj.GetComponent<GameController> ().bossDefeats [elementalType]) {
				Destroy (gameObject);
			}
		}
	}
}
