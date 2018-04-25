using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject item;

	private GameObject[] enemies;
	private bool[] enemiesDead;
	private int numEnemies;

	// Use this for initialization
	void Start () {
		item.SetActive (false);

		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		numEnemies = enemies.Length;
		enemiesDead = new bool[numEnemies];
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
		}
	}
}
