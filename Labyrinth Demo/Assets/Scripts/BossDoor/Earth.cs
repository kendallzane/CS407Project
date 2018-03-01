using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {
	private SpriteRenderer pRenderer;
	public Sprite up;
	public Sprite down;

	private Animator an;

	void Start () {
		pRenderer = GetComponent<SpriteRenderer> ();

		an = GameObject.Find("ElementalLock_Earth").GetComponent<Animator>();

	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			yield break;
		}
		pRenderer.sprite = down;

		an.SetBool ("earthUnlocked", true);

		yield break;
	}
}
