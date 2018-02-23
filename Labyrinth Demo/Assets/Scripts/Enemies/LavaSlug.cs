using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSlug : EnemyAI {

	//constants
	public GameObject Lava;
	public GameObject Baby;
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//variables
	public float speed = 2.0f;							//speed slug travels in
	public int damage = 20;								//how much damage does the slug do?
	public float changeDirMin = 2.0f;					//how many min seconds until changeDir
	public float changeDirMax = 4.0f;					//how many max seconds until changeDir
	public float pushBackForce = 50f;					//how quickly does the slug get pushed back?
	public float lavaCreationRate = 1.0f;
	public float timeSinceLava = 0.0f;
	public float lavaLifetime = 2.0f;
	public float lavaScale = 1.0f;
	public bool daddy = false;
	
	
	private float timeToChange;							//randomized float btwn min and max to change directions
	private float timeSinceChanged = 0.0f;				//when since last changeDir
	private int dir = 0;
	private bool canMove;								//can the slug move?
	private bool alive = true;
	

	// Use this for initialization
	void Start () {
		dir = Random.Range (0, 4);
		canMove = true;
		an.SetInteger ("Direction", dir);
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!alive) {
			return;
		}
		// Update time since the slug last made lava
        timeSinceLava += Time.deltaTime;

        // After a specified length of time, fire eight projectiles
        if (timeSinceLava > lavaCreationRate)
        {
            GameObject LavaSplat;
			LavaSplat = Instantiate(
				Lava,
				transform.position,
				transform.rotation) as GameObject;
			SpriteRenderer spRend = LavaSplat.transform.GetComponent<SpriteRenderer>();
			LavaSplat.transform.localScale = new Vector3(0.3f * lavaScale, 0.3f * lavaScale, 0.3f * lavaScale);
			LavaSplat.transform.localScale += new Vector3(Random.value/2, Random.value/2, Random.value/2);
			LavaSplat.transform.Rotate(1 - 2 * Random.Range (0, 1), 1 - 2 * Random.Range (0, 1), Random.Range (0, 360));
			spRend.color = new Color(1f,1f,1f,.8f);
				
				
            timeSinceLava  = 0f;
			Destroy(LavaSplat, lavaLifetime);
			
        }
		
		if (canMove) {
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
		Debug.Log("slug est mort");
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
			LavaSlug ls1 = BabySlug1.GetComponent<LavaSlug>();
			ls1.lavaScale = 0.3f;
			ls1.speed = 0.3f;
			
			GameObject BabySlug2;
			BabySlug2 = Instantiate(
				Baby,
				transform.position + new Vector3(-0.1f,0f,0f),
				transform.rotation) as GameObject;
			EnemyHealth bh2 = BabySlug2.GetComponent<EnemyHealth>();
			bh2.health = 30;
			BabySlug2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			LavaSlug ls2 = BabySlug2.GetComponent<LavaSlug>();
			ls2.lavaScale = 0.3f;
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
		dir = Random.Range (0, 4);
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		an.SetInteger ("Direction", dir);
	}
}
