using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEdgeCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Ignore collisions with anything but golems
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.tag == "Player" && coll.isTrigger)
        {
            Physics.IgnoreCollision(coll.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
