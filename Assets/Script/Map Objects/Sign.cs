using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour
{

    public GameObject dialogBox; //reference for the dialog box
    public Text dialogText; //reference for the text
    public Text dialogText2;
    public Text dialogText3;
    public string name;
    public string dialog; //reference to the string that shows up in place og the dialog
    public string dialog2;
    public bool playerInRange; //for activating the dialog
    public bool once;
    // Use this for initialization
    void Start ()
    {
        once = false;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (dialogBox==null)
        {
            dialogBox = GameMenu.instance.dialogBox;
            dialogText = GameMenu.instance.dialogBoxLine1;
            dialogText2 = GameMenu.instance.dialogBoxLine2;
            dialogText3 = GameMenu.instance.dialogBoxLine3;
        }
        if ((Input.GetKeyDown(KeyCode.Space) && playerInRange) && (GameManager.instance.sceneName == "OldManHouse")&&(GameManager.instance.gameMenuOpen==false)) //if player presses space near the sign then
        {
            if (dialogBox.activeInHierarchy) //if dialogBox is active change to false
            {
                dialogBox.SetActive(false);
                GameManager.instance.dialogActive = false;
            }
            else // else change to true
            {
                dialogBox.SetActive(true);
                dialogText.text = name;
                dialogText2.text = dialog;
                dialogText3.text = dialog2;
                GameManager.instance.dialogActive = true;
            }
        }
        else if ((playerInRange)&&(GameManager.instance.sceneName!= "OldManHouse"))
        {
            GameManager.instance.dialogActive = true;
            if (dialogBox.activeInHierarchy) //if dialogBox is active change to false
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    dialogBox.SetActive(false);
                    //GameManager.instance.dialogActive = true;
                    once = true;


                }
            }
            else // else change to true
            {
                if(once==false)
                {
                    dialogBox.SetActive(true);
                    dialogText.text = name;
                    dialogText2.text = dialog;
                    dialogText3.text = dialog2;
                    //GameManager.instance.dialogActive = true;
                }

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
