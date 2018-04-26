using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

	//constants
	private static int MAXBRIDGES = 5;

	//references
	private Animator an;
	private GameController gc;

	//variables
	public int switchNum = 0;
	public bool switchState = false;
	public float cooldown = 3.00f;
	public bool active = true;
	private float timer;
	public GameObject[] plats;
	public GameObject[] switches;
	public GameObject fan;
	

	// Use this for initialization
	void Start () {
		plats = GameObject.FindGameObjectsWithTag("FlipPlatform");
		switches = GameObject.FindGameObjectsWithTag("FlipSwitch");
		fan = GameObject.Find("Fan");
		an = GetComponent<Animator> ();
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj == null) {
			return;
		}
		gc = gcObj.GetComponent<GameController> ();
		switchState = gc.rotationSwitchStates[switchNum];
		if (switchState == true) {
			an.SetTrigger ("Other");
		}
		
		
		
	}
	
	void Update () {
		
			timer += Time.deltaTime;
			if (timer >= cooldown) {
				GetComponent<Animator>().SetTrigger ("Switch");
				foreach (GameObject plat in plats)
				{
					plat.GetComponent<GapPlatform>().Flip();
				}
				fan.GetComponent<FanWindToggleable>().active = !fan.GetComponent<FanWindToggleable>().active;
				timer = 0f;
			}
		
	}
	

	/// <summary>
	/// Switch this the state of this switch, and rotate the wind bridge.
	/// </summary>
	void Switch () {
		
		//
		
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.gameObject.GetComponent<PlayerSword>() != null && active) {
			foreach (GameObject s in switches)
			{
				s.GetComponent<Animator>().SetTrigger ("Switch");
			}
			foreach (GameObject plat in plats)
			{
				plat.GetComponent<GapPlatform>().Flip();
			}
			active = false;
		}
	}
}
