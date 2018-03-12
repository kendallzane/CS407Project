using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// To be attached to the GameController to allow seamless transitions to and from different rooms in the Labyrinth.
/// </summary>
public class Transition : MonoBehaviour {

	//references
	private GameController gc;

	//variables
	public string[] sceneNames;									//An array of all possible scenes in the Labyrinth to transition to.
	[HideInInspector] public int entranceNum;					//Which entrance to the room is the character entering in?

	// Use this for initialization
	void Start () {
		gc = GetComponent<GameController> ();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	/// <summary>
	/// Transitions to the specified scene's entrance.
	/// </summary>
	/// <param name="sceneNum">The assigned int to the scene you want to transition to.</param>
	/// <param name="entrance">Which entrance (0 if only one) of the scene are you going to?</param>
	public void TransitionToScene (int sceneNum, int entrance) {

		//Account for multiple entrances
		string toScene = "ParrDev";
		toScene = SwitchNumToString (sceneNum);
		entranceNum = entrance;

		SceneManager.LoadScene (toScene);
	}

	//Perform sanity checks before trying to access array
	string SwitchNumToString (int sceneNum) {
		if (sceneNum < 0 || sceneNum >= sceneNames.Length) {
			Debug.Log ("Trying to enter non-existent Scene");
			sceneNum = 0;
		}

		return sceneNames [sceneNum];
	}

	/// <summary>
	/// Called every time Unity transitions to a new scene.
	/// Used to set up which entrance the character enters.
	/// </summary>
	/// <param name="scene">The newly loaded scene.</param>
	/// <param name="mode">How the scene is loaded.</param>
	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {

		GameObject meh = GameObject.FindGameObjectWithTag ("MultipleEntranceHandler");
		if (meh == null) {
			return;
		}

		meh.GetComponent<EntranceHandler>().ChangeStartPosition (entranceNum);
	}
}
