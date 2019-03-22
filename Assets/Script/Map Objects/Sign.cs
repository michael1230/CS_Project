using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour {

    public GameObject dialogBox; //reference for the dialog box
    public Text dialogText; //reference for the text
    public string dialog; //reference to the string that shows up in place og the dialog
    public bool playerInRange; //for activating the dialog

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange) //if player presses space near the sign then
        {
            if (dialogBox.activeInHierarchy) //if dialogBox is active change to false
            {
                dialogBox.SetActive(false);
            }
            else // else change to true
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //check if the player entered the sign area 
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //check if the player exited the sign area 
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
