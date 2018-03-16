using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurricaneBladeCommand : PlayerCommand {

	public float timeToSpin = 2.0f;

	private bool spinning = false;
	private float timeDelay = 0f;

	// Use this for initialization
	void Start () {
		OnStart ();
		timeDelay = 0f;
		spinning = false;
	}

	// Update is called once per frame
	void Update () {
		OnUpdate ();
		if (spinning) {
			timeDelay += Time.deltaTime;

			if (timeDelay > timeToSpin) {
				//End Spin
				spinning = false;
				php.damageCollider.enabled = true;
				timeDelay = 0f;
				an.SetBool ("HurricaneBlade", false);
				pc.EnablePlayer ();
			}
		}
	}

	/// <summary>
	/// Uses the selected command by calling its unique action, also begins the recharge process
	/// </summary>
	public override void UseCommand () {
		base.UseCommand ();
		timeDelay = 0f;
		spinning = true;
		php.damageCollider.enabled = false;
		pc.DisablePlayer ();
		an.SetBool ("HurricaneBlade", true);
		an.SetTrigger ("Attack");
		pm.canMove = true;								//disable everything except movement
	}
}
