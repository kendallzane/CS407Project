using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour {

	//references
	protected Rigidbody2D rb;
	protected Animator an;
	protected EnemyHealth eh;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		an = GetComponent<Animator> ();
		eh = GetComponent<EnemyHealth> ();
	}

	//what does each enemy do when it takes damage?
	abstract public void TakeDamage (int damage, Vector2 dirHit);
	abstract public IEnumerator OnDeath ();
}
