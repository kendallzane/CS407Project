using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2OChad : EnemyAI {

	//constants
	
	public bool iAmBoss;
	public float bossTeleportRate = 1.0f;
	public int bossTeleportTimes = 3;
	private int bossTeleportCounter = 0;
	public GameObject Baby;
	
	public GameObject Bullet;
	private GameObject Player;
	private GameObject Puddles;
	
	public float baseX;
	public float baseY;
	
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;
	private Transform[] puddleList;

	//variables
	public float aimSpeed = 50f;							//speed drop travels in
	public bool aimAttack = false;
	public int aimAttackTimes = 40;
	private int aimAttackCounter = 0;
	public float aimAttackRate = 0.05f;
	
	public float spinSpeed = 50f;	
	public int spinAttack = 0;							//1 is counterclockwise, 2 is clockwise
	public float  spinAngle = 0;
	public int spinAngleIncrement = 3;
	public int spinAttackTimes = 360/3;
	private int spinAttackCounter = 0;
	public float spinAttackRate = 0.015f;
	
	public float waveSpeed = 30f;
	public bool waveAttack;							
	public int waveAngleIncrement = 3;
	public int waveAngleQuantity = 10;
	public int waveAttackTimes = 5;
	private int waveAttackCounter = 0;
	public float waveAttackRate = 2f;
	///
	public int damage = 20;								//how much damage does the drop do?
	public float changeDirMin = 2.0f;					//how many min seconds until changeDir
	public float changeDirMax = 4.0f;					//how many max seconds until changeDir
	public float pushBackForce = 50f;					//how quickly does the drop get pushed back?
	public float timeSinceAttack = 0.0f;
	public bool daddy = false;
	public float attackRate = 4.0f;
	
	private float timeToChange;							//randomized float btwn min and max to change directions
	private float timeSinceChanged = 0.0f;				//when since last changeDir
	private int dir = 0;
	private bool canMove;								//can the drop move?
	private bool alive = true;
	
	private float moveAngle = 90;
	public bool mercyInvincibility = false;
	
	// Use this for initialization
	void Start () {
		baseX = transform.position.x;
		baseY = transform.position.y;
		dir = 90;
		canMove = true;
		an.SetInteger ("Direction", 0);
		timeSinceChanged = 0f;
		timeToChange = Random.Range (changeDirMin, changeDirMax);
		timeSinceChanged = timeToChange;
		Player = GameObject.Find("MainCharacter");
		
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		if (iAmBoss) {
			s.color = new Color (1f, 0.5f, 1f);
			Puddles = GameObject.Find("Puddles");
			puddleList = (Transform[]) Puddles.GetComponentsInChildren<Transform>();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(baseX, baseY, 0);
		if (!alive) {
			return;
		}
		// Update time since the slug last made lava
		
		bool startAttack = false;
		
		if (aimAttack) {
			timeSinceAttack += Time.deltaTime;
			if (timeSinceAttack > aimAttackRate) {
				an.SetTrigger("Shoot");
				Vector3 dir = Player.transform.position - transform.position;
				dir = Player.transform.InverseTransformDirection(dir);
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				angle += Random.Range(-5, 6);
                StartCoroutine(FireProjectile(aimSpeed, angle));
        
				aimAttackCounter += 1;
				
				if (aimAttackCounter >= aimAttackTimes) {
					aimAttack = false;
					aimAttackCounter = 0;
				}
				
				timeSinceAttack = 0;
				
			}
			
			return;
		}
		
		if (spinAttack > 0) {
			timeSinceAttack += Time.deltaTime;
			if (timeSinceAttack > spinAttackRate) {
				an.SetTrigger("Shoot");
                StartCoroutine(FireProjectile(spinSpeed, spinAngle));
        
				spinAttackCounter += 1;
				if (spinAttack == 1) {
					spinAngle += spinAngleIncrement;
				} else {
					spinAngle -= spinAngleIncrement;
				}
				
				if (spinAttackCounter >= spinAttackTimes) {
					spinAttack = 0;
					spinAttackCounter = 0;
				}
				
				timeSinceAttack = 0;
				
			}
			
			return;
		}
		
		if (waveAttack) {
			timeSinceAttack += Time.deltaTime;
			if (timeSinceAttack > waveAttackRate) {
				Vector3 dir = Player.transform.position - transform.position;
				dir = Player.transform.InverseTransformDirection(dir);
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				for (int i = -waveAngleQuantity ; i < waveAngleQuantity; i++) {
					StartCoroutine(FireProjectile(waveSpeed, angle + (i * waveAngleIncrement)));
				}
        
				waveAttackCounter += 1;
				
				if (waveAttackCounter >= waveAttackTimes) {
					waveAttack = false;
					waveAttackCounter = 0;
				}
				
				timeSinceAttack = 0;
				
			}
			
			return;
		}
		
		timeSinceAttack += Time.deltaTime;
		
		if (iAmBoss && timeSinceAttack > bossTeleportRate) {
			Transform randomPuddle = puddleList[Random.Range(0,puddleList.Length)];
			transform.position = randomPuddle.transform.position + new Vector3(0f, 0.2f, 0f);
			baseX = transform.position.x;
			baseY = transform.position.y;
			timeSinceAttack = 0;
			bossTeleportCounter += 1;
			an.SetTrigger("DoneShoot");
			if (bossTeleportCounter >= bossTeleportTimes) {
				startAttack = true;
				bossTeleportCounter = 0;
			}
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
			
			
		if ((timeSinceAttack > attackRate) || startAttack)
        {
            
            int attackType; 
			if (iAmBoss) {
				attackType = Random.Range(0, 4);
			} else {
				attackType = Random.Range(0, 3);
			}
			if (attackType == 0) {
				aimAttack = true;
				timeSinceAttack = 0f;
				return;
			} 
			if (attackType == 1) {
				spinAttack = Random.Range(1, 3);
				timeSinceAttack = 0f;
				Vector3 dir = Player.transform.position - transform.position;
				dir = Player.transform.InverseTransformDirection(dir);
				spinAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				return;
			}
			if (attackType == 2) {
				waveAttack = true;
				timeSinceAttack = 0f;
				return;
			}
			if (attackType == 3) {
				GameObject BabyH2O;
				BabyH2O = Instantiate(
				Baby,
				transform.position + new Vector3(0f,-0.2f,0f),
				transform.rotation) as GameObject;
				an.SetTrigger("Shoot");
				timeSinceAttack = -2.0f;
				EnemyHealth bh = BabyH2O.GetComponent<EnemyHealth>();
				bh.health = 20;
				return;
				
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
		if (mercyInvincibility) {
			//Debug.Log("TakeDamage activated - Enemy is invincible");
			return;
		}
		if (alive) {
			//Debug.Log("TakeDamage activated - Enemy is not invincible");
			if (aimAttack) {
				aimAttack = false;
				timeSinceAttack = 0f;
				aimAttackCounter = 0;
			}
			if (waveAttack) {
				waveAttack = false;
				timeSinceAttack = 0f;
				waveAttackCounter = 0;
			}
			if (spinAttack > 0) {
				spinAttack = 0;
				timeSinceAttack = 0f;
				spinAttackCounter = 0;
			}
			mercyInvincibility = true;
			canMove = false;
			eh.TakeDamage (damage);
			an.SetTrigger ("Hurt");
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
		Debug.Log("h2o chad est mort");
		yield return new WaitForSecondsRealtime(1);

		//Update Game Controller and clear room
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			gcObj.GetComponent<GameController> ().DefeatBoss (3);
			gcObj.GetComponent<BackgroundMusic> ().SwitchLayers (gcObj.GetComponent<BackgroundMusic> ().currLayer);
		}
		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		foreach (GameObject door in doors) {
			door.GetComponent<Animator> ().SetTrigger ("Open");
		}
		
		Destroy(gameObject);
	}
	
	public void HurtFinished () {
		mercyInvincibility = false;
		if (alive) {
			canMove = true;
		}
		transform.position = new Vector3(baseX, baseY, 0);
	}
	
	    // Fire eight projectile at the player
    IEnumerator FireProjectile(float speed, float angle)
    {
        // Wait for one second before firing projectiles (animation)
        //yield return new WaitForSecondsRealtime(1);

        // The Projectile instantiation happens here
		
		an.SetTrigger("Shoot");
		
        GameObject Projectile;
        Projectile = Instantiate(
            Bullet,
            transform.position + new Vector3(0f, -0.2f, 0f),
            transform.rotation) as GameObject;
		
		transform.position = new Vector3(baseX + Random.Range(-0.01f, 0.01f), baseY + Random.Range(-0.01f, 0.01f), 0);
		
		Projectile.transform.localScale += new Vector3(Random.value/2, Random.value/2, Random.value/2);
		Projectile.transform.Rotate(1 - 2 * Random.Range (0f, 1f), 1 - 2 * Random.Range (0f, 1f), Random.Range (0, 360));

        // Set the owner of the new Projectile to this gameObject

        // Retrieve the Rigidbody component from the instantiated Projectile and control it
        Rigidbody2D ProjectileRigidBody;
        ProjectileRigidBody = Projectile.GetComponent<Rigidbody2D>();

        // Tell the Projectile to be "pushed" toward the player by an amount set by ProjectileSpeed
        // Each case is the direction of one projectile
		
		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		ProjectileRigidBody.AddForce(dir * speed);
		

        // Wait for one second before firing projectiles (animation)
        //yield return new WaitForSecondsRealtime(1);

        // Allow the flier to move again
       // canMove = true;

        // Basic clean up, set the Projectile to self destruct after 2 seconds.
        Destroy(Projectile, 4.0f);
		an.SetTrigger("DoneShoot");
		
		yield return new WaitForSecondsRealtime(0.3f);
	}
	
}
