using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
	private SpriteRenderer pRenderer;
	public Sprite up;
	public Sprite down;

	private Animator an;

	void Start () {
		pRenderer = GetComponent<SpriteRenderer> ();

		an = GameObject.Find("ElementalLock_Water").GetComponent<Animator>();

	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			yield break;
		}
		pRenderer.sprite = down;

		an.SetBool ("waterUnlocked", true);

		yield break;
	}
}
