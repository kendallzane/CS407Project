using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBossProjectile : MonoBehaviour {

    public int damageToPlayer;                              // Damage dealt to player

    
    public GameObject FanBoss;                              // Boss that created the projectile
	
	public int type = 0; //smile, diamond, star, heart, above are bullet
	public Sprite sprite1, sprite2, sprite3, sprite4, sprite5;
	
	void Start () {
		SpriteRenderer sr;
		sr = GetComponent<SpriteRenderer>();
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
				sr.sprite = sprite5;
				break;
		}
	}
	
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
        else if (coll.tag == "Sword" && coll.isTrigger)
        {
			if (type > 4) {
				Destroy(gameObject);
			} else {
				if (FanBoss && FanBoss.GetComponent<WindBoss>()) {
					FanBoss.GetComponent<WindBoss>().BulletHit(type);
					Destroy(gameObject);
				}
			}
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    // Destroy the projectile when it collides with the edge of the map
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.layer == 0)
        {
            Destroy(gameObject);
        }
		*/
    }
}
