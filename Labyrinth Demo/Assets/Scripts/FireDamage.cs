using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

public class FireDamage : MonoBehaviour {

    public Vector2 locationToRespawn;
    public int damagePerTick;
    private bool containsPlayer = false;
    private Collider2D player;

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag == "Player") {
            containsPlayer = true;
            player = coll;
        } 
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.tag == "Player") {
            containsPlayer = false;
        }
    }

    void Update(){
        if(containsPlayer && player != null)
            player.GetComponent<PlayerHealth>().TakeDamage(damagePerTick, new Vector2(0,0));
    }
}
