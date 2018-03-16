using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	//references
    public GameObject gameOverCanvas;

	[HideInInspector] public PlayerHealth playerHealth;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
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
		GetComponentInParent<GameController>().toBeDestroyed = true;
		SceneManager.LoadScene("Entrance");												//This is where we'll respawn upon death
		Destroy (GetComponentInParent<GameController>().gameObject);					//Destroy the old GameController
    }
    public void Quit()
    {
        Application.Quit();
    }
}
