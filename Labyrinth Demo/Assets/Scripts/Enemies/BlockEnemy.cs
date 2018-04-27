using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockEnemy : EnemyAI {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;
	
	private const int PLAYERLAYER = 8;
	
	public GameObject Player;
	
	public float attackSpeed = 0.02f;					//speed block travels in when attacking
	public float attackRange = 2.0f;
	public float retreatSpeed = 0.01f;					//speed block travels in when attacking
	public float attackDelay = 0.5f;					//how long before the block attacks
	public float retreatDelay = 0.5f;					//how long before the block attacks
	public int damage = 20;								//how much damage does the block do?

	public bool checkUp = false;
	public bool checkDown = false;
	public bool checkLeft = false;
	public bool checkRight = false;
	
	public bool attacking = false;
	public bool retreating = false;
	
	public Vector2 homePosition;						//where the block stays and returns to
	
	public float timer;
	public Vector2 desiredVelocity = Vector2.zero;
	
	public Tilemap tm;
	public Grid gr;
	
	public Sprite normalSprite, attackSprite;
	
	
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("MainCharacter");
		tm = GameObject.Find("Grid/Floor").GetComponent<Tilemap>();
		gr = GameObject.Find("Grid").GetComponent<Grid>();
		homePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!attacking && !retreating) {
			int directionToMove = checkForPlayer();
			if (directionToMove != -1) {
				attacking = true;
				GetComponent<SpriteRenderer>().sprite = attackSprite;
				timer = attackDelay;
				switch (directionToMove) {
				case DOWN:
					desiredVelocity = Vector2.down * attackSpeed;
					break;
				case LEFT:
					desiredVelocity = Vector2.left * attackSpeed;
					break;
				case UP:
					desiredVelocity = Vector2.up * attackSpeed;
					break;
				case RIGHT:
					desiredVelocity = Vector2.right * attackSpeed;
					break;
				}
			}
		}
		
		if (attacking || retreating) {
			if (timer >= 0) {
				timer -= Time.deltaTime;
			} else {
				timer = 0;
				//transform.position = (Vector2)transform.position + (Vector2)desiredVelocity;
				transform.Translate(desiredVelocity * Time.deltaTime);
				if (retreating) {
					GetComponent<SpriteRenderer>().sprite = normalSprite;
				}
			}
		}
		
		if (retreating && Vector2.Distance(transform.position, homePosition) <= 0.02f) {
			desiredVelocity = Vector2.zero;
			transform.position = homePosition;
			retreating = false;
		}
		
	}

	int checkForPlayer () {
		RaycastHit2D hit;
		if (checkDown) {
			//Debug.Log(Time.time + "checking down");
			Physics2D.queriesHitTriggers = false;
			hit = Physics2D.Raycast(homePosition, Vector2.down, attackRange);
			Physics2D.queriesHitTriggers = true;
			if (hit) {
				
				//Debug.Log(Time.time + "		gameObject hit: " + hit.transform.name);
				if (hit.collider.gameObject == Player) {
					Debug.Log("down");
					return DOWN;
				}
			}
		}
		if (checkUp) {
			Physics2D.queriesHitTriggers = false;
			hit = Physics2D.Raycast(homePosition, Vector2.up, attackRange);
			Physics2D.queriesHitTriggers = true;
			if (hit) {
				//Debug.Log(Time.time + "		gameObject hit: " + hit.transform.name);
				if (hit.collider.gameObject == Player) {
					return UP;
				}
			}
		}
		if (checkLeft) {
			Physics2D.queriesHitTriggers = false;
			hit = Physics2D.Raycast(homePosition, Vector2.left, attackRange);
			Physics2D.queriesHitTriggers = true;
			if (hit) {
				Debug.Log(Time.time + "		gameObject hit: " + hit.transform.name);
				if (hit.collider.gameObject == Player) {
					
					return LEFT;
				}
			}
		}
		if (checkRight) {
			Physics2D.queriesHitTriggers = false;
			hit = Physics2D.Raycast(homePosition, Vector2.right, attackRange);
			Physics2D.queriesHitTriggers = true;
			if (hit) {
				//Debug.Log(Time.time + "		gameObject hit: " + hit.transform.name);
				if (hit.collider.gameObject == Player) {
					return RIGHT;
				}
			}
		}
		return -1;
	}
	
	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		Debug.Log(coll.name);
		if (coll.tag == "Player" && coll.isTrigger) {
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
		if ((coll.gameObject == tm.gameObject || coll.tag == "Borders") && attacking) {
			attacking = false;
			retreating = true;
			timer = retreatDelay;
			desiredVelocity = (-desiredVelocity / attackSpeed) * retreatSpeed;
			Debug.Log("c");
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
