using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSprite : MonoBehaviour {
	
	public Sprite s0, s1;
	private int i = 0;
	public float minChangeTime;
	public float maxChangeTime;
	private float timerGoal;
	private float timer;
	// Use this for initialization
	void Start () {
		i = Random.Range(0,2);
		if (i == 0){
			GetComponent<SpriteRenderer>().sprite = s0;
		} else {
			GetComponent<SpriteRenderer>().sprite = s1;
		}
		timerGoal = Random.Range(minChangeTime, maxChangeTime);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= timerGoal) {
			if (i == 0){
				GetComponent<SpriteRenderer>().sprite = s1;
				i = 1;
			} else {
				GetComponent<SpriteRenderer>().sprite = s0;
				i = 0;
			}
			timerGoal = Random.Range(minChangeTime, maxChangeTime);
			timer = 0f;
		}
		
	}
}
