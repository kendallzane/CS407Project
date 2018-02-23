using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWind : MonoBehaviour {

	public float windForce;
	public bool facingDown = true;
	private bool containsPlayer;

	private GameObject player;

	// Use this for initialization
	void Start () {
		containsPlayer = false;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
		if (containsPlayer) {
			Vector3 deltaPos = (player.transform.position - this.transform.position);

			//Inversion
			deltaPos = new Vector3 (1f / deltaPos.x, 1f / deltaPos.y, deltaPos.z);

			deltaPos.Scale (new Vector3 (windForce, windForce, 1));

			//Only apply in direction of wind travel
			if (facingDown) {
				//vertical wind	
				deltaPos.x = 0;
			} else {
				//horizontal wind
				deltaPos.y = 0;
			}
			deltaPos.z = 0;
			player.transform.position = player.transform.position + deltaPos;
		}
		containsPlayer = false;
	}

	void OnTriggerStay2D(Collider2D coll) {
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
