using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

	private Animator an;

	private GameObject player;
	private GameObject hud;
	private GameObject gc;

	public int elementType = 0;			//1 for Earth, 2 for Fire, 3 for Water, 4 for Wind
	public GameObject mainCamera;
	public GameObject cutSceneCamera;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator> ();
		player = GameObject.Find ("MainCharacter");
		hud = GameObject.FindGameObjectWithTag ("HUD");
		gc = GameObject.Find ("GameController");

		// called if lock has been opened before
		for (int i = 0; i < 5; i++) {
			disablePlayer ();
			if (gc != null && gc.GetComponent<GameController> ().ElementalLocks[i]) {
				switch (i) {
				case 0:
					break;
				case 1:
					if (elementType == 1) {
						gameObject.GetComponent<Animator> ().enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().enabled = false;
					} 

					if (elementType == 0) {
						an.SetBool ("earthUnlocked", true);
					}
					break;
				case 2:
					if (elementType == 2) {
						gameObject.GetComponent<Animator> ().enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().enabled = false;

					}
					if (elementType == 0) {
						an.SetBool ("fireUnlocked", true);
					}
					break;
				case 3:
					if (elementType == 3) {
						gameObject.GetComponent<Animator> ().enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().enabled = false;

					}
						if (elementType == 0) {
						an.SetBool ("waterUnlocked", true);
					}
					break;
				case 4:
					if (elementType == 4) {
						gameObject.GetComponent<Animator> ().enabled = false;
						gameObject.GetComponent<SpriteRenderer> ().enabled = false;

					}
						if (elementType == 0) {
						an.SetBool ("airUnlocked", true);
					}
					break;
				}
			}
			enablePlayer ();
		}

		// called if lock has not been opened before
		for (int i = 0; i < 5; i++) {
			disablePlayer ();
			if (gc != null && gc.GetComponent<GameController> ().bossDefeats [i] && !gc.GetComponent<GameController> ().ElementalLocks[i]) {
				switch (i) {
				case 0:
					break;
				case 1:
					if (elementType == 1) {
						gc.GetComponent<GameController> ().ElementalLockOpen (1);
						an.SetBool ("earthUnlocked", true);
					}
					break;
				case 2:
					if (elementType == 2) {
						gc.GetComponent<GameController> ().ElementalLockOpen (2);
						an.SetBool ("fireUnlocked", true);
					}
					break;
				case 3:
					if (elementType == 3) {
						gc.GetComponent<GameController> ().ElementalLockOpen (3);
						an.SetBool ("waterUnlocked", true);
					}
					break;
				case 4:
					if (elementType == 4) {
						gc.GetComponent<GameController> ().ElementalLockOpen (4);
						an.SetBool ("airUnlocked", true);
					}
					break;
				}
			}
			enablePlayer ();
		}

	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			an.SetBool ("characterClose", true);
			yield break;
		}
		yield break;
	}

	private void disablePlayer() {
		gc.GetComponentInChildren<PauseMenu> ().enabled = false;
		player.GetComponent<PlayerCombat>().DisablePlayer();
		hud.SetActive (false);
		mainCamera.gameObject.SetActive(false);
		cutSceneCamera.gameObject.SetActive(true);
		Time.timeScale = 0f;
	}

	private void enablePlayer() {
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		hud.SetActive (true);
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void airLock() {
		Animator anbd = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
		anbd.SetBool ("airUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		hud.SetActive (true);
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void earthLock() {
		Animator anbd = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
		anbd.SetBool ("earthUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		hud.SetActive (true);
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void fireLock() {
		Animator anbd = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
		anbd.SetBool ("fireUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		hud.SetActive (true);
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void waterLock() {
		Animator anbd = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
		anbd.SetBool ("waterUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		hud.SetActive (true);
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}
}
