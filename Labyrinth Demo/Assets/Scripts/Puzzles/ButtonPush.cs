﻿using System.Collections;
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
            if(canal.GetComponent<ButtonLogic>() != null) canal.GetComponent<ButtonLogic>().switchIt();
            if(canal.GetComponent<FlameToggle>() != null) canal.GetComponent<FlameToggle>().switchIt();
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.tag == "Sword") {
			onPress (); 
		} 
	}
			

}
