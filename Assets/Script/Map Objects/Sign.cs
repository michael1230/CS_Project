﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour
{

    public GameObject dialogBox;//reference for the dialog box
    public Text dialogText; //reference for the text1
    public Text dialogText2;//reference for the text2
    public Text dialogText3;//reference for the text3
    public string name;//reference to the string that shows up in place of the dialogText
    public string dialog;//reference to the string that shows up in place of the dialogText2
    public string dialog2;//reference to the string that shows up in place of the dialogText3
    public bool playerInRange;//for activating the dialog when player is in range
    public bool once;//activating the dialog only one time for per scene  
    
    // Use this for initialization
    void Start ()
    {
        once = false;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dialogBox==null)//if it doesn't exist then initialize from GameMenu
        {
            dialogBox = GameMenu.instance.dialogBox;
            dialogText = GameMenu.instance.dialogBoxLine1;
            dialogText2 = GameMenu.instance.dialogBoxLine2;
            dialogText3 = GameMenu.instance.dialogBoxLine3;
        }
        if ((Input.GetKeyDown(KeyCode.Space) && playerInRange) && (GameManager.instance.sceneName == "OldManHouse")&&(GameManager.instance.gameMenuOpen==false))//if player presses space near the sign then
        {
            if (dialogBox.activeInHierarchy)//if dialogBox is active change to false
            {
                dialogBox.SetActive(false);//set to false
                GameManager.instance.dialogActive = false;//update the GameManager dialog is off
            }
            else// else change to true
            {
                dialogBox.SetActive(true);//set to true
                dialogText.text = name;//place a variable in Unity Inspector
                dialogText2.text = dialog;//place a variable in Unity Inspector
                dialogText3.text = dialog2;//place a variable in Unity Inspector
                GameManager.instance.dialogActive = true;//update the GameManager dialog is on
            }
        }
        else if ((playerInRange)&&(GameManager.instance.sceneName!= "OldManHouse"))//if player in range of the sign and its not the OldManHouse then:
        {
            if (dialogBox.activeInHierarchy)//if dialogBox is active change to false
            {
                if (Input.GetKeyDown(KeyCode.Space))//only if space was pressed 
                {
                    dialogBox.SetActive(false);
                    once = true;
                }
            }
            else// else change to true
            {
                if(once==false)
                {
                    GameManager.instance.dialogActive = true;//update the GameManager dialog is on
                    dialogBox.SetActive(true);
                    dialogText.text = name;//place a variable in Unity Inspector
                    dialogText2.text = dialog;//place a variable in Unity Inspector
                    dialogText3.text = dialog2;//place a variable in Unity Inspector
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)//check if the player entered the sign area 
    {
        if (other.CompareTag("Player") && other.isTrigger)//if its the player that entered then true
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)//check if the player exited the sign area 
    {
        if (other.CompareTag("Player") && !other.isTrigger)//if the player got out of the trigger area 
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
