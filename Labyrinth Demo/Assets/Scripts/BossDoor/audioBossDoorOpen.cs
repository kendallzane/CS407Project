using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioBossDoorOpen : MonoBehaviour {

	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}

	public void playAudio() {
		audio.enabled = true;
		audio.Play ();
	}

	public void stopAudio() {
		audio.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
