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
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>().DefeatBoss(element);
			GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
			foreach (GameObject door in doors) {
				door.GetComponent<Animator> ().SetTrigger ("Open");
			}
			Destroy (gameObject);
		}
	}
}
