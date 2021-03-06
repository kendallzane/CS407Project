﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

	//constants
	const int BASE = 0;
	const int EARTH = 1;
	const int FIRE = 2;
	const int WATER = 3;
	const int WIND = 4;
	const int BOSS = 5;
	const int ALL = 6;

	//references
	private AudioSource[] aus;
	public AudioClip bossIntro;
	[HideInInspector] public int currLayer = BASE;			//what layer is the player currently in?

	//variables
	public float standaloneBaseVolume = 1;
	public float baseVolume = 0.6f;
	public float layerVolume = 1;
	public float sFXVolume = 1;

	// Use this for initialization
	void Start () {
		aus = GetComponents<AudioSource> ();
		aus [BASE].volume = standaloneBaseVolume;
		aus [EARTH].volume = 0;
		aus [FIRE].volume = 0;
		aus [WATER].volume = 0;
		aus [WIND].volume = 0;
		aus [BOSS].volume = 0;
	}

	/// <summary>
	/// Switch the music to the boss battle music
	/// </summary>
	public void PlayBossTheme () {
		aus [BASE].volume = 0;
		aus [EARTH].volume = 0;
		aus [FIRE].volume = 0;
		aus [WATER].volume = 0;
		aus [WIND].volume = 0;
		aus [BOSS].volume = layerVolume;
		aus [BOSS].PlayOneShot (bossIntro);
		aus [BOSS].PlayDelayed (bossIntro.length);
	}

	/// <summary>
	/// Switch to the proper layer theme.
	/// </summary>
	/// <param name="layer">Layer.</param>
	public void SwitchLayers (int layer) {
		currLayer = layer;
		GetComponent<GameController> ().currLayer = layer;

		//default to all off
		aus [BASE].volume = baseVolume;
		aus [EARTH].volume = 0;
		aus [FIRE].volume = 0;
		aus [WATER].volume = 0;
		aus [WIND].volume = 0;
		aus [BOSS].volume = 0;

		switch (layer) {
		case BASE:
			aus [BASE].volume = standaloneBaseVolume;
			break;
		case EARTH:
			aus [EARTH].volume = layerVolume;
			break;
		case FIRE:
			aus [FIRE].volume = layerVolume;
			break;
		case WATER:
			aus [WATER].volume = layerVolume;
			break;
		case WIND:
			aus [WIND].volume = layerVolume;
			break;
		case ALL:
			aus [BASE].volume = standaloneBaseVolume;
			aus [EARTH].volume = layerVolume;
			aus [FIRE].volume = layerVolume;
			aus [WATER].volume = layerVolume;
			aus [WIND].volume = layerVolume;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		//will need to timeDelay to smoothly crossfade between songs
	}
}
