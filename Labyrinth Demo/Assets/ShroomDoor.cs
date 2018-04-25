using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			Destroy(gameObject);
		}
	}
}
