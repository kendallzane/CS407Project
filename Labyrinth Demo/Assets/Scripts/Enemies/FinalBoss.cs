using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyAI {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references
	private GameObject player;
	public GameObject smokeAttack;
	public GameObject victoryScreen;

	//variables
	public float speed = 2.0f;							//speed boss travels in
	public int damage = 20;								//how much damage does the final boss do?
	public float changeDirMin = 2.0f;					//how many min seconds until changeDir
	public float changeDirMax = 4.0f;					//how many max seconds until changeDir

	public int movesTillChange = 8;						//how many movement iterations until an attack?
	public float attackTime = 3.0f;						//how long does the boss attack for?
	public float turnSpeed = 5f;

	private float timeToChange;							//randomized float btwn min and max to change directions
	private float timeSinceChanged = 0.0f;				//when since last changeDir
	private int dir = 0;
	private bool canMove;								//can the wraith move?

	private float attackTimeDelay;
	private int stateCounter = 0;						//how many movement iterations until an attack?
	private bool madeAttack;
	private GameObject mySmoke;

	// Use this for initialization
	void Start () {
		dir = Random.Range (0, 4);
		canMove = true;
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		player = GameObject.FindGameObjectWithTag ("Player");
		attackTimeDelay = attackTime;
	}

	// Update is called once per frame
	void Update () {
		if (stateCounter > movesTillChange) {
			Attack ();
		} else if (canMove) {
			timeSinceChanged += Time.deltaTime;
			if (timeSinceChanged > timeToChange) {
				ChangeDir ();
			}

			switch (dir) {
			case DOWN:
				rb.velocity = Vector2.down * speed;
				break;
			case LEFT:
				rb.velocity = Vector2.left * speed;
				break;
			case UP:
				rb.velocity = Vector2.up * speed;
				break;
			case RIGHT:
				rb.velocity = Vector2.right * speed;
				break;
			}
		}
	}

	void Attack () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		rb.velocity = Vector2.zero;
		canMove = false;
		attackTimeDelay -= Time.deltaTime;
		if (player != null) {
			transform.rotation = Quaternion.Euler (0f, 0f, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
			if (!madeAttack) {
				mySmoke = Instantiate (smokeAttack, transform.position, transform.rotation);
				madeAttack = true;
			} else {
				mySmoke.transform.rotation = transform.rotation;
				mySmoke.transform.position = transform.position;
			}
		}
		if (attackTimeDelay < 0) {
			Destroy (mySmoke);
			attackTimeDelay = attackTime;
			transform.rotation = Quaternion.identity;
			madeAttack = false;
			stateCounter = 0;
			canMove = true;
		}
	}

	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && coll.isTrigger) {
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
	}

	/// <summary>
	/// Takes damage similar to the main character.
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="dirHit">Dir hit.</param>
	public override void TakeDamage (int damage, Vector2 dirHit)
	{
		canMove = false;
		eh.TakeDamage (damage);
		an.SetTrigger ("Hurt");
		rb.velocity = Vector2.zero;
	}

	public void HurtFinished () {
		canMove = true;
	}

	public override IEnumerator OnDeath ()
	{
		yield return null;
		victoryScreen.SetActive (true);
		Destroy(gameObject);
	}

	//start moving in a new direction
	void ChangeDir () {
		dir = Random.Range (0, 4);
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		stateCounter++;
	}
}