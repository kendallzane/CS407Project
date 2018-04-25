using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour {

	public GameObject other;
	bool pressed = false;
	public Sprite pressedSprite;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void onPress(){
		
		other.SendMessage("ButtonPressed");
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
