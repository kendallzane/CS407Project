using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystalHit : MonoBehaviour {

    public GameObject enemyToSpawn;

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag == "Sword") {
            GameObject go = Instantiate(enemyToSpawn);
            go.transform.position = this.transform.position;
        } 
    }
}
