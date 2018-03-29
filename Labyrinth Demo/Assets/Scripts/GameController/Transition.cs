using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// To be attached to the GameController to allow seamless transitions to and from different rooms in the Labyrinth.
/// </summary>
public class Transition : MonoBehaviour {

	//references
	private GameController gc;
	public Image fadeScreen;

	//variables
	public string[] sceneNames;									//An array of all possible scenes in the Labyrinth to transition to.
	public float transitionTime = 0.5f;							//How long does it take to transition to the next scene?

	private float timeDelay;									//How long have we been fading?
	private bool fading = false;								//Is the scene fading in/out?
	private bool fin = true;									//in or out?
	private string toScene;										//used to transition to the next scene
	[HideInInspector] public int entranceNum;					//Which entrance to the room is the character entering in?

	// Use this for initialization
	void Start () {
		gc = GetComponent<GameController> ();
		SceneManager.sceneLoaded += OnSceneLoaded;
		timeDelay = 0;
		fading = true;
		fin = true;
	}

	// Elegantly fade the screen in and out between room transitions
	void Update () {
		if (fading) {
			timeDelay += Time.deltaTime;
			if (timeDelay > transitionTime) {
				fading = false;
				if (fin) {
					fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0);
				} else {
					//load next scene
					gc.UpdatePlayerValues();
					fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1);
					SceneManager.LoadScene (toScene);
				}
			} else {
				if (fin) {
					fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1 - (timeDelay / transitionTime));
				} else {
					fadeScreen.color = new Color (fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, (timeDelay / transitionTime));
				}
			}
		}
	}

	/// <summary>
	/// Transitions to the specified scene's entrance.
	/// </summary>
	/// <param name="sceneNum">The assigned int to the scene you want to transition to.</param>
	/// <param name="entrance">Which entrance (0 if only one) of the scene are you going to?</param>
	public void TransitionToScene (int sceneNum, int entrance) {

		//Account for multiple entrances
		toScene = SwitchNumToString (sceneNum);
		entranceNum = entrance;

		timeDelay = 0;
		fin = false;
		fading = true;


	}

	//Perform sanity checks before trying to access array
	string SwitchNumToString (int sceneNum) {
		if (sceneNum < 0 || sceneNum >= sceneNames.Length) {
			Debug.Log ("Trying to enter non-existent Scene");
			sceneNum = 5;
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
		gc.OnSceneStart ();
		fin = true;
		timeDelay = 0;
		fading = true;
		gc.roomsExplored [scene.buildIndex] = true;
		GameObject meh = GameObject.FindGameObjectWithTag ("MultipleEntranceHandler");
		if (meh == null) {
			return;
		}

		meh.GetComponent<EntranceHandler>().ChangeStartPosition (entranceNum);
	}
}
