using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2OChadProjectile : MonoBehaviour {

    public int damageToPlayer;                              // Damage dealt to player

    //private float speed = 5f;                               // The speed, in units per second, the projectile will move towards the target
    //private bool moveTowardsTarget = false;                 // Controls whether the targets return to the flier
   // public GameObject Owner;                                // Flier that created the projectile
	
	// Update is called once per frame
	void Update () {
        
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
