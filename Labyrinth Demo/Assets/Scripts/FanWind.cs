using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWind : MonoBehaviour {

	public float windForce;
	private bool containsPlayer;

	// Use this for initialization
	void Start () {
		containsPlayer = false;
	}

	// Update is called once per frame
	void Update () {
		if (containsPlayer) {
			GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
			Vector3 deltaPos = (player.transform.position - this.transform.position);
			deltaPos.Scale (new Vector3 (windForce, windForce, 1));
			deltaPos.z = 0;
			player.transform.position = player.transform.position + deltaPos;
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Player") {
			containsPlayer = true;
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			containsPlayer = false;
		}
	}

}
