using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private Animator an;
	bool close;
	bool open;

	// Use this for initialization
	void Start () {
		an = GetComponentInChildren <Animator> ();
		open = false;
		close = false;
	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			close = true;
			yield break;
		}
		yield break;
	}

	IEnumerator OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			close = false;
			yield break;
		}
		yield break;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("f") && close) {
			if (open) {
				an.SetBool("open", false);
				open = false;
			} else {
				an.SetBool("open", true);
				open = true;
			}
		}
	}
}
