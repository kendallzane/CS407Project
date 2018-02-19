using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour {

	public Sprite activeCanalSprite;
	public Sprite reserveCanalSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void switchIt(){
		Sprite temp = activeCanalSprite;
		activeCanalSprite = reserveCanalSprite;
		reserveCanalSprite = temp;
		this.GetComponent<SpriteRenderer>().sprite = activeCanalSprite;
	}

}
