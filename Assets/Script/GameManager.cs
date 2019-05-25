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

    public bool cheatsON = false;

    public bool inForest = true;
    public bool inDesert = true;
    public bool inIce = true;


    public bool imReallyDead = true;
    public bool canAttack = true;
    public bool gameBeat = false;
    public string sceneName;
    public CharStats[] playerStats;//work on activate playerStats if the number of element!!!!

    public BattleItem[] totalItems;

    public int numberOfElement = 0;
    //public int activePartyMemberIndex = 1;

    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, battleActive,gameOver,noMenu, mainMenu;

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
        sceneName = SceneManager.GetActiveScene().name;
        enemyTracker = FindObjectOfType<EnemyTracker>();
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || battleActive|| gameOver||mainMenu)
        {
            noMenu = true;
            PlayerController.instance.canMovePlayer = false;
            PlayerController.instance.PlyerIdle(false);
            enemyTracker.EnemyMovment(false);
            GameMenu.instance.healthHolder.SetActive(false);
        }
        else
        {
            noMenu = false;
            PlayerController.instance.canMovePlayer = true;
            PlayerController.instance.PlyerIdle(true);
            enemyTracker.EnemyMovment(true);
            GameMenu.instance.healthHolder.SetActive(true);
        }

        if ((PlayerController.instance.imAlive==false)&&(imReallyDead==true))
        {
            StartCoroutine(BattleManager.instance.GameOverCo());
            imReallyDead = false;
        }

        if (cheatsON==true)
        {
            if (Input.GetKeyDown(KeyCode.T))//for test
            {
                //addElement();
                ElementGet();
            }
        }

       
        if ((sceneName == "OldManHouse")||(sceneName == "DeltaForestKnight") ||(sceneName == "ChronoDesertKnight") ||(sceneName == "IceAgeKnight") ||(sceneName == "DarkLand"))
        {
            canAttack = false;
            GameMenu.instance.healthHolder.SetActive(false);
        }
        else if (((sceneName == "MB_MapForBattle")||(sceneName=="DeltaForest"))&&(battleActive==false)&&(inForest==true))//later for forest battles
        {
            canAttack = true;
            GameMenu.instance.healthHolder.SetActive(true);
            GameMenu.instance.heartContainers.heartContainers.initialValue = 3;
            PlayerController.instance.currentHealth.initialValue = 6;
            GameMenu.instance.heartContainers.InitHearts();
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.fireSlider.maxValue;
            inForest = false;
        }
        else if (((sceneName == "MB_MapForBattle") || (sceneName == "ChronoDesert")) && (battleActive == false) && (inDesert == true))//later for forest battles
        {
            canAttack = true;
            GameMenu.instance.healthHolder.SetActive(true);
            GameMenu.instance.heartContainers.heartContainers.initialValue = 4;
            PlayerController.instance.currentHealth.initialValue = 8;
            GameMenu.instance.heartContainers.InitHearts();
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.playerInventory.currentMagic;
            inDesert = false;
        }
        else if (((sceneName == "MB_MapForBattle") || (sceneName == "IceAge")) && (battleActive == false) && (inIce == true))//later for forest battles
        {
            canAttack = true;
            GameMenu.instance.healthHolder.SetActive(true);
            GameMenu.instance.heartContainers.heartContainers.initialValue = 5;
            PlayerController.instance.currentHealth.initialValue = 10;
            GameMenu.instance.heartContainers.InitHearts();
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.playerInventory.currentMagic;
            inDesert = false;
        }

    }

   /* public void HeartLoad()
    {
        if (sceneName == "MB_MapForBattle")//later for forest battles
        {
            //Debug.Log(GameMenu.instance.healthHolder.active);
            //GameMenu.instance.heartContainers.heartContainers.initialValue = 2;
            //PlayerController.instance.currentHealth.initialValue = 4;
            GameMenu.instance.heartContainers.InitHearts();

        }
        else if (sceneName == "MB_SceneMoveTest")//later for other battles
        {
           // GameMenu.instance.heartContainers.heartContainers.initialValue = 4;
            //PlayerController.instance.currentHealth.initialValue = 8;
            GameMenu.instance.heartContainers.InitHearts();
        }
    }*/


    public void ElementGet()//activate when we defeate a mapBoss
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
