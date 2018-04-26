using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLaserDestroy : MonoBehaviour {

	bool pressed = false;
	public Sprite pressedSprite;
	public int laserToDestroy = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void onPress(){
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj == null) {
			return;
		}
		GameController gc = gcObj.GetComponent<GameController> ();
		if (laserToDestroy == 1) {
			gc.redLaserDestroyed = true;
		} else {
			gc.blueLaserDestroyed = true;
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Sword" && !pressed) {
			onPress(); 
			pressed = true;
			GetComponent<SpriteRenderer>().sprite = pressedSprite;
			BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();
			foreach(BoxCollider2D bc in myColliders) bc.enabled = false;
		} 
	}
}
