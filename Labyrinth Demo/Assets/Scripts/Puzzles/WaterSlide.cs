using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlide : MonoBehaviour {

	//constants
	private const int DOWN = 0;
	private const int LEFT = 1;
	private const int UP = 2;
	private const int RIGHT = 3;

	//references

	//variables
	public int dir = DOWN;			//direction the water is flowing
	public float speed = 3f;		//how fast does the player float along?

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerCombat> ().DisablePlayer ();
			switch (dir) {
			case DOWN:
				coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -speed);
				break;
			case LEFT:
				coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, 0);
				break;
			case UP:
				coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
				break;
			case RIGHT:
				coll.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, 0);
				break;
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll) {
		if (coll.tag == "Player") {
			coll.GetComponent<PlayerCombat> ().EnablePlayer ();
		}
	}
}
