using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralSign : MonoBehaviour {
    public GameObject CentralSignCanvas;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Debug.Log("Hit Sign");
            CentralSignCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("Not sensing Main Character");
            Debug.Log(coll.tag);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited");
        CentralSignCanvas.SetActive(false);
    }
}
