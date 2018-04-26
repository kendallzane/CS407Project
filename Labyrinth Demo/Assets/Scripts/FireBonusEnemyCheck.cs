using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBonusEnemyCheck : MonoBehaviour {

	GameObject container;

	GameObject cube;

	//private GameObject gc;

	// Use this for initialization
	void Start () {
		container = GameObject.Find ("Puzzle 5");
		cube = container.transform.Find ("cube").gameObject;
    }

    // Update is called once per frame
    void Update () {

        if (GameObject.FindWithTag("Enemy") != null)
        {
            // Enemies exist

        }
        else
        {
            // No enemies left
            Destroy(cube);
        }
    }
}