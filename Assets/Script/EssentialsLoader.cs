using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EssentialsLoader : MonoBehaviour
{

    public GameObject FadeMan;
    public GameObject player;
    public GameObject gameMan;
    public GameObject audioMan;
    public GameObject battleMan;
    //public EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();

    // Use this for initialization
    void Start()
    {
        if (FadeManager.instance == null)
        {
            FadeManager.instance = Instantiate(FadeMan).GetComponent<FadeManager>();
        }

        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
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

        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null)
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
