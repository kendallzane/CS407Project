using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

	//references
	public Slider dashBar;				//the visual indicator of how many dashes are remaining
	public Slider healthBar;			//the visual indicator of how much health the player has remaining
	public Slider[] commandBar;			//the visual indicator of how long until a command is recharged
	public Image dashForeground;		//the front color of the dash
	public Image dashBackground;		//the back color of the dash
	private Animator an;
	private PlayerCombat pc;

	//variables
	public Color[] dashColors;			//the colors used to indicate how many charges are remaining
	private bool canChange = true;

	// Use this for initialization
	void Start () {
		GetComponent<Canvas> ().worldCamera = Camera.main;
		GetComponent<Canvas> ().planeDistance = 1;
		an = GetComponent<Animator> ();
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerCombat> ();
		canChange = true;
		for (int i = 0; i < pc.commands.Length; i++) {
			commandBar [i].GetComponentInChildren<Text> ().text = pc.commands [i].commandName;
			pc.commands [i].commandNum = i;
		}
		ResetSelection ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Minimize")) {
			Minimize ();
		}
		if (canChange && Input.GetButtonDown ("ChangeCommand")) {
			ChangeCommand ();
		}
	}

	/// <summary>
	/// Changes the selected command. (Done in PlayerHUD first, then this calls player combat's change command)
	/// </summary>
	public void ChangeCommand () {
		canChange = false;
		pc.canCommand = false;
		an.SetTrigger ("ChangeCommand");
	}

	// Called by animations to show menu switch is completed
	public void EndCommandSwitch () {
		canChange = true;
		pc.ChangeCommand ();
	}

	/// <summary>
	/// Shows the health in the HUD.
	/// </summary>
	/// <param name="health">Health remaining.</param>
	public void ShowHealth (int health) {
		healthBar.value = health;
	}

/// <summary>
/// Shows the time remaining until dash is charged.
/// </summary>
/// <param name="numCharges">Number of charges.</param>
/// <param name="time">The time until a new charge is done / time it takes to fill a complete charge.</param>
	public void ShowDash (int numCharges, float timeFraction) {
		if (timeFraction < 0) {
			timeFraction = 0;
		}
		dashForeground.color = dashColors [numCharges + 1];
		dashBackground.color = dashColors [numCharges];
		dashBar.value = 1 - timeFraction;
	}

	// Called when the player hits the minimize button
	public void Minimize () {
		//Minimize the HUD
		an.SetTrigger("Minimize");
	}

	/// <summary>
	/// Resets the selection of the HUD to what it should be.
	/// </summary>
	public void ResetSelection () {
		an.SetInteger ("SelectedCommand", pc.selectedCommand);
		an.SetTrigger ("ResetSelection");
	}
}
