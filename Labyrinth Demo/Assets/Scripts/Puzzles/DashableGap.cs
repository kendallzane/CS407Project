using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashableGap : MonoBehaviour {

	//variables
	public Vector2[] respawnPoints;
	public int damage = 10;

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && !coll.GetComponent<PlayerMovement> ().fallSafe) {
			coll.GetComponent<PlayerHealth>().Fall (damage, respawnPoints[GetClosestPoint((Vector2) coll.gameObject.transform.position)]);
		}
	}

	int GetClosestPoint (Vector2 oldPos) {
		int returnable = 0;
		float shortestLength = 1000f;
		for (int i = 0; i < respawnPoints.Length; i++) {
			if ((respawnPoints [i] - oldPos).magnitude < shortestLength) {
				shortestLength = (respawnPoints [i] - oldPos).magnitude;
				returnable = i;
			}
		}
		return returnable;
	}
}
