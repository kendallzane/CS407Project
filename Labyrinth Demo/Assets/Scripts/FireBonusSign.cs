using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBonusSign : MonoBehaviour {
    public GameObject FireBonusSignCanvas;
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
            FireBonusSignCanvas.SetActive(true);
        } else
        {
            Debug.Log("Not sensing Main Character");
            Debug.Log(coll.tag);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited");
        FireBonusSignCanvas.SetActive(false);
    }
}
