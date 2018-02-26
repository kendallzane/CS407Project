using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2OBoy : EnemyAI {

	//constants
	public GameObject Trail;
	public GameObject Baby;
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//variables
	public float speed = 0.7f;							//speed drop travels in
	public int damage = 20;								//how much damage does the drop do?
	public float changeDirMin = 2.0f;					//how many min seconds until changeDir
	public float changeDirMax = 4.0f;					//how many max seconds until changeDir
	public float pushBackForce = 50f;					//how quickly does the drop get pushed back?
	public float trailCreationRate = 1.0f;
	public float timeSinceTrail = 0.0f;
	public float trailLifetime = 2.0f;
	public float trailScale = 1.0f;
	public bool daddy = false;
	
	
	private float timeToChange;							//randomized float btwn min and max to change directions
	private float timeSinceChanged = 0.0f;				//when since last changeDir
	private int dir = 0;
	private bool canMove;								//can the drop move?
	private bool alive = true;
	
	private float moveAngle = 90;

	// Use this for initialization
	void Start () {
		dir = 90;
		canMove = true;
		an.SetInteger ("Direction", 0);
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		timeSinceChanged = timeToChange;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!alive) {
			return;
		}
		// Update time since the slug last made lava
        timeSinceTrail += Time.deltaTime;

        // After a specified length of time, fire eight projewactiles
        if (timeSinceTrail > trailCreationRate)
        {
            GameObject LavaSplat;
			LavaSplat = Instantiate(
				Trail,
				transform.position,
				transform.rotation) as GameObject;
			SpriteRenderer spRend = LavaSplat.transform.GetComponent<SpriteRenderer>();
			LavaSplat.transform.localScale = new Vector3(0.3f * trailScale, 0.3f * trailScale, 0.3f * trailScale);
			LavaSplat.transform.localScale += new Vector3(Random.value/2, Random.value/2, Random.value/2);
			LavaSplat.transform.Rotate(1 - 2 * Random.Range (0, 1), 1 - 2 * Random.Range (0, 1), 90 + moveAngle);
			spRend.color = new Color(1f,1f,1f,.8f);
				
				
            timeSinceTrail  = 0f;
			Destroy(LavaSplat, trailLifetime);
			
        }
		
		if (canMove) {
			timeSinceChanged += Time.deltaTime;
			if (timeSinceChanged > timeToChange) {
				ChangeDir ();
				Vector3 angle = Quaternion.AngleAxis(dir, Vector3.forward) * Vector3.right;
				rb.AddForce(angle*speed, ForceMode2D.Impulse);
			}

			
			/*
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
			*/
		}
	}

	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && coll.isTrigger && canMove) {
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
		if (alive) {
			canMove = false;
			eh.TakeDamage (damage);
			an.SetTrigger ("Hurt");
			rb.velocity = Vector2.zero;
			rb.AddForce (dirHit.normalized * pushBackForce);
			moveAngle = Vector2.SignedAngle(Vector2.right, dirHit);
			if (moveAngle < 0) {
				moveAngle += 360;
			}
		}
	}

	public override IEnumerator OnDeath ()
	{
		an.SetTrigger ("Dead");
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		rb.isKinematic = true;
		canMove = false;
		alive = false;
		Debug.Log("h2o boy est mort");
		yield return new WaitForSecondsRealtime(1);
		if (daddy) {
			
			
			
			GameObject BabySlug1;
			BabySlug1 = Instantiate(
				Baby,
				transform.position + new Vector3(0.1f,0f,0f),
				transform.rotation) as GameObject;
			
			EnemyHealth bh1 = BabySlug1.GetComponent<EnemyHealth>();
			bh1.health = 30;
			BabySlug1.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			H2OBoy ls1 = BabySlug1.GetComponent<H2OBoy>();
			ls1.trailScale = 0.3f;
			ls1.speed = 0.3f;
			
			GameObject BabySlug2;
			BabySlug2 = Instantiate(
				Baby,
				transform.position + new Vector3(-0.1f,0f,0f),
				transform.rotation) as GameObject;
			EnemyHealth bh2 = BabySlug2.GetComponent<EnemyHealth>();
			bh2.health = 30;
			BabySlug2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			H2OBoy ls2 = BabySlug2.GetComponent<H2OBoy>();
			ls2.trailScale = 0.3f;
			ls2.speed = 0.3f;
		}
		
		Destroy(gameObject);
	}
	
	public void HurtFinished () {
		if (alive) {
			canMove = true;
		}
	}

	//start moving in a new direction
	void ChangeDir () {
		dir = Random.Range (0, 360);
		moveAngle = dir;
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		if (dir < 45 || dir > 360 - 45) {
			an.SetInteger ("Direction", 3);
		} else if (dir > 45 && dir < 90 + 45) {
			an.SetInteger ("Direction", 2);
		} else if (dir > 90 + 45 && dir < 180 + 45) {
			an.SetInteger ("Direction", 1);
		} else {
			an.SetInteger ("Direction", 0);
		}
		
	}
}
