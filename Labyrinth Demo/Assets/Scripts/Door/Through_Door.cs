using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Through_Door : MonoBehaviour {

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		// load new scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		yield break;
	}
}
