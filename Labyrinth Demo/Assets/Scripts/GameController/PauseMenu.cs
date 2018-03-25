using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public bool isPaused;
    public GameObject pauseMenuCanvas;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if(isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
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
}
