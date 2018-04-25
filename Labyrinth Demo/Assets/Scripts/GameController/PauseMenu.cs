using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	//references
    public GameObject pauseMenuCanvas;
    public Text numOfFireKeys;
    public Text numOfBaseKeys;
    public Text numOfEarthKeys;
    public Text numOfWaterKeys;
    public Text numOfWindKeys;
    public GameObject woodSword;
    public GameObject steelSword;
    public GameObject ironSword;
	public GameObject[] maps;

	//variables
	public bool isPaused;
	private GameObject GC;

    // Use this for initialization
	void Start () {
		GC = GameObject.FindGameObjectWithTag("GameController");
		foreach (GameObject map in maps) {
			map.SetActive (false);
		}
	}

    // Update is called once per frame
    
    void Update () {
		if(isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            if (GC != null)
            {
				//INVENTORY
                int numBase = GC.GetComponent<GameController>().playerKeysHeld[0];
                int numFire = GC.GetComponent<GameController>().playerKeysHeld[2];
                int numEarth = GC.GetComponent<GameController>().playerKeysHeld[1];
                int numWater = GC.GetComponent<GameController>().playerKeysHeld[3];
                int numWin = GC.GetComponent<GameController>().playerKeysHeld[4];
                //Debug.Log("NumOfBaseKeys");
                //Debug.Log(numBase);
                numOfFireKeys.text = numFire.ToString() + "x ";
                numOfBaseKeys.text = numBase.ToString() + "x ";
                numOfEarthKeys.text = numEarth.ToString() + "x ";
                numOfWaterKeys.text = numWater.ToString() + "x ";
                numOfWindKeys.text = numWin.ToString() + "x ";
                int sword = GC.GetComponent<GameController>().swordUpgrade;
                if(sword == 0)
                {
                    woodSword.SetActive(true);
                    steelSword.SetActive(false);
                    ironSword.SetActive(false);
                } else if (sword == 1)
                {
                    ironSword.SetActive(true);
                    woodSword.SetActive(false);
                    steelSword.SetActive(false);
                } else if (sword == 2)
                {
                    steelSword.SetActive(true);
                    woodSword.SetActive(false);
                    ironSword.SetActive(false);
                } else
                {
                    Debug.Log("Could not find sword");
                }
            }
            else
            {
                Debug.Log("Couldn't Find GameController");
            }
        } else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

			if (isPaused) {
				//MAP
				foreach (GameObject map in maps) {
					MapDisplay md = map.GetComponent<MapDisplay> ();
					map.SetActive (false);
					if (GC != null) {
						if (md.mapLayer == GC.GetComponent<GameController> ().currLayer) {
							map.SetActive (true);
							md.UpdateMap ();
						}
					}
				}
			}
        }
	}
    public void Resume()
    {
        isPaused = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
		GetComponentInParent<GameController>().toBeDestroyed = true;
        SceneManager.LoadScene("Entrance");												//This is where we'll respawn upon death
		Destroy (GetComponentInParent<GameController>().gameObject);					//Destroy the old GameController
    }
    public void QuitToMainMenu()
    {
        isPaused = false;
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        GC.GetComponent<GameController>().toBeDestroyed = true;
        Destroy(GC);
        SceneManager.LoadScene("Main Menu");
    }
    public void getKeys()
    {

    }
}
