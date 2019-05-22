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

    public string mainMenuScene;
    public string loadGameScene;
    public GameObject[] Pages;
    public TextMeshProUGUI headLine;
    //0->Button
    //1->Load

    public Button[] loadButtons;



    // Use this for initialization
    void Start()
    {
        PlayerController.instance.mySprite.enabled = true;
        //GameMenu.instance.healthHolder.SetActive(false);
        PlayerController.instance.transform.position = new Vector2(0,0);
        GameManager.instance.gameOver = true;
        headLine.alpha = 0;
        StartCoroutine(FadeIN(0, 1, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        //GameManager.instance.gameOver = true;
    }


    IEnumerator FadeIN(float oldValue, float newValue, float duration)
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
        ShowPageButton(1);
        //yield return new WaitForSeconds(.3f);
    }



    public void ShowPageButton(int onOff)
    {
        if (onOff == 0)
        {
            Pages[0].SetActive(false);//show the page we want
            Pages[1].SetActive(true);//show the page we want
            LoadButtonsEnable();
        }
        else if (onOff == 1)
        {
            Pages[0].SetActive(true);//show the page we want
            Pages[1].SetActive(false);//show the page we want
        }
        else if (onOff == 2)
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

    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }


    public void QuitToMain()//need to add!!
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }


    public void QuitGame()//need to add!!
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave(int slot)
    {
        ShowPageButton(2);
        GameMenu.instance.PrepareLoadData(slot);
    }

}