using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneBladeCommand : PlayerCommand {

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
	new public void UseCommand () {
		base.UseCommand ();

	}
}
