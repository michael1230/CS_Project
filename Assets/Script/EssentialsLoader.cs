using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject FadeMan;//the FadeManager object
    public GameObject player;//the PlayerController object
    public GameObject gameMan;//the GameManager object
    public GameObject audioMan;//the AudioManager object
    public GameObject battleMan;//the BattleManager object
    public GameObject menuMan;//the GameMenu object
    EventSystem sceneEventSystem;

    // Use this for initialization
    void Start()
    {//for all of the is bellow
     //if it object doesn't exits then create it
        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }
        if (FadeManager.instance == null)
        {
            FadeManager.instance = Instantiate(FadeMan).GetComponent<FadeManager>();
        }
        if (GameMenu.instance == null)
        {
            GameMenu.instance = Instantiate(menuMan).GetComponent<GameMenu>();
        }
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }

        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }

        if (BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }
        sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null)//the same but with EventSystem
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
