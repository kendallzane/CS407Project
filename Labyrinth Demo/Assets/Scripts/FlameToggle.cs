using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameToggle : ButtonLogic {

    private GameObject fire;
	private bool fireActive;

	// Use this for initialization
	void Start () {
		fire = gameObject;
		fireActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void switchIt(){
        fireActive = !fireActive;
        fire.SetActive(fireActive);
    }

}
