using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;

    public GameObject continueButton;

    public string loadGameScene;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Continue()
    {
        StartCoroutine(SceneSwitch());
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void Exit()
    {
        Application.Quit();
    }


    public IEnumerator SceneSwitch()
    {
        FadeManager.instance.ScenenTransition("ScenensFade");
        //GameManager.instance.fadingBetweenAreas = true;
        // yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        SceneManager.LoadScene("OldManHouse");
        GameManager.instance.fadingBetweenAreas = false;
    }
}
