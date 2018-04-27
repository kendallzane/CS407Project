using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanWindToggleable : MonoBehaviour {

	public bool facingDown = true;
	private bool containsPlayer;
	public float thrust = 0.5f;
	private GameObject player;
	public Rigidbody2D rb;
	public PlayerCombat pc;
	public Sprite s1, s2, s3, s4;
	public float animRate = 0.1f;
	private float timer;
	public bool active = true;

	// Use this for initialization
	void Start () {
		if (!facingDown) {
			GetComponent<SpriteRenderer>().sprite = s4;
		}
		containsPlayer = false;
		player = GameObject.FindGameObjectWithTag("Player");
		rb = player.GetComponent<Rigidbody2D>();
		pc = player.GetComponent<PlayerCombat>();
	}

	// Update is called once per frame
	void Update () {
		if (!active) {
			return;
		}
		if (facingDown) {
			timer += Time.deltaTime;
			if (timer >= animRate) {
				if (GetComponent<SpriteRenderer>().sprite == s1) {
					GetComponent<SpriteRenderer>().sprite = s2;
				} else if (GetComponent<SpriteRenderer>().sprite == s2) {
					GetComponent<SpriteRenderer>().sprite = s3;
				} else if (GetComponent<SpriteRenderer>().sprite == s3) {
					GetComponent<SpriteRenderer>().sprite = s1;
				}
				timer = 0;
			}
		}
		if (containsPlayer && pc.attacking == false && pc.dashing == false && pc.canAttack == true) {
			//Only apply in direction of wind travel
			if (facingDown) {
				//vertical wind	
				rb.AddForce(Vector3.down * thrust);
			} else {
				//horizontal wind
				rb.AddForce(Vector3.up * thrust);
			}
		}
		containsPlayer = false;
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.tag == "Player") {
			containsPlayer = true;
		} 
		if (coll.tag == "Projectile" && active) {
			if (facingDown) {
				//vertical wind	
				coll.GetComponent<Rigidbody2D>().AddForce(Vector3.down * thrust/20);
			} else {
				//horizontal wind
				coll.GetComponent<Rigidbody2D>().AddForce(Vector3.up * thrust/20);
			}
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			containsPlayer = false;
		}
	}

}
