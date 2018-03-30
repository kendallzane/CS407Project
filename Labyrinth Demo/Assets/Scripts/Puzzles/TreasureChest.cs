using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour {

	//constants
	const int BASEKEY = 0;
	const int EARTHKEY = 1;
	const int FIREKEY = 2;
	const int WATERKEY = 3;
	const int WINDKEY = 4;
	const int SWORDUPGRADE = 5;
	const int DASHUPGRADE = 6;
	const int HEALTHUPGRADE = 7;

	//references
	private Animator an;
	private GameController gc;
	public SpriteRenderer itemSpriteObj;
	public Sprite[] containedItemSprites;
	public Color[] keyColors;

	//variables
	public int containedItem;								//based on the constants listed above
	public int chestNum;									//which chest is this in the game? (for keeping track by gameController)
	private bool open = false;								//has the chest already been opened?

	void Start () {
		an = GetComponent<Animator> ();

		//set itemSprite and color based on Type
		switch (containedItem) {
		case BASEKEY:
			itemSpriteObj.sprite = containedItemSprites [0];
			itemSpriteObj.color = keyColors [BASEKEY];
			break;
		case EARTHKEY:
			itemSpriteObj.sprite = containedItemSprites [0];
			itemSpriteObj.color = keyColors [EARTHKEY];
			break;
		case FIREKEY:
			itemSpriteObj.sprite = containedItemSprites [0];
			itemSpriteObj.color = keyColors [FIREKEY];
			break;
		case WATERKEY:
			itemSpriteObj.sprite = containedItemSprites [0];
			itemSpriteObj.color = keyColors [WATERKEY];
			break;
		case WINDKEY:
			itemSpriteObj.sprite = containedItemSprites [0];
			itemSpriteObj.color = keyColors [WINDKEY];
			break;
		case SWORDUPGRADE:
			itemSpriteObj.sprite = containedItemSprites [1];
			break;
		case DASHUPGRADE:
			itemSpriteObj.sprite = containedItemSprites [2];
			break;
		case HEALTHUPGRADE:
			itemSpriteObj.sprite = containedItemSprites [3];
			itemSpriteObj.color = keyColors [FIREKEY];
			break;
		}

		GameObject gcObj = GameObject.FindGameObjectWithTag ("GameController");
		if (gcObj != null) {
			gc = gcObj.GetComponent<GameController> ();		//account for if there is no GameController

			if (gc.treasureChests [chestNum]) {				//maintain opened status after coming back to the scene
				an.SetBool ("Opened", true);
				open = true;
			}
		} else {
			Debug.Log ("Treasure Chest can't find the GameController");
		}
	}

	//Open the chest by hitting it with your sword
	void OnTriggerEnter2D (Collider2D coll) {
		if (!open && coll.tag == "Sword") {
			an.SetTrigger ("Open");
		}
	}

	/// <summary>
	/// Called from Animator to show that the chest has been opened
	/// </summary>
	public void ChestOpened () {
		open = true;
		an.SetBool ("Opened", true);
		if (gc != null) {
			gc.treasureChests [chestNum] = true;
		}

		//Give player the item
		switch (containedItem) {
		case BASEKEY:
			gc.playerKeysHeld [0]++;
			break;
		case EARTHKEY:
			gc.playerKeysHeld [1]++;
			break;
		case FIREKEY:
			gc.playerKeysHeld [2]++;
			break;
		case WATERKEY:
			gc.playerKeysHeld [3]++;
			break;
		case WINDKEY:
			gc.playerKeysHeld [4]++;
			break;
		case SWORDUPGRADE:
			GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<PlayerSword> ().UpgradeToNextLevel ();
			break;
		case DASHUPGRADE:
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerCombat> ().numDashes++;
			break;
		case HEALTHUPGRADE:
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ().UpgradeHealth ();
			break;
		}
	}
}
