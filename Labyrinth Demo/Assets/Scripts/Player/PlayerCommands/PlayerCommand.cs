using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCommand : MonoBehaviour {

	//references
	protected GameObject player;
	protected Animator an;
	protected PlayerCombat pc;
	protected PlayerHUD ph;
	protected PlayerHealth php;
	protected PlayerMovement pm;
	protected PlayerSword ps;
	protected Rigidbody2D rb;

	//variables
	public float rechargeTime = 10.0f;					//how long must the command recharge until it can be used again?
	[HideInInspector] public bool canUse = true;		//is the command charged and usable?
	[HideInInspector] public int commandNum;			//which numbered command is this?
	public string commandName;							//what is this command called? (To be overridden)
	protected float rechargeDelay = 0;

	// Use this for initialization
	public void OnStart () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pm = player.GetComponent<PlayerMovement> ();
		rb = player.GetComponent<Rigidbody2D> ();
		an = player.GetComponent<Animator> ();
		ps = player.GetComponentInChildren<PlayerSword> ();
		ph = GameObject.FindGameObjectWithTag("HUD").GetComponent<PlayerHUD> ();
		php = player.GetComponent<PlayerHealth> ();
		pc = player.GetComponent<PlayerCombat> ();
		rechargeDelay = 0;
		canUse = true;
	}
	
	// Update is called once per frame
	public void OnUpdate () {
		if (rechargeDelay > 0) {
			rechargeDelay -= Time.deltaTime;
			if (rechargeDelay <= 0) {
				//Command recharged!
				rechargeDelay = 0;
				canUse = true;
			}
			ph.commandBar [commandNum].value = rechargeDelay / rechargeTime;
		}
	}

	/// <summary>
	/// Uses the selected command by calling its unique action, also begins the recharge process
	/// </summary>
	public virtual void UseCommand () {
		canUse = false;
		rechargeDelay = rechargeTime;
	}
}
