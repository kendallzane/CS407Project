﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlate : MonoBehaviour {

	private SpriteRenderer pRenderer;
	public Sprite up;
	public Sprite down;

	public float dTime;

	public GameObject wall;

	private bool isOn;

	// Use this for initialization
	void Start () {
		isOn = false;
		// Up = Resources.Load<Sprite> ("PPlate Up");
		// Down = Resources.Load<Sprite> ("PPlate Down");
		pRenderer = GetComponent<SpriteRenderer> ();
	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			if (other.gameObject.CompareTag ("Projectile")) {
				yield break;
			}
			if (pRenderer.enabled) {
				pRenderer.sprite = down;
			}
			yield break;
		}
		isOn = true;
		if (!pRenderer.enabled) {
			pRenderer.enabled = true;
		}
		pRenderer.sprite = down;
		if (wall != null) {
			wall.SetActive (false);
		}
		yield return new WaitForSeconds(0);
	}

	IEnumerator OnTriggerExit2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			if (!isOn) {
				pRenderer.sprite = up;
			}
			yield break;
		}
		if (dTime >= 0) {
			yield return new WaitForSecondsRealtime (dTime);
			pRenderer.sprite = up;
			if (wall != null) {
				wall.SetActive (true);
			}
			isOn = false;
		}
	}

	public bool getIsOn () {
		return isOn;
	}

	public void off () {
		StartCoroutine (pOff());
	}

	IEnumerator pOff () {
		yield return new WaitForSecondsRealtime (.3f);
		isOn = false;
		pRenderer.sprite = up;	
	}

	public void invisible () {
		pRenderer.enabled = false;
	}

	public void visible () {
		pRenderer.enabled = true;
	}

	// Update is called once per frame
	void Update () {

	}
}
