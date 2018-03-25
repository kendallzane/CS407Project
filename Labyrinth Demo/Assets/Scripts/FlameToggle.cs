using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameToggle : ButtonLogic {

    public GameObject fire;
    private bool fireActive = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void switchIt(){
        fireActive = !fireActive;
        fire.SetActive(fireActive);
    }

}
