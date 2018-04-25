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
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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
