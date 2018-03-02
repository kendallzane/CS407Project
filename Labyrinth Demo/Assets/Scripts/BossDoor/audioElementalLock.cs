using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class audioElementalLock : MonoBehaviour {

	AudioSource audio1;
	AudioSource audio2;

	private GameObject player;
	private GameObject hud;
	private GameObject gc;
	public GameObject mainCamera;
	public GameObject cutSceneCamera;

	// Use this for initialization
	void Start () {
		AudioSource[] audios = GetComponents<AudioSource> ();
		audio1 = audios [0];
		audio2 = audios [1];

		player = GameObject.Find ("MainCharacter");
		hud = GameObject.FindGameObjectWithTag ("HUD");
		gc = GameObject.Find ("GameController");


		//mainCamera = GameObject.Find ("Main Camera");
		//cutSceneCamera = GameObject.Find ("cutSceneCamera");
		//cutSceneCamera.disable
	}

	public void playAudio1() {
		audio1.enabled = true;
		audio1.Play ();
		gc.GetComponentInChildren<PauseMenu> ().enabled = false;
		player.GetComponent<PlayerCombat>().DisablePlayer();
		hud.SetActive (false);
		mainCamera.gameObject.SetActive(false);
		cutSceneCamera.gameObject.SetActive(true);
		Time.timeScale = 0f;
	}

	public void playAudio2() {
		audio2.enabled = true;
		audio2.Play ();
	}

	public void stopAudio1() {
		audio1.enabled = false;
		/*
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		mainCamera.gameObject.SetActive(true);
		cutSceneCamera.gameObject.SetActive(false);
		Time.timeScale = 1f;
		*/
	}

	public void stopAudio2() {
		audio2.enabled = false;
		/*
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		mainCamera.gameObject.SetActive(true);
		cutSceneCamera.gameObject.SetActive(false);
		Time.timeScale = 1f;
		*/
	}

}
