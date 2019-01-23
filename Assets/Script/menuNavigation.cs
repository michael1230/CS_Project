using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuNavigation : MonoBehaviour
{
    //public static menuNavigation instance;
    public GameObject[] theMenus;
    public int currentMenu =0;
    public int previousMenu =0;
    // Use this for initialization
    void Start()
    {
        //instance = this;
        //DontDestroyOnLoad(gameObject);
        for (int i = 0; i < theMenus.Length; i++)
        {
            if (i == 0)
            {
                theMenus[i].SetActive(true);
            }
            else
            {
                theMenus[i].SetActive(false);
            }
        }
    }
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown(("Fire2")))
        {
            if (currentMenu == 0)
            {
                previousMenu = 0;
            }
            else
            {
                this.backMenu();
            }
        }
    }
    public void backMenu()
    {       
        if(currentMenu==5|| currentMenu==6)
        {
            theMenus[previousMenu].SetActive(true);
            theMenus[currentMenu].SetActive(false);
            currentMenu = previousMenu;
            previousMenu = 0;
        }
        else
        {
            previousMenu = 0;
            theMenus[previousMenu].SetActive(true);
            theMenus[currentMenu].SetActive(false);
            currentMenu = 0;
            BattleManager.instance.currentMenuText.text = "Main";
        }
    }
    public void goToMenu(int menuNext,int menuNow)
    {
        for (int i = 0; i < theMenus.Length; i++)
        {
            if(i == menuNext)
            {
                theMenus[i].SetActive(true);
            }
            else
            {
                theMenus[i].SetActive(false);
            }
            currentMenu = menuNext;
            previousMenu = menuNow;
        }
    }
}
