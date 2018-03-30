using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour {

    public int damageToPlayer;                              // Damage dealt to player

    
    public GameObject baby;                              	// baby to spawn
	public float timer = 0.0f;
	public float timeToRespawn = 5.0f;
	public bool respawning = false;
	
	public GameObject fake;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (respawning) {
			timer += Time.deltaTime;
			if (timer >= timeToRespawn) {
				SpriteRenderer sr;
				sr = GetComponent<SpriteRenderer>();
				SpriteRenderer srb;
				Animator sra;
				sra = fake.GetComponent<Animator>();
				srb = fake.GetComponent<SpriteRenderer>();
				sr.enabled = true;
				sra.enabled = true;
				srb.enabled = true;
				respawning = false;
				foreach(BoxCollider2D c in GetComponents<BoxCollider2D> ()) {
					c.enabled = true;
				}
				timer = 0.0f;
			}
		}
    }

    // Hurt the player character when colliding
    // Reflect the projectile back to the flier when colliding with the sword
    // Destroy the projectile with any other collisions
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        
        if (coll.tag == "Sword" && coll.isTrigger && !respawning)
        {
			GameObject Drop;
			Drop = Instantiate(
				baby,
				transform.position,
				transform.rotation) as GameObject;
				
			Drop.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			Drop.GetComponent<H2OBoy>().pushBackForce = 100f;
			Drop.layer = 0;
			SpriteRenderer sr;
			sr = GetComponent<SpriteRenderer>();
			SpriteRenderer srb;
			Animator sra;
			sra = fake.GetComponent<Animator>();
			srb = fake.GetComponent<SpriteRenderer>();
			sr.enabled = false;
			sra.enabled = false;
			srb.enabled = false;
			respawning = true;
			foreach(BoxCollider2D c in GetComponents<BoxCollider2D> ()) {
					c.enabled = false;
				}
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
