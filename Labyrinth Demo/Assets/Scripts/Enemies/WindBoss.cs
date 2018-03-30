using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBoss : EnemyAI {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//variables
	public float leftBound = -2.5f;
	public float rightBound = 2.5f;
	private int direction = 0;							// 0 = left, 1 = right
	public float speed = 1.0f;							//speed fan travels in
	
	public GameObject exitPlatform;						//Added to exit the Labyrinth's WindBoss room
	
	
	private float timeToChange;							//randomized float btwn min and max to change directions
	private bool canMove;								//can the slug move?
	private bool alive = true;
	public bool mercyInvincibility = false;
	
	public float windForce;
	public bool facingDown = true;
	private bool containsPlayer;

	private GameObject player;
	public GameObject bullet;
	
	private int phaseNum = 0;
	private int progress = 0;
	
	public float iconY = -2f;
	public float iconGap = 1f;
	public float iconCenter = 0.0f;
	private int numIcons;
	public GameObject icon;
	private GameObject icon1, icon2, icon3, icon4, icon5;
	public int icon1type, icon2type, icon3type, icon4type, icon5type;
	private float timeSinceAttack = 0.0f;
	private float shootRate = 0.1f;
	public float rectSize = 5.0f;
	public float bulletSpeed = 60.0f;
	
	public Sprite sprite1, sprite2, sprite3, sprite4;
	
	public int numPhases = 2;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		phase(phaseNum);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!alive) {
			return;
		}
		
		timeSinceAttack += Time.deltaTime;
		if (timeSinceAttack >= shootRate) {
			timeSinceAttack = 0;
			
			int side = Random.Range(0, 4); //left, right, up, down
			float posX, posY;
			int angle = 0;
			switch (side)
			{
				case 0:
					posX = -rectSize;
					posY = Random.Range(-rectSize, rectSize);
					angle = 0;
					break;
				case 1:
					posX = rectSize;
					posY = Random.Range(-rectSize, rectSize);
					angle = 180;
					break;
				case 2:
					posX = Random.Range(-rectSize, rectSize);
					posY = rectSize;
					angle = 270;
					break;
				case 3:
					posX = Random.Range(-rectSize, rectSize);
					posY = -rectSize;
					angle = 90;
					break;
				default:
					posX = -rectSize;
					posY = Random.Range(-rectSize, rectSize);
					break;
			}
			
			angle += Random.Range(-70, 71);
			
			GameObject Projectile;
			Projectile = Instantiate(
            bullet,
            new Vector3(posX, posY, 0f),
            transform.rotation) as GameObject;
			
			Projectile.GetComponent<FanBossProjectile>().FanBoss = gameObject;
			
			int type = Random.Range(1, 8); //left, right, up, down
			
			Projectile.GetComponent<FanBossProjectile>().type = type;
			
			
			// Retrieve the Rigidbody component from the instantiated Projectile and control it
			Rigidbody2D ProjectileRigidBody;
			ProjectileRigidBody = Projectile.GetComponent<Rigidbody2D>();

			// Tell the Projectile to be "pushed" toward the player by an amount set by ProjectileSpeed
			// Each case is the direction of one projectile
		
			Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
			ProjectileRigidBody.AddForce(dir * bulletSpeed);
			
			Destroy(Projectile, 10.0f);
			
		}
		
		if (containsPlayer) {
			Vector3 deltaPos = (player.transform.position - this.transform.position);

			//Inversion
			deltaPos = new Vector3 (1f / deltaPos.x, 1f / deltaPos.y, deltaPos.z);

			deltaPos.Scale (new Vector3 (windForce, windForce, 1));

			//Only apply in direction of wind travel
			if (facingDown) {
				//vertical wind	
				deltaPos.x = 0;
			} else {
				//horizontal wind
				deltaPos.y = 0;
			}
			deltaPos.z = 0;
			player.transform.position = player.transform.position + deltaPos;
		}
		containsPlayer = false;
		
		if (transform.position.x <= leftBound) {
			direction = 1;
		} else if (transform.position.x >= rightBound) {
			direction = 0;
		}
		
		if (direction == 1) {
			rb.velocity = Vector2.right * speed;
		} else {
			rb.velocity = Vector2.left * speed;
		}
	}

	//hurt the player character by running into them
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && coll.isTrigger && canMove) {
			//coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
	}

	public void phase(int phase) {
		
		
		Destroy(icon1);
		Destroy(icon2);
		Destroy(icon3);
		Destroy(icon4);
		Destroy(icon5);
		progress = 0;
		icon1 = null;
		icon2 = null; 
		icon3 = null; 
		icon4 = null; 
		icon5 = null; 
		if (phase >= numPhases) {
			GameObject[] bullets;
			bullets =  GameObject.FindGameObjectsWithTag("Projectile");
			for(int i = 0; i < bullets.Length; i++) {
				Destroy(bullets[i]);
			}
			Destroy(gameObject);
			return;
		}
		
		
		if (phase == 0) {
			numIcons = 3;
		} else if (phase == 1) {
			numIcons = 4;
		} else {
			numIcons = 5;
		}
		
		for (int i = 0; i < numIcons; i++) {
			setIcon(i);
		}
		
	}
	
	public void setIcon(int num) {
		float posX = iconCenter - ((iconGap * numIcons) / 2.0f) + (iconGap * (0.5f + num));
		GameObject iconToSet;
		iconToSet = Instantiate(
            icon,
            new Vector3(posX, iconY, 0f),
            transform.rotation) as GameObject;
			
		int type = Random.Range(1, 5); //smile, diamond, star, heart
		
		SpriteRenderer sr;
        sr = iconToSet.GetComponent<SpriteRenderer>();
		sr.color = new Color (0.3f, 0.3f, 0.3f);
		switch (type)
			{
				case 1:
					sr.sprite = sprite1;
					break;
				case 2:
					sr.sprite = sprite2;
					break;
				case 3:
					sr.sprite = sprite3;
					break;
				case 4:
					sr.sprite = sprite4;
					break;
				default:
					break;
			}
		
		switch (num + 1)
			{
				case 1:
					icon1 = iconToSet;
					icon1type = type;
					break;
				case 2:
					icon2 = iconToSet;
					icon2type = type;
					break;
				case 3:
					icon3 = iconToSet;
					icon3type = type;
					break;
				case 4:
					icon4 = iconToSet;
					icon4type = type;
					break;
				case 5:
					icon5 = iconToSet;
					icon5type = type;
					break;
				default:
					break;
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

	public void BulletHit (int type) 
	{
		int typeNeeded;
		switch (progress)
			{
				case 0:
					typeNeeded = icon1type;
					break;
				case 1:
					typeNeeded = icon2type;
					break;
				case 2:
					typeNeeded = icon3type;
					break;
				case 3:
					typeNeeded = icon4type;
					break;
				case 4:
					typeNeeded = icon5type;
					break;
				default:
					typeNeeded = 1;
					break;
			}
		
		if (type == typeNeeded) {
			
			SpriteRenderer sr;
			Animator ar;
			switch (progress)
			{
				case 0:
					sr = icon1.GetComponent<SpriteRenderer>();
					ar = icon1.GetComponent<Animator>();
					break;
				case 1:
					sr = icon2.GetComponent<SpriteRenderer>();
					ar = icon2.GetComponent<Animator>();
					break;
				case 2:
					sr = icon3.GetComponent<SpriteRenderer>();
					ar = icon3.GetComponent<Animator>();
					break;
				case 3:
					sr = icon4.GetComponent<SpriteRenderer>();
					ar = icon4.GetComponent<Animator>();
					break;
				case 4:
					sr = icon5.GetComponent<SpriteRenderer>();
					ar = icon5.GetComponent<Animator>();
					break;
				default:
					sr = icon1.GetComponent<SpriteRenderer>();
					ar = icon1.GetComponent<Animator>();
					break;
			}
			sr.color = new Color (1f, 1f, 1f);
			ar.SetTrigger("Dance");
			progress++;
			if (progress == numIcons) {
				
				phaseNum++;
				speed += 0.5f;
				phase(phaseNum);
			}
		
		} else {
			phase(phaseNum);
		}
		
	}
	
	public override IEnumerator OnDeath ()
	{
		an.SetTrigger ("Dead");
		Debug.Log("fan est mort");
		yield return new WaitForSecondsRealtime(1);

		//Update Game Controller and clear room
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			gcObj.GetComponent<GameController> ().DefeatBoss (4);
			gcObj.GetComponent<BackgroundMusic> ().SwitchLayers (gcObj.GetComponent<BackgroundMusic> ().currLayer);
		}
		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		foreach (GameObject door in doors) {
			door.GetComponent<Animator> ().SetTrigger ("Open");
		}
		if (exitPlatform != null) {
			exitPlatform.SetActive (true);
		}
		
		Destroy(gameObject);
	}
	
	public void HurtFinished () {
		mercyInvincibility = false;
		if (alive) {
			canMove = true;
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.tag == "Player") {
			containsPlayer = true;
		} 
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Player") {
			containsPlayer = false;
		}
	}
	//start moving in a new direction
}
