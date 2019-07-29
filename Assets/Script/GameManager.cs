using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;//the GameManager object itself
    public EnemyTracker enemyTracker;//the EnemyTracker object itself
    public int[] bonusSecondElement;//the bonus for the second Element
    public int[] bonusThirdElement;//the bonus for the third Element
    public int[] bonusFourthElement;//the bonus for the fourth Element
    public bool[] gotElement;//which element we have
    public bool cheatsON = false;//if the cheats are on
    public bool inForest = true;//if we are in the forset..only once!
    public bool inDesert = true;//if we are in the Desert..only once!
    public bool inIce = true;//if we are in the Ice..only once!
    public bool inDark = true;//if we are in the Dark..only once!
    public bool imReallyDead = true;////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool canAttack = true;//if we cant attack now
    public bool gameBeat = false;//if we have beat the final boss
    public string sceneName;//the name for this scene
    public CharStats[] playerStats;//the players stats
    public BattleItem[] totalItems;//the amount of items
    public int numberOfElement = 0;//the number of element
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, battleActive,gameOver, mainMenu;//a bool to know if we cant move or not
    public bool dontGainSpeed;//a bool to know if the boss can gain speed now

    // Use this for initialization
    private void Awake()
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();

    }
    void Start ()//reset value
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        gotElement = new bool[3];
        enemyTracker=FindObjectOfType<EnemyTracker>();
        playerStats[1].gameObject.SetActive(false);
        playerStats[2].gameObject.SetActive(false);
        playerStats[3].gameObject.SetActive(false);
    }	
	// Update is called once per frame
	void Update ()
    {
        sceneName = SceneManager.GetActiveScene().name;//get the name of this scene
        enemyTracker = FindObjectOfType<EnemyTracker>();//get enemyTracker object
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || battleActive|| gameOver||mainMenu)//if one pf them is true then
        {
            dontGainSpeed = true;//don't gain speed now
            PlayerController.instance.canMovePlayer = false;//player cant move
            PlayerController.instance.PlyerIdle(false);/////////////////////////////////////////////////////////////////////
            enemyTracker.EnemyMovment(false);//enemies and boss cant move
            GameMenu.instance.healthHolder.SetActive(false);////////////////////////////////////////////////////////////////
        }
        else//if all of them is false then
        {
            dontGainSpeed = false;//gain speed now
            PlayerController.instance.canMovePlayer = true;//player can move
            PlayerController.instance.PlyerIdle(true);/////////////////////////////////////////////////////////////////////
            enemyTracker.EnemyMovment(true);//enemies and boss can move
            GameMenu.instance.healthHolder.SetActive(true);/////////////////////////////////////////////////////////////////////
        }

        if ((PlayerController.instance.imAlive==false)&&(imReallyDead==true))//////////////////////////////////////////////////
        {
            StartCoroutine(BattleManager.instance.GameOverCo());
            imReallyDead = false;
        }

        if (cheatsON==true)//if the cheats are on then
        {
            if (Input.GetKeyDown(KeyCode.T))//for test
            {
                ElementGet();
            }
        }     
        if ((sceneName == "OldManHouse")||(sceneName == "DeltaForestKnight") ||(sceneName == "ChronoDesertKnight") ||(sceneName == "IceAgeKnight") ||(sceneName == "DarkLand"))//if we are in one of these scene
        {
            canAttack = false;//cant attack
            GameMenu.instance.healthHolder.SetActive(false);////////////////////////////////////////////////////////
        }
        else if (((sceneName == "MB_MapForBattle")||(sceneName=="DeltaForest"))&&(battleActive==false)&&(inForest==true))//if we are in DeltaForest (or MB_MapForBattle for tests) and 
        // battleActive is false(thats means there is not a battle now) and inForest is true(thats means we have not been in this if yet) 
        {
            canAttack = true;//can attack 
            GameMenu.instance.healthHolder.SetActive(true);////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.heartContainers.initialValue = 3;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.heartContainers.RuntimeValue = 3;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.initialValue = 6;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.RuntimeValue = 6;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.InitHearts();////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.fireSlider.maxValue;////////////////////////////////////////////////////////
            totalItems[0].ItemAmount = 10;//have 10 at the start of this map
            totalItems[1].ItemAmount = 10;//have 10 at the start of this map
            totalItems[2].ItemAmount = 10;//have 10 at the start of this map
            inForest = false;//so we wont get in this if again...only once!(because of the save and load)
        }
        else if ((sceneName == "ChronoDesert") && (battleActive == false) && (inDesert == true))//if we are in ChronoDesert and 
        // battleActive is false(thats means there is not a battle now) and inDesert is true(thats means we have not been in this if yet)
        {
            canAttack = true;//can attack 
            GameMenu.instance.healthHolder.SetActive(true);////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.heartContainers.initialValue = 4;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.heartContainers.RuntimeValue = 4;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.initialValue = 8;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.RuntimeValue = 8;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.InitHearts();////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.playerInventory.currentMagic;////////////////////////////////////////////////////////
            totalItems[0].ItemAmount = 13;//have 13 at the start of this map
            totalItems[1].ItemAmount = 13;//have 13 at the start of this map
            totalItems[2].ItemAmount = 13;//have 13 at the start of this map
            inDesert = false;//so we wont get in this if again...only once!(because of the save and load)
        }
        else if ((sceneName == "IceAge") && (battleActive == false) && (inIce == true))//if we are in IceAge and 
        // battleActive is false(thats means there is not a battle now) and inIce is true(thats means we have not been in this if yet)
        {
            canAttack = true;//can attack 
            GameMenu.instance.healthHolder.SetActive(true);
            GameMenu.instance.heartContainers.heartContainers.initialValue = 5;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.heartContainers.RuntimeValue = 5;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.initialValue = 10;////////////////////////////////////////////////////////
            PlayerController.instance.currentHealth.RuntimeValue = 10;////////////////////////////////////////////////////////
            GameMenu.instance.heartContainers.InitHearts();////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.playerInventory.currentMagic = GameMenu.instance.sliderHolder.playerInventory.maxMagic;////////////////////////////////////////////////////////
            GameMenu.instance.sliderHolder.fireSlider.value = GameMenu.instance.sliderHolder.playerInventory.currentMagic;////////////////////////////////////////////////////////
            totalItems[0].ItemAmount = 15;//have 15 at the start of this map
            totalItems[1].ItemAmount = 15;//have 15 at the start of this map
            totalItems[2].ItemAmount = 15;//have 15 at the start of this map
            inIce = false;//so we wont get in this if again...only once!(because of the save and load)
        }
        else if ((sceneName == "DarkLand") && (battleActive == false) && (inDark == true))//if we are in DarkLand and 
        // battleActive is false(thats means there is not a battle now) and inDark is true(thats means we have not been in this if yet)
        {
            totalItems[0].ItemAmount = 20;//have 20 at the start of this map
            totalItems[1].ItemAmount = 20;//have 20 at the start of this map
            totalItems[2].ItemAmount = 20;//have 20 at the start of this map
            inDark = false;//so we wont get in this if again...only once!(because of the save and load)
        }

    }
    public void ElementGet()//a method to activate when we defeat a mapBoss...(when we load data we only call addElement and thats why we have this helper method to call when a mapBoss is defeated) 
    {
        numberOfElement++;//this for when we defeat a mapBoss..only after numberOfElement++ we call addElement
        addElement();//call the addElement method
    }
    public void addElement()//a method to add an element with its moves and bonuses and party 
    {
        switch (numberOfElement)//swith case for numberOfElement
        {
            case 0://if numberOfElement is 0
                for (int i = 0; i < playerStats.Length; i++)//go on all the players we have
                {
                    playerStats[i].movesAvailable = playerStats[i].movesSet1;//change the moveSet of the  i player to moveSet1
                    if (i<= numberOfElement)//activate the first (numberOfElement) players 
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
                for (int i = 0; i < playerStats.Length; i++)//go on all the players we have
                {
                    if(gotElement[0]==false)//if we still didn't add the bonus
                    {
                        playerStats[i].AddBonusElementStats(bonusSecondElement);
                        
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet2;//change the moveSet of the  i player to moveSet2
                    if (i <= numberOfElement)//activate the first (numberOfElement) players 
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }                    
                }
                gotElement[0] = true;//to only add the bunus once(because of the save and load)
                break;
            case 2:
                for (int i = 0; i < playerStats.Length; i++)//go on all the players we have
                {
                    if (gotElement[1] == false)//if we still didn't add the bonus
                    {
                        playerStats[i].AddBonusElementStats(bonusThirdElement);
                       
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet3;//change the moveSet of the  i player to moveSet3
                    if (i <= numberOfElement)//activate the first (numberOfElement) players 
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }
                }
                gotElement[1] = true;//to only add the bunus once(because of the save and load)
                break;
            case 3:
                for (int i = 0; i < playerStats.Length; i++)//go on all the players we have
                {
                    if (gotElement[2] == false)//if we still didn't add the bonus
                    {
                        playerStats[i].AddBonusElementStats(bonusFourthElement);
                        
                    }
                    playerStats[i].movesAvailable = playerStats[i].movesSet4;//change the moveSet of the  i player to moveSet4
                    if (i <= numberOfElement)//activate the first (numberOfElement) players 
                    {
                        playerStats[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        playerStats[i].gameObject.SetActive(false);
                    }
                }
                gotElement[2] = true;//to only add the bunus once(because of the save and load)
                break;
            default:
                break;
        }
    }
}
