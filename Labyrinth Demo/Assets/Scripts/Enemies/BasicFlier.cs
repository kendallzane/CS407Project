using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlier : EnemyAI
{

    public GameObject BasicFlierProjectile;             // Drag in the Projectile from the Component Inspector
    public Transform ProjectileSpawn;                   // Drag in the Projectile Spawn from the Component Inspector
    public float ProjectileSpeed;                       // Enter the speed of the Projectile from the Component Inspector
    public float speed;                                 // Speed basic flier travels in
    public int damage;                                  // How much damage does the basic flier do?
    public float pushBackForce;                         // How quickly does the basic flier get pushed back?
    public float timeSinceFire;                         // Time since last projectile was fired
    public float fireRate;                              // Fire rate of the basic flier

    private bool canMove;                               // Can the basic flier move?

    // Use this for initialization
    void Start()
    {
        // Allow the flier to move
        canMove = true;

        // Set initial fire rate to 2, so the flier shoots the very first projectile more quickly
        timeSinceFire = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Create new instance of Player object with each loop
            GameObject Player = GameObject.Find("MainCharacter");

            // Find distance between Player and BasicFlier
            Vector3 targetPosition = Player.transform.position;
            Vector3 currentPosition = this.transform.position;
            Vector3 difference = targetPosition - currentPosition;
            
            // Use the location differences to keep the flier a safe distance from the player
            // All of the following are conditionals for which direction to move
            if (difference.x > 3)
            {
                if (difference.y > 2)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 1.5 && difference.y >= 0)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 0 && difference.y > -1.5)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < -2)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else
                {
                    rb.velocity = new Vector2(speed, 0);
                }
            }
            else if (difference.x < 2 && difference.x >= 0)
            {
                if (difference.y > 2)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 1.5 && difference.y >= 0)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 0 && difference.y > -1.5)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < -2)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else
                {
                    rb.velocity = new Vector2(speed * -1, 0);
                }
            }
            else if (difference.x < 0 && difference.x > -2)
            {
                if (difference.y > 2)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 1.5 && difference.y >= 0)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 0 && difference.y > -1.5)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < -2)
                {
                    rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else
                {
                    rb.velocity = new Vector2(speed, 0);
                }
            }
            else if (difference.x < -3)
            {
                if (difference.y > 2)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 1.5 && difference.y >= 0)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < 0 && difference.y > -1.5)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                }
                else if (difference.y < -2)
                {
                    rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                }
                else
                {
                    rb.velocity = new Vector2(speed * -1, 0);
                }
            }
            else
            {
                if (difference.y > 2)
                {
                    rb.velocity = new Vector2(0, speed);
                }
                else if (difference.y < 1.5 && difference.y >= 0)
                {
                    rb.velocity = new Vector2(0, speed * -1);
                }
                else if (difference.y < 0 && difference.y > -1.5)
                {
                    rb.velocity = new Vector2(0, speed);
                }
                else if (difference.y < -2)
                {
                    rb.velocity = new Vector2(0, speed * -1);
                }
                else
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
        }

        // Update time since the flier last fired projectiles
        timeSinceFire += Time.deltaTime;

        // After a specified length of time, fire eight projectiles
        if (timeSinceFire > fireRate)
        {
            for (int i = 0; i < 8; i++)
            {
                rb.velocity = Vector3.zero;
                an.SetTrigger("Attack");
                canMove = false;
                StartCoroutine(FireProjectile(i));
             
            }
            timeSinceFire = 0f;

        }
    }

    // Hurt the player character by running into them
    // Move through other enemies
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player" && coll.isTrigger) {
			coll.GetComponent<PlayerHealth>().TakeDamage (damage, (Vector2) coll.transform.position - (Vector2) transform.position);
		}
        else if (coll.tag == "Enemy" && coll.isTrigger)
        {
            Physics.IgnoreCollision(coll.GetComponent<Collider>(), GetComponent<Collider>());
        }
	}

    // Takes damage similar to the main character.
    public override void TakeDamage(int damage, Vector2 dirHit)
    {
        canMove = false;
        eh.TakeDamage(damage);
        an.SetTrigger("Hurt");
        rb.velocity = Vector2.zero;
        rb.AddForce(dirHit.normalized * pushBackForce);
    }

    // Allow the flier to begin moving again
    public void HurtFinished()
    {
        canMove = true;
    }

	public override IEnumerator OnDeath ()
	{
		yield return null;
		Destroy(gameObject);
	}
	
    // Fire eight projectile at the player
    IEnumerator FireProjectile(int num)
    {
        // Wait for one second before firing projectiles (animation)
        yield return new WaitForSecondsRealtime(1);

        // The Projectile instantiation happens here
        GameObject Projectile;
        Projectile = Instantiate(
            BasicFlierProjectile,
            ProjectileSpawn.transform.position,
            ProjectileSpawn.transform.rotation) as GameObject;

        // Set the owner of the new Projectile to this gameObject
        Projectile.GetComponent<BasicFlierProjectile>().Owner = gameObject;

        // Retrieve the Rigidbody component from the instantiated Projectile and control it
        Rigidbody2D ProjectileRigidBody;
        ProjectileRigidBody = Projectile.GetComponent<Rigidbody2D>();

        // Tell the Projectile to be "pushed" toward the player by an amount set by ProjectileSpeed
        // Each case is the direction of one projectile
        switch (num)
        {
            case 0:
                ProjectileRigidBody.AddForce(transform.up * ProjectileSpeed);
                break;
            case 1:
                ProjectileRigidBody.AddForce(transform.up * ProjectileSpeed * Mathf.Sqrt(2)/2);
                ProjectileRigidBody.AddForce(transform.right * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                break;
            case 2:
                ProjectileRigidBody.AddForce(transform.right * ProjectileSpeed);
                break;
            case 3:
                ProjectileRigidBody.AddForce(transform.right * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                ProjectileRigidBody.AddForce(transform.up * -1 * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                break;
            case 4:
                ProjectileRigidBody.AddForce(transform.right * -1 * ProjectileSpeed);
                break;
            case 5:
                ProjectileRigidBody.AddForce(transform.up * -1 * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                ProjectileRigidBody.AddForce(transform.right * -1 * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                break;
            case 6:
                ProjectileRigidBody.AddForce(transform.up * -1 * ProjectileSpeed);
                break;
            case 7:
                ProjectileRigidBody.AddForce(transform.right * -1 * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                ProjectileRigidBody.AddForce(transform.up * ProjectileSpeed * Mathf.Sqrt(2) / 2);
                break;
        }

        // Wait for one second before firing projectiles (animation)
        yield return new WaitForSecondsRealtime(1);

        // Allow the flier to move again
        canMove = true;

        // Basic clean up, set the Projectile to self destruct after 2 seconds.
        Destroy(Projectile, 4.0f);
    }
}
