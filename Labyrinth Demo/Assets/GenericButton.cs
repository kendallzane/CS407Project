using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : MonoBehaviour {

	public GameObject other;
	bool pressed = false;
	public Sprite pressedSprite;
	[HideInInspector] public GameObject gcObj;
	[HideInInspector] public GameController gc;
	// Use this for initialization
	void Start () {
		gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj == null) {
			return;
		}
		gc = gcObj.GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj == null) {
			return;
		}
		gc = gcObj.GetComponent<GameController> ();
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
