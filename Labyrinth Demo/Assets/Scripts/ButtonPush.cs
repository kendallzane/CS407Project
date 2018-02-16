using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPush : MonoBehaviour {

	public GameObject[] canalsToSwitch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onPress(){
		foreach(GameObject canal in canalsToSwitch){
			canal.GetComponent<ButtonLogic>().switchIt();
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Sword") {
			onPress (); 
		} 
	}
			

}
