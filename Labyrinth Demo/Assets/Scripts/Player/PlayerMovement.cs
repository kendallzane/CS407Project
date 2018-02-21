using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references
	private Rigidbody2D rb;
	private Animator an;
	private PlayerCombat pc;

	//variables
	private Vector2 movement;
	private float horiz;
	private float vert;

	public float speed = 2.0f;
	[HideInInspector] public bool canMove = true;

	// Use this for initialization
	void Start () {
		canMove = true;
		pc = GetComponent<PlayerCombat> ();
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (canMove) {
			horiz = Input.GetAxisRaw ("Horizontal");
			vert = Input.GetAxisRaw ("Vertical");
			movement = new Vector2 (horiz, vert).normalized * speed;
			rb.velocity = movement;
			an.SetFloat ("Speed", rb.velocity.magnitude);

			//set facing direction
			if (horiz != 0 || vert != 0) {
				if (Mathf.Abs (horiz) > Mathf.Abs (vert)) {
					if (horiz > 0) {
						an.SetInteger ("Direction", RIGHT);
					} else {
						an.SetInteger ("Direction", LEFT);
					}
				} else {
					if (vert > 0) {
						an.SetInteger ("Direction", UP);
					} else {
						an.SetInteger ("Direction", DOWN);
					}
				}
			}
		} else {
			//cancel end attack frames by moving
			if (pc.canAttack && !pc.attackLock) {
				if (Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.1f || Mathf.Abs (Input.GetAxis ("Vertical")) > 0.1f) {
					pc.EndAttack ();
					an.SetTrigger ("Cancel");
				}
			}
		}
	}
}
