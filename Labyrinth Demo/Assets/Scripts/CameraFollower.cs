using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

	//references
	[HideInInspector] public bool clamped = false;
	public float heightDamping = 0.15f;
	public float widthDamping = 0.15f;
	public float smoothTime = 0.4f;

	private Transform target;
	private Vector3 velocity = Vector3.zero;
	private GameObject borders;

	//variables
	public float MinX = 0, MaxX = 0, MinY = 0, MaxY = 0;			//bounds of the camera, calculated from the borders
	

	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		borders = GameObject.FindGameObjectWithTag ("Borders");
		if (borders == null) {
			Debug.Log ("No borders found, using manual values");
			
		} else {

			Vector2[] colliderpoints = borders.GetComponent<EdgeCollider2D>().points;
			MinX = colliderpoints[0].x;
			MaxX = colliderpoints[0].x;
			MinY = colliderpoints[0].y;
			MaxY = colliderpoints[0].y;
			//Get the cornermost edges of the scene
			foreach (Vector2 point in colliderpoints) {
				if (point.x < MinX) {
					MinX = point.x;
				}
				if (point.x > MaxX) {
					MaxX = point.x;
				}
				if (point.y < MinY) {
					MinY = point.y;
				}
				if (point.y > MaxY) {
					MaxY = point.y;
				}
			}

			//adjust the edges to the size of the scren
			MinX += (Camera.main.orthographicSize * (16f/9f));
			MaxX -= (Camera.main.orthographicSize * (16f/9f));
			MinY += Camera.main.orthographicSize;
			MaxY -= Camera.main.orthographicSize;

			//fix for if the screen is too small
			if ((MaxX < MinX)) {
				float tmp;
				tmp = MaxX;
				MaxX = MinX;
				MinX = tmp;
			}
			if ((MaxY < MinY)) {
				float tmp;
				tmp = MaxY;
				MaxY = MinY;
				MinY = tmp;
			}
		}
	}

	void FixedUpdate () {
		if (!clamped) {
			//Only move camera if character is out of the middle of the screen
			if (target != null && (Mathf.Abs (target.position.x - transform.position.x) > widthDamping || Mathf.Abs (target.position.y - transform.position.y) > heightDamping)) {
				Vector3 goalPos = target.position;
				goalPos.z = transform.position.z;
				transform.position = Vector3.SmoothDamp (transform.position, goalPos, ref velocity, smoothTime);

				//don't move past the edges
				if (!(transform.position.y < MinY && transform.position.y > MaxY)) {
					transform.position = new Vector3 (
						transform.position.x,
						Mathf.Clamp (transform.position.y, MinY, MaxY),
						transform.position.z);
				}
				if (!(transform.position.x < MinX && transform.position.x > MaxX)) {
					transform.position = new Vector3 (
						Mathf.Clamp (transform.position.x, MinX, MaxX),
						transform.position.y,
						transform.position.z);
				}
			}
		}
	}
}
