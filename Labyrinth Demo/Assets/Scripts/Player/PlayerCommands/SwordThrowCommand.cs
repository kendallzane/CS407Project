using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThrowCommand : PlayerCommand {

	//references
	public GameObject thrownSword;					//the GameObject to instantiate
	public float timeTillThrow = 0.13f / 0.60f;		//how long until the player throw's the sword?

	// Use this for initialization
	void Start () {
		OnStart ();
	}

	// Update is called once per frame
	void Update () {
		OnUpdate ();
	}

	/// <summary>
	/// Uses the selected command by calling its unique action, also begins the recharge process
	/// </summary>
	public override void UseCommand () {
		base.UseCommand ();
		pc.DisablePlayer ();
		an.SetTrigger ("ThrowSword");
		StartCoroutine ("ThrowTheSword");
	}

	/// <summary>
	/// Throws the sword after the animation completes.
	/// </summary>
	/// <returns>The the sword.</returns>
	IEnumerator ThrowTheSword () {
		yield return new WaitForSeconds (timeTillThrow);
		pc.EnablePlayer ();
		pc.LostSword ();
		GameObject sword = Instantiate (thrownSword, player.transform.position, Quaternion.identity);
		sword.GetComponent<ThrownSword> ().player = player;
	}
}
