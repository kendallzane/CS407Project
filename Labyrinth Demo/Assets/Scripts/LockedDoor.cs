using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

	//constants
	const int BASE = 0;
	const int EARTH = 1;
	const int FIRE = 2;
	const int WATER = 3;
	const int WIND = 4;

	//references
	private Animator an;
	private GameController gc;
	public GameObject theLock;

	//variables
	public bool isLock = false;									//only set to true if this door is supposed to be locked
	public int lockNum = -1;									//which lock is this in the game? (for keeping track by gameController)
	public int lockType = 0;									//what type of lock is this? Based on the constants above
	[HideInInspector] public bool unlocked = false;				//is the door locked?

	[SerializeField] private Color[] typeColors;				//the color of the lock to match its section;

	// Use this for initialization
	void Start () {
		an = GetComponent<Animator> ();
		if (isLock) {
			GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
			if (gcObj != null) {
				gc = gcObj.GetComponent<GameController> ();		//account for if there is no GameController


				if (!gc.unlockedDoors [lockNum]) {				//maintain opened status after coming back to the scene
					an.SetBool ("Locked", true);
					unlocked = false;
				} else {
					unlocked = true;
					an.SetBool ("Locked", false);
				}
			} else {
				Debug.Log ("Locked door can't find the GameController");
			}

			theLock.GetComponent<SpriteRenderer> ().color = typeColors [lockType];
		}
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (isLock && !unlocked && coll.tag == "Sword") {
			if (gc.playerKeysHeld[lockType] > 0) {
				//Unlocked!
				an.SetBool("Locked", false);
				an.SetTrigger("Unlock");
				unlocked = true;
			} else {
				Debug.Log("You doesn't have a matching key!");
			}
		}
	}

	/// <summary>
	/// Called by the animator to let the GameController know that the door is unlocked and a key has been used.
	/// </summary>
	public void DoorJustUnlocked () {
		gc.playerKeysHeld [lockType]--;
		gc.unlockedDoors [lockNum] = true;
	}
}
