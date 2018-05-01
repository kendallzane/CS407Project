using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

	//constants
	const int BASE = 0;
	const int EARTH = 1;
	const int FIRE = 2;
	const int WATER = 3;
	const int WIND = 4;

	//references
	private GameController gc;

	//variables
	public int mapLayer;			//which layer is this map?
	public GameObject[] rooms;		//array of all the rooms
	private int toAdd = 0;			//account for the previous layer's maps

	// Use this for initialization
	void Start () {
		switch (mapLayer) {
		case BASE:
			toAdd = 0;
			break;
		case EARTH:
			toAdd = 6;
			break;
		case FIRE:
			toAdd = 12;
			break;
		case WATER:
			toAdd = 17;
			break;
		case WIND:
			toAdd = 23;
			break;
		}
		foreach (GameObject room in rooms) {
			room.SetActive (false);
		}
		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		gc = gcObj.GetComponent<GameController> ();
	}

	/// <summary>
	/// Called every time the map is opened, to make sure that only the explored rooms are showing.
	/// </summary>
	public void UpdateMap () {
		for(int i = 0; i < rooms.Length; i++) {
			if (gc != null && gc.roomsExplored[i + toAdd]) {
				rooms[i].SetActive (true);
			}
		}
	}
}
