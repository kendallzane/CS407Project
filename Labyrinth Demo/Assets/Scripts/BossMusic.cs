using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<BackgroundMusic> ().PlayBossTheme();
	}
}
