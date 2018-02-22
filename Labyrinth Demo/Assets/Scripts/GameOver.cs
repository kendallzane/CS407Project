using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public PlayerHealth playerHealth;
    public GameObject gameOverCanvas;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if(playerHealth.isDead)
        {
            gameOverCanvas.SetActive(true);
        } else
        {
            gameOverCanvas.SetActive(false);
        }
	}
    public void Restart()
    {
        //playerHealth.isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
