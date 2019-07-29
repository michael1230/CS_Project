using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    public GameObject[] Pages;//the pages for the game over menu
    //0->Button page
    //1->Load page
    public TextMeshProUGUI headLine;//the headline of the scene
    public Button[] loadButtons;//the buttons

    // Use this for initialization
    void Start()//reset values 
    {
        PlayerController.instance.currentState = PlayerState.idle;
        PlayerController.instance.mySprite.enabled = true;
        headLine.gameObject.SetActive(true);
        PlayerController.instance.transform.position = new Vector2(0,0);
        GameManager.instance.gameOver = true;
        headLine.alpha = 0;
        StartCoroutine(FadeIN(0, 1, 5f));
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator FadeIN(float oldValue, float newValue, float duration)//make the headline fade in
    {
        float value = 0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)//for loop to Fade in
        {
            value = Mathf.Lerp(oldValue, newValue, t / duration);
            headLine.alpha = value;
            yield return null;
        }
        value = newValue;
        headLine.alpha = value;
        ShowPageButton(1);//now after the headline is here show the buttons 
    }
    public void ShowPageButton(int onOff)
    {
        if (onOff == 0)//show the load buttons
        {
            Pages[0].SetActive(false);//show the page we want
            Pages[1].SetActive(true);//show the page we want
            LoadButtonsEnable();
        }
        else if (onOff == 1)//show the menu buttons
        {
            Pages[0].SetActive(true);//show the page we want
            Pages[1].SetActive(false);//show the page we want
        }
        else if (onOff == 2)//close both of the menus
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
    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }
    public void QuitToMain()//a method to quit to main menu
    {

        ShowPageButton(2);
        headLine.gameObject.SetActive(false);
        StartCoroutine(SceneSwitch());
    }
    public void QuitGame()//a method to quit the game
    {
        ShowPageButton(2);
        headLine.gameObject.SetActive(false);
        Application.Quit();       
    }
    public void LoadLastSave(int slot)//a method to load the save
    {
        ShowPageButton(2);
        if(slot==0)//if we want to load the latest save thin its 0
        {
            for (int i = 0; i < loadButtons.Length; i++)//on the first interactable button we load it
            {
                if(loadButtons[i].interactable == true)
                {
                    slot = i;
                    break;
                }
            }
        }
        GameMenu.instance.PrepareLoadData(slot);
    }
    public IEnumerator SceneSwitch()//a Coroutine to go back to main menu
    {
        FadeManager.instance.ScenenTransition("ScenensFade");//start the ScenenTransition with ScenensFade effect
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//wait until the screen is black
        PlayerController.instance.mySprite.enabled = true;//show the player sprite
        GameManager.instance.mainMenu = false;//reset
        SceneManager.LoadScene("MainMenu");//load the scene
        GameManager.instance.fadingBetweenAreas = false;//reset
        GameManager.instance.gameOver = false;//reset
    }
}