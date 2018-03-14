using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Through_Door : MonoBehaviour {

	public int toScene = 0;
	public int toEntrance = 0;

	private bool loading = false;

	void Start () {
		loading = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		// load new scene if player enters
		if (other.tag == "Player" && !loading) {
			loading = true;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<Transition> ().TransitionToScene(toScene, toEntrance);
		}
	}
}
