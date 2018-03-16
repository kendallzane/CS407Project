using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[HideInInspector] public int[] playerKeysHeld;			//how many of each type of key is the player holding? Based on constants above
	[HideInInspector] public bool toBeDestroyed = false;	//should the gameObject call starting functions?

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
	void Start () {
		DontDestroyOnLoad (gameObject);
		playerKeysHeld = new int[5];
		commandCharges = new float[3];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
}
