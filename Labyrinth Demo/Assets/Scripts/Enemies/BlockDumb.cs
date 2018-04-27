using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockDumb : EnemyAI {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;
	
	public float moveSpeed = 0.02f;					//speed block travels in when attacking
	public int damage = 20;							//how much damage does the block do?
	public int initialDirection;					//initial movement direction
	
	public Tilemap tm;
	public Grid gr;
	private Vector2 desiredVelocity = Vector2.zero;
	private float flipRate = 1.0f;
	private float timer;
	
	// Use this for initialization
	void Start () {
		tm = GameObject.Find("Grid/Floor").GetComponent<Tilemap>();
		gr = GameObject.Find("Grid").GetComponent<Grid>();
		
		switch (initialDirection) {
			case DOWN:
				desiredVelocity = Vector2.down * moveSpeed;
				break;
			case LEFT:
				desiredVelocity = Vector2.left * moveSpeed;
				break;
			case UP:
				desiredVelocity = Vector2.up * moveSpeed;
				break;
			case RIGHT:
				desiredVelocity = Vector2.right * moveSpeed;
				break;
		}
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(desiredVelocity * Time.deltaTime);
		timer += Time.deltaTime;
			if (timer >= flipRate) {
				GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
				timer = 0f;
			}
	}

	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		
		if (coll.tag == "Player" && coll.isTrigger) {
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
		if (coll.gameObject == tm.gameObject || coll.tag == "Borders") {
			desiredVelocity = (-desiredVelocity);
		}
	}

	/// <summary>
	/// Takes damage similar to the main character.
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="dirHit">Dir hit.</param>
	public override void TakeDamage (int damage, Vector2 dirHit)
	{
		//
	}

	public void HurtFinished () {
		//
	}

	public override IEnumerator OnDeath ()
	{
		yield return null;
	}
	
	
}
