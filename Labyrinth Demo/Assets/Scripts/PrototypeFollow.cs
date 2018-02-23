using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeFollow : MonoBehaviour {
	private Transform target;
	[HideInInspector] public bool clamped = false;
	public float heightDamping = 0.5f;
	public float widthDamping = 0.5f;
	public float smoothTime = 0.5f;
	public float MinX = -100.0f;
	public float MaxX = 100.0f;
	public float MinY = -100.0f;
	public float MaxY = 100.0f;

	private Vector3 velocity = Vector3.zero;

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void FixedUpdate () {
		if (!clamped) {
			//Only move camera if character is out of the middle of the screen
			if (target != null && (Mathf.Abs (target.position.x - transform.position.x) > widthDamping || Mathf.Abs (target.position.y - transform.position.y) > heightDamping)) {
				Vector3 goalPos = target.position;
				goalPos.z = transform.position.z;
				transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);

				//don't move past the edges
				transform.position = new Vector3 (
					Mathf.Clamp (transform.position.x, MinX, MaxX),
					Mathf.Clamp (transform.position.y, MinY, MaxY),
					transform.position.z);
			}
		}
	}
}