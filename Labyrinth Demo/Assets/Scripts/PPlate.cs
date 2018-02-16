using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlate : MonoBehaviour {

	SpriteRenderer pRenderer;
	public Sprite Up;
	public Sprite Down;

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

	public bool getIsOn () {
		return isOn;
	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			pRenderer.sprite = Down;
			yield break;
		}
		isOn = true;
		pRenderer.sprite = Down;
		if (wall != null) {
			wall.SetActive (false);
		}
		yield return new WaitForSeconds(0);
	}

	IEnumerator OnTriggerExit2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			pRenderer.sprite = Up;
			yield break;
		}
		if (dTime >= 0) {
			yield return new WaitForSecondsRealtime (dTime);
			pRenderer.sprite = Up;
			if (wall != null) {
				wall.SetActive (true);
			}
			isOn = false;
		}
	}

	public void off() {
		StartCoroutine (pOff());
	}

	IEnumerator pOff() {
		yield return new WaitForSecondsRealtime (1);
		isOn = false;
		pRenderer.sprite = Up;	
	}

	// Update is called once per frame
	void Update () {

	}
}
