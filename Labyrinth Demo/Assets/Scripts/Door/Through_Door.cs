using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Through_Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		// load new scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield break;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
