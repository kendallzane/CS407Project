using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

	private Animator an;

	// Use this for initialization
	void Start () {
		an = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
	}

	private void airLock() {
		an.SetBool ("airUnlocked", true);
	}

	private void earthLock() {
		an.SetBool ("earthUnlocked", true);
	}

	private void fireLock() {
		an.SetBool ("fireUnlocked", true);
	}

	private void waterLock() {
		an.SetBool ("waterUnlocked", true);
	}
}
