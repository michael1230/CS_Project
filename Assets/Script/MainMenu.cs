using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject[] HeadLines;//the headline of this scene
    public GameObject[] Pages;//the [ages of this menu
    //0->startPage
    //1->load page
    public Button[] loadButtons;//the load data buttons

    // Use this for initialization
    void Start()//reset values
    {
        PlayerController.instance.mySprite.enabled = false;
        GameManager.instance.mainMenu = true;
        HeadLines[0].SetActive(true);
        HeadLines[1].SetActive(true);
        HeadLines[2].SetActive(true);
        ShowPageButton(0);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void NewGame()//a method for creating a new game
    {
        ShowPageButton(2);//turn off the pages
        HeadLines[0].SetActive(false);//turn off the headline
        HeadLines[1].SetActive(false);//turn off the headline
        HeadLines[2].SetActive(false);//turn off the headline
        StartCoroutine(SceneSwitch());//start the Coroutine
    }
    public void Exit()//a method to exit the game
    {
        ShowPageButton(2);//turn off the pages
        HeadLines[0].SetActive(false);//turn off the headline
        HeadLines[1].SetActive(false);//turn off the headline
        HeadLines[2].SetActive(false);//turn off the headline
        Application.Quit();
    }
    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }
    public IEnumerator SceneSwitch()//a method to go to the first map
    {
        FadeManager.instance.ScenenTransition("ScenensFade");//start the ScenenTransition with ScenensFade effect
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//wait until the screen is black
        PlayerController.instance.mySprite.enabled = true;//show the player sprite
        GameManager.instance.mainMenu = false;//reset
        SceneManager.LoadScene("OldManHouse");//load the scene
        PlayerController.instance.gameObject.transform.position = new Vector3(-1.18f, -1.49f, 0);//reset the player position
        GameManager.instance.fadingBetweenAreas = false;//reset
    }
    public void ShowPageButton(int onOff)//a method for showing the right pages
    {
        if (onOff == 0)//start page
        {
            Pages[0].SetActive(true);//show the page we want
            Pages[1].SetActive(false);//show the page we want

        }
        else if (onOff == 1)//load page
        {
            Pages[0].SetActive(false);//show the page we want
            Pages[1].SetActive(true);//show the page we want
            LoadButtonsEnable();
        }
        else if (onOff == 2)//none of them
        {
            Pages[0].SetActive(false);//show the page we want
            Pages[1].SetActive(false);//show the page we want
        }
    }
    public void LoadButtonsEnable()//a method to make only the button that the save exits in this system pressable 
    {
        for (int i = 0; i < loadButtons.Length; i++)
        {
            string path = Application.persistentDataPath + "/" + (i + 1) + "Save.MBAG";
            if (File.Exists(path))
            {
                loadButtons[i].interactable = true;
            }
            else
            {
                loadButtons[i].interactable = false;
            }
            if (i == 8)
            {
                loadButtons[i].interactable = true;
            }
        }
    }
    public void LoadLastSave(int slot)//a method to load the save
    {
        HeadLines[0].SetActive(false);//turn off the headline
        HeadLines[1].SetActive(false);//turn off the headline
        HeadLines[2].SetActive(false);//turn off the headline
        ShowPageButton(2);//turn off the pages
        GameManager.instance.mainMenu = false;//reset
        if (slot == 0)//if we want to load the latest save thin its 0
        {
            for (int i = 0; i < loadButtons.Length; i++)//on the first interactable button we load it
            {
                if (loadButtons[i].interactable == true)
                {
                    slot = i;
                    break;
                }
            }
        }
        GameMenu.instance.PrepareLoadData(slot);
    }
}
