using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This should allow the sword to throw out and come back to the player like a boomerang
public class ThrownSword : MonoBehaviour {

	//constants
	private const int DOWN = 0;						//direction
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references
	private Rigidbody2D rb;
	[HideInInspector] public GameObject player;			//to get the position of the player

	//variables
	public float outForce = 90.0f;						//how fast does the sword throw out?
	public float inSpeed = 4.0f;						//how quickly to draw the sword back in each frame
	public float returnTolerance = 0.3f;				//how close to the player until he catches it
	public float timeTillReturn = 1.0f;					//how long until the sword returns to the player?
	private float timeDelay = 0f;						//how long since the sword was thrown?
	private Vector2 towardsPlayer;						//the calculated vector2 to return to the player
	private bool canCatch = false;						//can the player catch the thrownSword?

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		float horiz = Input.GetAxis ("Horizontal");
		float vert = Input.GetAxis ("Vertical");
		Vector2 dirVector = new Vector2 (horiz, vert).normalized;
		if (dirVector.magnitude > 0.1f) {
			rb.AddForce (dirVector * outForce);
		} else {
			switch (player.GetComponent<Animator> ().GetInteger ("Direction")) {
			case DOWN:
				rb.AddForce (Vector2.down * outForce);
				break;
			case LEFT:
				rb.AddForce (Vector2.left * outForce);
				break;
			case UP:
				rb.AddForce (Vector2.up * outForce);
				break;
			case RIGHT:
				rb.AddForce (Vector2.right * outForce);
				break;
			}
		}
		canCatch = false;
	}

	//Update is called once per frame
	void Update () {
		if (timeDelay < timeTillReturn) {
			timeDelay += Time.deltaTime;
		} else {
			canCatch = true;
		}
	}

	// FixedUpdate is called once per physics iteration
	void FixedUpdate () {
		if (canCatch) {
			towardsPlayer = player.transform.position - transform.position;
			rb.velocity = towardsPlayer.normalized * inSpeed;

			if (Mathf.Abs (transform.position.x - player.transform.position.x) < returnTolerance && Mathf.Abs (transform.position.y - player.transform.position.y) < returnTolerance) {
				//Player caught the sword
				player.GetComponent<PlayerCombat>().RetrieveSword();
				Destroy (gameObject);
			}
		}
	}
}
