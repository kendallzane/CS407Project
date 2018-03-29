﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	//constants
	const int BASE = 0;
	const int EARTH = 1;
	const int FIRE = 2;
	const int WATER = 3;
	const int WIND = 4;

	//references

	//variables
	public bool[] treasureChests;							//a list of all treasureChests in the Labyrinth, true if it has been opened
	public bool[] unlockedDoors;							//a list of all lockedDoors in the Labyrinth, true if it has been unlocked
	[HideInInspector] public bool[] bossDefeats;			//based on the constants above, has the proper boss been defeated?
	[HideInInspector] public int[] playerKeysHeld;			//how many of each type of key is the player holding? Based on constants above
	[HideInInspector] public bool[] roomsExplored;			//which rooms has the player explored? (to show on the map in the pause menu)
	[HideInInspector] public bool toBeDestroyed = false;	//should the gameObject call starting functions?
	[HideInInspector] public int currLayer = BASE;			//what layer is the player currently in?

	//playerValues (could be good to refactor as a struct)
	[HideInInspector] public int maxHealth;					//what is the maxHealth of the player?
	[HideInInspector] public int health;					//the Player's health
	[HideInInspector] public int maxDashCharges;			//what is the maxDashCharges of the player?
	[HideInInspector] public int dashCharges;				//how many charges does the player have?
	[HideInInspector] public float dashTimeDelay;			//how long until the dash is recharged?
	[HideInInspector] public int selectedCommand;			//the Player's currently selected command
	[HideInInspector] public float[] commandCharges;		//the time remaining on the player's commandCharges
	[HideInInspector] public int swordUpgrade;				//what upgrade is the player's sword at?

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		playerKeysHeld = new int[5];
		commandCharges = new float[3];
		bossDefeats = new bool[5];
		roomsExplored = new bool[SceneManager.sceneCountInBuildSettings];
		roomsExplored [0] = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Called every time a new Scene is transitioned to. 
	/// </summary>
	public void OnSceneStart () {
		if (!toBeDestroyed) {

			//Keep player values consistent
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent<PlayerCombat> ().PlayerSetup (maxHealth, health, maxDashCharges, dashCharges, dashTimeDelay, selectedCommand, commandCharges, swordUpgrade);
			GetComponentInChildren<GameOver> ().playerHealth = player.GetComponent<PlayerHealth> ();

			//Keep track of enemy respawns
		}
	}

	#region HelpfulFunctions

	/// <summary>
	/// Called just before a transition to record the player's current status.
	/// </summary>
	public void UpdatePlayerValues () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		health = player.GetComponent<PlayerHealth> ().GetHealth ();
		maxHealth = player.GetComponent<PlayerHealth> ().maxHealth;
		PlayerCombat pc = player.GetComponent<PlayerCombat> ();
		maxDashCharges = pc.numDashes;
		dashCharges = pc.GetDashCharges ();
		dashTimeDelay = pc.GetDashDelay ();
		selectedCommand = pc.selectedCommand;
		for (int i = 0; i < commandCharges.Length; i++) {
			commandCharges [i] = pc.commands [i].GetRechargeDelay ();
		}
		swordUpgrade = player.GetComponentInChildren<PlayerSword> ().upgrade;
	}

	/// <summary>
	/// Let the GameController know that the boss has been defeated.
	/// </summary>
	/// <param name="element">The element of the boss. 1 = Earth, 2 = Fire, 3 = Water, 4 = Wind.</param>
	public void DefeatBoss (int element) {
		bossDefeats [element] = true;
	}

	#endregion
}
