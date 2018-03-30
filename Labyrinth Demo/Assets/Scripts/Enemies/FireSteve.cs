using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSteve : EnemyAI {

	//constants
	
	public GameObject Bullet;
	private GameObject Player;
	public GameObject Lava;
	public GameObject Cage;
	
	//variables
	public float aimSpeed = 20f;
	public int damage = 20;				//how quickly does the drop get pushed back?
	public float timeSinceAttack = 0.0f;
	public float attackRate = 0.8f;
	
	public float lavaCreationRate = 0.1f;
	public float timeSinceLava = 0.0f;
	public float lavaLifetime = 2.0f;
	public float lavaScale = 1.0f;
	
	public float timer = 0.0f;
	public float regenerate = 1.0f;
	public bool regernerating = false;
	
	private bool canMove;								//can the drop move?
	private bool alive = true;
	
    public float rotateSpeed = 1f;
    public float radius = 1.5f;
	private Vector2 center;
    private float angle;
	
	public bool mercyInvincibility = false;
	
	public int health = 3;
	
	// Use this for initialization
	void Start () {
		canMove = true;//
		center = new Vector2(0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3(baseX, baseY, 0);
		if (!alive) {
			return;
		}
		// Update time since the slug last made lava
		
		
		
		timeSinceAttack += Time.deltaTime;
		
		if (regernerating) {
			timer += Time.deltaTime;
			if (timer >= regenerate) {
				timer = 0.0f;
				regernerating = false;
			}
		}
		
		if (timeSinceAttack >= attackRate) {
			float angle = Random.Range(0, 360);
			FireProjectile(aimSpeed, angle);
			timeSinceAttack = 0f;
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
		
		angle += rotateSpeed * Time.deltaTime;
		var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * (radius + (Mathf.Cos(6 * angle)/2));
        transform.position = center + offset;
	}

	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		Debug.Log("touching something");
		if (coll.tag == "Player" && coll.isTrigger && canMove) {
			Debug.Log("touching player");
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
		
		if (coll.tag == "Enemy") {
			Debug.Log("touching enemy");
			bool hurtMe = coll.GetComponent<H2OBoy>().mercyInvincibility;
			if (hurtMe && !regernerating) {
				Destroy(coll.gameObject);
				regernerating = true;
				an.SetTrigger ("Hurt");
				health--;
				rotateSpeed *= -1.5f;
				mercyInvincibility = true;
				if (health <= 0) {
					Destroy(Cage);
					die();
				}
			} else {
				Destroy(coll.gameObject);
			}
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
	public void die() {
		an.SetTrigger ("Dead");
		canMove = false;
		alive = false;
		Debug.Log("fire steve est mort");

		//Update Game Controller and clear room
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			gcObj.GetComponent<GameController> ().DefeatBoss (2);
			gcObj.GetComponent<BackgroundMusic> ().SwitchLayers (gcObj.GetComponent<BackgroundMusic> ().currLayer);
		}
		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		foreach (GameObject door in doors) {
			door.GetComponent<Animator> ().SetTrigger ("Open");
		}
		
		OnDeath();
	}
	public override IEnumerator OnDeath ()
	{
		Destroy(gameObject);
		yield return new WaitForSecondsRealtime(1);
	}
	
	public void HurtFinished () {
		mercyInvincibility = false;
		if (alive) {
			canMove = true;
		}
	}
	
	    // Fire eight projectile at the player
    public void FireProjectile(float speed, float angle)
    {
        // Wait for one second before firing projectiles (animation)
        //yield return new WaitForSecondsRealtime(1);

        // The Projectile instantiation happens here
		
        GameObject Projectile;
        Projectile = Instantiate(
            Bullet,
            transform.position + new Vector3(0f, -0.2f, 0f),
            transform.rotation) as GameObject;
		
		
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
	}
	
}
