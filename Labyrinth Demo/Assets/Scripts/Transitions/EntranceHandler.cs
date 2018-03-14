using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceHandler : MonoBehaviour {

	//references
	private GameObject player;
	private GameObject playerCam;

	//variables
	public Vector3[] characterStartPositions;
	public Vector3[] cameraStartPositions;

	//Get initialized before called
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerCam = Camera.main.gameObject;
	}

	public void ChangeStartPosition (int entrance) {
		if (entrance < 0 || entrance >= characterStartPositions.Length || entrance >= cameraStartPositions.Length) {
			Debug.Log ("The given entrance doesn't exist! Entering at default");
			return;
		}

		player.transform.position = characterStartPositions [entrance];
		playerCam.transform.position = cameraStartPositions [entrance];
		//playerCam.transform.position = new Vector3 (characterStartPositions [entrance].x, characterStartPositions [entrance].y, -10);
	}
}
