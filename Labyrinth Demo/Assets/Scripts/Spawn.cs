using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Spawn : MonoBehaviour {

	public GameObject item;

	private GameObject[] enemies;
	private bool[] enemiesDead;
	private int numEnemies;


	private int sceneN;
	private GameObject gc;

	// Use this for initialization
	void Start () {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		numEnemies = enemies.Length;
		enemiesDead = new bool[numEnemies];

		Scene c = SceneManager.GetActiveScene();
		if (c.name == "Earth1") {
			sceneN = 6;
		} else if (c.name == "Earth2") {
			sceneN = 7;
		} else if (c.name == "Earth3") {
			sceneN = 8;
		}

		gc = GameObject.Find ("GameController");

		if (gc.GetComponent<GameController> ().roomPuzzle [sceneN] == 1) {
			foreach (GameObject enemy in enemies) {
				Destroy (enemy);
			}
			GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
			foreach (GameObject door in doors) {
				door.GetComponent<Animator> ().SetTrigger ("Open");

			}
		} else {
			item.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numEnemies; i++) {
			if (enemies [i] == null) {
				enemiesDead [i] = true;
			}
		}
		bool done = true;
		for (int i = 0; i < numEnemies; i++) {
			done = done && enemiesDead [i];
		}
		if (done) {
			item.SetActive (true);
			GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
			foreach (GameObject door in doors) {
				door.GetComponent<Animator> ().SetTrigger ("Open");
			}
			gc.GetComponent<GameController> ().roomPuzzle [sceneN] = 1;
		}
	}
}
