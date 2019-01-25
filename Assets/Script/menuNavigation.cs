using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public GameObject[] theMenus;//all the battle menus
    public int currentMenu =0;//the current menu
    public int previousMenu =0;//the previous menu for knowing where to go back from target and self menu
    public Button[] menuButtons;//the buttons on the current menu

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < theMenus.Length; i++)//this for loop is for showing the main battle menu and turning off the rest
        {
            if (i == 0)
            {
                theMenus[i].SetActive(true);             
            }
            else
            {
                theMenus[i].SetActive(false);
            }
            this.buttonSelect();//select the first available button
        }
    }
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown(("Fire2")))//the back button
        {
            if (currentMenu == 0)//if we are in the main menu then update the previousMenu to 0
            {
                previousMenu = 0;
            }
            else//if we are not in main
            {
                this.backMenu();//go back
            }
        }
    }
    public void backMenu()//a method forgoing to the previous Menu
    {       
        if(currentMenu==5|| currentMenu==6)//if we are in the target or self menu
        {
            this.goToMenu(previousMenu, currentMenu);//go to previous
        }
        else//if we are in one of the others menu then the previous is main battle menu which is 0
        {
            this.goToMenu(0, currentMenu);//go to main battle menu
            BattleManager.instance.currentMenuText.text = "Main";//show that we are in main battle menu
        }
        this.buttonSelect();//select the first available button
    }
    public void goToMenu(int menuNext,int menuNow)//a method for goint into a menu
    {
        for (int i = 0; i < theMenus.Length; i++)//go on all the battle menu
        {
            if (i == menuNext)//if i is the menu index we want
            {
                theMenus[i].SetActive(true);//show it
            }
            else//if not
            {
                theMenus[i].SetActive(false);//turn it off
            }
            currentMenu = menuNext;//update the currentMenu
            previousMenu = menuNow;//update the previousMenu
        }
        this.buttonSelect();//select the first available button
    }

    public void buttonSelect()//a method for selecting the first available button
    {
        menuButtons = theMenus[currentMenu].GetComponentsInChildren<Button>();//get all the buttons in this menu
        bool alreadySelected = false;//a flag to know if we already Selected a button
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if ((menuButtons[i].interactable == true)&&(alreadySelected==false))//if the button is available to select and we haven't still selected a button
            {
                menuButtons[i].Select();//select it
                alreadySelected = true;//rise the flag
            }                    
        }
    }
}
