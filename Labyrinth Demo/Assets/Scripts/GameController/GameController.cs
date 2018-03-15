using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//references
	//private Transition trans;

	//variables
	public bool[] treasureChests;							//a list of all treasureChests in the Labyrinth, true if it has been opened
	public bool[] unlockedDoors;							//a list of all lockedDoors in the Labyrinth, true if it has been unlocked

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		//trans = GetComponent<Transition> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Called every time a new Scene starts. 
	/// </summary>
	public void OnSceneStart () {
		//Keep track of enemy respawns
	}
}
