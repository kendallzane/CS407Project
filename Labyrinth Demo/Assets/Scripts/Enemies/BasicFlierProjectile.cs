using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlierProjectile : MonoBehaviour {

    public int damageToPlayer;                              // Damage dealt to player
    public int damageToSelf;                                // Damage to the Basic Flier

    private float speed = 5f;                               // The speed, in units per second, the projectile will move towards the target
    private bool moveTowardsTarget = false;                 // Controls whether the targets return to the flier
    public GameObject Owner;                                // Flier that created the projectile
	
	// Update is called once per frame
	void Update () {
        if (moveTowardsTarget)
        {
            // Get position of projectile and flier
            Vector3 targetPosition = Owner.transform.position;
            Vector3 currentPosition = this.transform.position;

            // Check to see if we're close enough to the target
            if (Vector3.Distance(currentPosition, targetPosition) > .1f)
            {
                // Determine distance
                Vector3 directionOfTravel = targetPosition - currentPosition;

                // Nromalize the direction
                directionOfTravel.Normalize();

                // Move the projectile towards the flier
                this.transform.Translate(
                    (directionOfTravel.x * speed * Time.deltaTime),
                    (directionOfTravel.y * speed * Time.deltaTime),
                    (0),
                    Space.World);
            }
            else
            {
                // Destroy the projectile and deal damage to the flier once in a certain range
                Destroy(gameObject);
                Owner.GetComponent<EnemyAI>().TakeDamage(damageToSelf, (Vector2)Owner.transform.position - (Vector2)transform.position);
            }
        }
    }

    // Hurt the player character when colliding
    // Reflect the projectile back to the flier when colliding with the sword
    // Destroy the projectile with any other collisions
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.tag == "Player" && coll.isTrigger)
        {
            coll.GetComponent<PlayerHealth>().TakeDamage(damageToPlayer, (Vector2)coll.transform.position - (Vector2)transform.position);
            Destroy(gameObject);
        }
        else if (coll.tag == "Sword" && coll.isTrigger)
        {
            moveTowardsTarget = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Destroy the projectile when it collides with the edge of the map
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            Destroy(gameObject);
        }
    }
}
