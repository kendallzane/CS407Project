using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLayerShifter : MonoBehaviour {

	public int layer;

	// Use this for initialization
	void Start () {
		GameObject gc = GameObject.FindGameObjectWithTag("GameController");
		if (gc != null) {
			gc.GetComponent<BackgroundMusic> ().SwitchLayers (layer);
		}
	}
}
