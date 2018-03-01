using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : MonoBehaviour {
	private SpriteRenderer pRenderer;
	public Sprite up;
	public Sprite down;

	private Animator an;

	void Start () {
		pRenderer = GetComponent<SpriteRenderer> ();

		an = GameObject.Find("ElementalLock_Air").GetComponent<Animator>();
	}

	IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!(other.gameObject.CompareTag ("Player"))) {
			yield break;
		}
		pRenderer.sprite = down;

		an.SetBool ("airUnlocked", true);

		yield break;
	}
}
