using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public AudioClip[] soundEffects;
	private float volume;
	private AudioSource aus;

	// Use this for initialization
	void Start () {
		aus = GetComponent<AudioSource> ();
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			volume = gcObj.GetComponent<BackgroundMusic> ().sFXVolume;
		} else {
			volume = 1;
		}
	}

	/// <summary>
	/// Plays the specified sound.
	/// </summary>
	/// <param name="soundNum">Number of sound in Sound Effects array.</param>
	public void playSound (int soundNum) {
		aus.PlayOneShot (soundEffects [soundNum], volume);
	}
}
