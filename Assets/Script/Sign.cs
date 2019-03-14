using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour {

    public GameObject dailogBox; //reference for the dialog box
    public Text dialogText; //reference for the text
    public string dialog; //reference to the string that shows up in place og the dialog
    public bool dialogActive; //for activating the dialog

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other) //check if the player entered the sign area 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other) //check if the player exited the sign area 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left range");
        }
    }
}
