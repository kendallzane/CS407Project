using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpBossDefeater : MonoBehaviour {

	public int element;

	void Start () {
		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		foreach (GameObject door in doors) {
			door.GetComponent<Animator> ().SetTrigger ("Close");
		}
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Sword") {
			GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
			gcObj.GetComponent<GameController>().DefeatBoss(element);
			gcObj.GetComponent<BackgroundMusic> ().SwitchLayers (gcObj.GetComponent<BackgroundMusic> ().currLayer);
			GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
			foreach (GameObject door in doors) {
				door.GetComponent<Animator> ().SetTrigger ("Open");
			}
			Destroy (gameObject);
		}
	}
}
