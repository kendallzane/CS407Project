using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    protected GameController gc;
    protected Rigidbody2D rb;
    protected Animator an;
    protected float health = 100.0f;
    public float healthMax = 100.0f;  

    protected virtual void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    public virtual void AddHealth(float restore)  
    {
        health += restore;
        if (health > healthMax)
        {
            health = healthMax;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0.0f;
        }
    }

}
