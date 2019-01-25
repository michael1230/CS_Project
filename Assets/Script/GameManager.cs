using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;//work on activate playerStats if the number of element!!!!

    public BattleItem[] totalItems;

    public int numberOfElement = 0;

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

        if (Input.GetKeyDown(KeyCode.T))//for test
        {
            if (numberOfElement < 3)
            {
                
                for (int i = 0; i < playerStats.Length; i++)
                {
                    playerStats[i].AddBonusElementStats(numberOfElement);
                    playerStats[i].hasElement[numberOfElement] = true;
                }
                numberOfElement++;
            }
        }


    }
}
