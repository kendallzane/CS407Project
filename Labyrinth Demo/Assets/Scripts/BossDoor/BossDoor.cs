using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

	private Animator an;

	private GameObject player;
	private GameObject gc;

	public GameObject mainCamera;
	public GameObject cutSceneCamera;

	// Use this for initialization
	void Start () {
		an = GameObject.Find("Boss Door Front").GetComponent<Animator> ();
		player = GameObject.Find ("MainCharacter");
		gc = GameObject.Find ("GameController");
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
		mainCamera.gameObject.SetActive(false);
		cutSceneCamera.gameObject.SetActive(true);
		Time.timeScale = 0f;
	}

	private void enablePlayer() {
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void airLock() {
		an.SetBool ("airUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void earthLock() {
		an.SetBool ("earthUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void fireLock() {
		an.SetBool ("fireUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}

	private void waterLock() {
		an.SetBool ("waterUnlocked", true);
		gc.GetComponentInChildren<PauseMenu> ().enabled = true;
		player.GetComponent<PlayerCombat>().EnablePlayer();
		cutSceneCamera.gameObject.SetActive(false);
		mainCamera.gameObject.SetActive(true);
		Time.timeScale = 1f;
	}
}
