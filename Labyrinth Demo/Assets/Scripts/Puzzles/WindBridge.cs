using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBridge : MonoBehaviour {

	public GameObject[] bridges;

	// Use this for initialization
	void Start () {
		foreach (GameObject bridge in bridges) {
			bridge.SetActive (false);
		}
		bridges [GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().windBridgeState].SetActive (true);
	}
}
