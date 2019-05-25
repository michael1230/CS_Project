using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{



    public GameObject[] HeadLines;
    public GameObject[] Pages;
    //0->startPage
    //1->load page
    public Button[] loadButtons;


    // Use this for initialization
    void Start()
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

    public void NewGame()
    {
        ShowPageButton(2);
        HeadLines[0].SetActive(false);
        HeadLines[1].SetActive(false);
        HeadLines[2].SetActive(false);
        StartCoroutine(SceneSwitch());
    }

    public void Exit()
    {
        ShowPageButton(2);
        HeadLines[0].SetActive(false);
        HeadLines[1].SetActive(false);
        HeadLines[2].SetActive(false);
        ///////
        Application.Quit();
    }


    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }


    public IEnumerator SceneSwitch()
    {
        FadeManager.instance.ScenenTransition("ScenensFade");
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        PlayerController.instance.mySprite.enabled = true;
        GameManager.instance.mainMenu = false;

        SceneManager.LoadScene("OldManHouse");
        PlayerController.instance.gameObject.transform.position = new Vector3(-1.18f, -1.49f, 0);
        GameManager.instance.fadingBetweenAreas = false;
    }

    public void ShowPageButton(int onOff)
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

    public void LoadButtonsEnable()
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


    public void LoadLastSave(int slot)
    {
        HeadLines[0].SetActive(false);
        HeadLines[1].SetActive(false);
        HeadLines[2].SetActive(false);
        ShowPageButton(2);
        //PlayerController.instance.mySprite.enabled = true;
        GameManager.instance.mainMenu = false;
        GameMenu.instance.PrepareLoadData(slot);
    }


}
