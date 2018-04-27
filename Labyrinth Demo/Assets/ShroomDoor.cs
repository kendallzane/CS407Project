using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomDoor : MonoBehaviour {

	private GameObject gc;

	// Use this for initialization
	void Start () {
		if (gc.GetComponent<GameController> ().roomPuzzle [20] >= 1) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			Destroy(gameObject);
			gc.GetComponent<GameController> ().roomPuzzle [20] = 1;
		}
	}
}
