using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystalHit : MonoBehaviour {

    public GameObject enemyToSpawn;

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag == "Sword") {
            GameObject go = Instantiate(enemyToSpawn);
            go.transform.localScale = new Vector3(0.15f, 0.15f, 1f);
            go.transform.position = this.transform.position;
        } 
    }
}
