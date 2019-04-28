using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Canvas theMenuCanves;
    public GameObject theMenu;
    public TextMeshProUGUI currentSceneName;
    public Button firstButton;
    public GameObject[] pages;
    public static GameMenu instance;
    public GameObject[] Elements;
    public GameObject[] partyImages;
    public TextMeshProUGUI[] currentPotionAmountText;
    

    // Use this for initialization
    void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        theMenuCanves.worldCamera = Camera.main;//get the main camera and use it
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//if the button is pressed
        {
            if (theMenu.activeInHierarchy)//if the menu is open
            {
                CloseMenu();//close it
            }
            else//if its not
            {
                theMenu.SetActive(true);//open it
                pages[0].SetActive(true);//the info page open with the menu
                currentSceneName.text = SceneManager.GetActiveScene().name;//show the name of the map in the menu
                firstButton.Select();//select the first button for keyboard navigation
                UpdateStatsPage();//update the status
                GameManager.instance.gameMenuOpen = true;//tell GameManager that the menu is opened so that we stop movement 
            }
            AudioManager.instance.PlaySFX(5);//play menu sound 
        }
    }

    public void UpdateStatsPage()//a method for updating the info page
    {
        int numberOfElement = GameManager.instance.numberOfElement;//get the number of element we have acquired
        for (int i = 0; i < numberOfElement; i++)
        {
            if (Elements[i].gameObject.activeInHierarchy==false)//if its false
            {
                Elements[i].SetActive(true);//show the element
                partyImages[i].SetActive(true);//show the img of the players
            }
        }
        currentPotionAmountText[0].text = "HP potion: " + GameManager.instance.totalItems[0].ItemAmount;//show the hp potion amount
        currentPotionAmountText[1].text = "MP potion: " + GameManager.instance.totalItems[1].ItemAmount;//show the mp potion amount
        currentPotionAmountText[2].text = "SP potion: " + GameManager.instance.totalItems[2].ItemAmount;//show the sp potion amount
       // currentSceneName.text = SceneManager.GetActiveScene().name;//show the name of the map in the menu
      
    }

    public void ToggleWindow(int windowNumber)//a method to show the right page
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == windowNumber)
            {
                    pages[i].SetActive(true);//show the page we want
            }
            else
            {
                    pages[i].SetActive(false);//close all the rest
            }
        }
    }

    public void CloseMenu()//a method to close the menu and all the pages
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);//close all the pages
        }

        theMenu.SetActive(false);//close the menu
        GameManager.instance.gameMenuOpen = false;//tell GameManager that the menu is closed so that we start movement 
    }

    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }

}
