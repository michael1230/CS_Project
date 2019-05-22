using System.Collections;
using System.Collections.Generic;
using System.IO;//
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;//
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EnemyTracker enemyTracker;
    public int[] bonusSecondElement;
    public int[] bonusThirdElement;
    public int[] bonusFourthElement;
    public bool[] gotElement;
    public GameObject mapCanves;

    public CharStats[] playerStats;//work on activate playerStats if the number of element!!!!

    public BattleItem[] totalItems;

    public int numberOfElement = 0;
    //public int activePartyMemberIndex = 1;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, battleActive,gameOver;

    // Use this for initialization
    private void Awake()
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();

    }
    void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        gotElement = new bool[3];

        enemyTracker=FindObjectOfType<EnemyTracker>();

         playerStats[1].gameObject.SetActive(false);//for test is comma when game is ready delete comme and keep the lines!!!!
         playerStats[2].gameObject.SetActive(false);
         playerStats[3].gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || battleActive|| gameOver)
        {
            PlayerController.instance.canMovePlayer = false;
            enemyTracker.EnemyMovment(false);
        }
        else
        {
            PlayerController.instance.canMovePlayer = true;
            enemyTracker.EnemyMovment(true);
        }

        if (Input.GetKeyDown(KeyCode.T))//for test
        {
            //addElement();
            ElementGet(0);
        }
        if (Input.GetKeyDown(KeyCode.R))//for test
        {
            //addElement();
            ElementGet(1);
        }
        if (Input.GetKeyDown(KeyCode.E))//for test
        {
            //addElement();
            ElementGet(2);
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MB_MapForBattle")//later for forest battles
        {
            mapCanves.SetActive(false);
           // mapCanves.GetComponentInChildren<HeartManager>().playerCurrentHealth = new FloatValue(2);
           // PlayerController.instance.currentHealth =  new FloatValue(4);

        }
        else if (sceneName == "MB_SceneMoveTest")//later for other battles
        {
            mapCanves.SetActive(false);
           // mapCanves.GetComponentInChildren<HeartManager>().playerCurrentHealth = new FloatValue(4);
            //PlayerController.instance.currentHealth =  new FloatValue(8);
        }
        else
        {
            mapCanves.SetActive(false);
        }


    }

    public void ElementGet( int index)//activate when we defeate a mapBoss
    {
        //a bool for who was defeated 
       // gotElement[index] = true;
        numberOfElement++;//this for when we defeate a mapBoss..only after numberOfElement++ we call addElement
        //addElement(true);//->true if from game and false if from load
        addElement();
    }

    //public void addElement(bool gameOrLoad)
    public void addElement()
    {
        switch (numberOfElement)
        {
            case 0:
                for (int i = 0; i < playerStats.Length; i++)
                {
                    playerStats[i].movesAvailable = playerStats[i].movesSet1;
                    if (i<= numberOfElement)
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < playerStats.Length; i++)
                {
                    if(gotElement[0]==false)
                    {
                        playerStats[i].AddBonusElementStats(bonusSecondElement);
                        
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet2;
                    if (i <= numberOfElement)
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }                    
                }
                gotElement[0] = true;
                break;
            case 2:
                for (int i = 0; i < playerStats.Length; i++)
                {
                    if (gotElement[1] == false)
                    {
                        playerStats[i].AddBonusElementStats(bonusThirdElement);
                       
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet3;
                    if (i <= numberOfElement)
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }
                }
                gotElement[1] = true;
                break;
            case 3:
                for (int i = 0; i < playerStats.Length; i++)
                {
                    if (gotElement[2] == false)
                    {
                        playerStats[i].AddBonusElementStats(bonusFourthElement);
                        
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet4;
                    if (i <= numberOfElement)
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }
                }
                gotElement[2] = true;
                break;
            default:
                break;
        }
    }
}
