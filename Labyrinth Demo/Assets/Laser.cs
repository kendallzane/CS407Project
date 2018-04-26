using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	
	public float animRate = 0.1f;
	public float changeW = 0.05f;
	private float timer;
	private float maxW = 1;
	private float minW = 0.5f;
	private int dir = 1;
	public int laserNum = 0;
	public GameObject gcObj;
	public GameController gc;
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
		if (gc.redLaserDestroyed && laserNum == 1) {
			Destroy(gameObject);
		}
		if (gc.blueLaserDestroyed && laserNum == 2) {
			Destroy(gameObject);
		}
			
		
		timer += Time.deltaTime;
		if (timer >= animRate) {
			if (gameObject.transform.localScale.x >= maxW || gameObject.transform.localScale.x <= minW) {
				dir *= -1;
			}
			gameObject.transform.localScale += new Vector3(dir * changeW,0,0);
			timer = 0;
		}
		
	}
}
