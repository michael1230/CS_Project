using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, battleActive;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || battleActive)
        {
            PlayerController.instance.canMovePlayer = false;
        }
        else
        {
            PlayerController.instance.canMovePlayer = true;
        }
    }
}
