using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMenu : MonoBehaviour
{
    public Canvas theMenuCanves;
    public GameObject theMenu;
    public TextMeshProUGUI currentSceneName;
    public Button firstButton;
    public GameObject[] pages;
    public static GameMenu instance;
    public GameObject[] Elements;
    public GameObject[] partyImages;
    public TextMeshProUGUI[] currentPotionAmountText;
    public Button[] loadButtons;

    public GameObject dialogBox;
    public Text dialogBoxLine1;
    public Text dialogBoxLine2;
    public Text dialogBoxLine3;
    public GameObject healthHolder;
    public HeartManager heartContainers;
    public FireBarManager sliderHolder;

    private void Awake()
    {
        theMenuCanves.worldCamera = Camera.main;//get the main camera and use it
    }
    // Use this for initialization
    void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        theMenuCanves.worldCamera = Camera.main;//get the main camera and use it
    }
	
	// Update is called once per frame
	void Update ()
    {
        theMenuCanves.worldCamera = Camera.main;//get the main camera and use it
       // if (Input.GetKeyDown(KeyCode.Escape))//if the button is pressed
        if ((Input.GetKeyDown(KeyCode.Escape))&&(GameManager.instance.gameOver==false) && (GameManager.instance.battleActive == false) && (GameManager.instance.fadingBetweenAreas==false) && (GameManager.instance.dialogActive == false) && (GameManager.instance.mainMenu == false))//if the button is pressed and GameOver is false or battleActive is false (if we are in gameOver or loading from GameOver or in battle) then dont activate menu
        {
            if (theMenu.activeInHierarchy)//if the menu is open
            {
                CloseMenu();//close it
            }
            else//if its not
            {
                theMenu.SetActive(true);//open it
                pages[0].SetActive(true);//the info page open with the menu
                currentSceneName.text = SceneManager.GetActiveScene().name;//show the name of the map in the menu
                firstButton.Select();//select the first button for keyboard navigation
                UpdateStatsPage();//update the status
                GameManager.instance.gameMenuOpen = true;//tell GameManager that the menu is opened so that we stop movement 
            }
            AudioManager.instance.PlaySFX(5);//play menu sound 
        }
    }

    public void UpdateStatsPage()//a method for updating the info page
    {
        int numberOfElement = GameManager.instance.numberOfElement;//get the number of element we have acquired
        for (int i = 0; i < numberOfElement; i++)
        {
            if (Elements[i].gameObject.activeInHierarchy==false)//if its false
            {
                Elements[i].SetActive(true);//show the element
                partyImages[i].SetActive(true);//show the img of the players
            }
        }
        currentPotionAmountText[0].text = "HP potion: " + GameManager.instance.totalItems[0].ItemAmount;//show the hp potion amount
        currentPotionAmountText[1].text = "MP potion: " + GameManager.instance.totalItems[1].ItemAmount;//show the mp potion amount
        currentPotionAmountText[2].text = "SP potion: " + GameManager.instance.totalItems[2].ItemAmount;//show the sp potion amount
       // currentSceneName.text = SceneManager.GetActiveScene().name;//show the name of the map in the menu
      
    }

    public void ToggleWindow(int windowNumber)//a method to show the right page
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == windowNumber)
            {
                    pages[i].SetActive(true);//show the page we want
            }
            else
            {
                    pages[i].SetActive(false);//close all the rest
            }
        }
    }

    public void LoadButtonsEnable()
    {
        for (int i = 0; i < loadButtons.Length; i++)
        {
            string path = Application.persistentDataPath + "/" + (i+1) + "Save.MBAG";
            if (File.Exists(path))
            {
                loadButtons[i].interactable = true;
            }
            else
            {
                loadButtons[i].interactable = false;
            }
        }
    }

    public void QuitToMain()//need to add!!
    {

        CloseMenu();//close it
        StartCoroutine(SceneSwitch());
    }


    public IEnumerator SceneSwitch()
    {
        FadeManager.instance.ScenenTransition("ScenensFade");
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        PlayerController.instance.mySprite.enabled = true;
        GameManager.instance.mainMenu = false;
        SceneManager.LoadScene("MainMenu");
        GameManager.instance.fadingBetweenAreas = false;
        GameManager.instance.gameOver = false;

    }


    public void CloseMenu()//a method to close the menu and all the pages
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);//close all the pages
        }

        theMenu.SetActive(false);//close the menu
        GameManager.instance.gameMenuOpen = false;//tell GameManager that the menu is closed so that we start movement 
    }

    public void PlayButtonSound()//a method to play button sound
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void SaveData(int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + slot + "Save.MBAG";

        FileStream stream = new FileStream(path, FileMode.Create);
        GameData save = new GameData();
        save.SceneName = SceneManager.GetActiveScene().name;
        save.NumOfElement = GameManager.instance.numberOfElement;
        save.EnemyOnMap = GameManager.instance.enemyTracker.enemyOnMap;
        save.ItemsAmount = new int[3] { GameManager.instance.totalItems[0].ItemAmount, GameManager.instance.totalItems[1].ItemAmount, GameManager.instance.totalItems[2].ItemAmount };
        save.PlayerPos = new float[3] { PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, PlayerController.instance.transform.position.z };
        save.PlayersHP = new int[4] { GameManager.instance.playerStats[0].maxHP, GameManager.instance.playerStats[1].maxHP, GameManager.instance.playerStats[2].maxHP, GameManager.instance.playerStats[3].maxHP};
        save.PlayersMP = new int[4] { GameManager.instance.playerStats[0].maxMP, GameManager.instance.playerStats[1].maxMP, GameManager.instance.playerStats[2].maxMP, GameManager.instance.playerStats[3].maxMP };
        save.PlayersSP = new int[4] { GameManager.instance.playerStats[0].maxSP, GameManager.instance.playerStats[1].maxSP, GameManager.instance.playerStats[2].maxSP, GameManager.instance.playerStats[3].maxSP };
        save.PlayersLevel=new float[4] { GameManager.instance.playerStats[0].playerLevel, GameManager.instance.playerStats[1].playerLevel, GameManager.instance.playerStats[2].playerLevel, GameManager.instance.playerStats[3].playerLevel };
        save.ElementGot = GameManager.instance.gotElement;
        save.BossOnMap = GameManager.instance.enemyTracker.bossOnMap;
        save.HeartContainers = heartContainers.heartContainers.initialValue;
        save.BossSpeed = GameManager.instance.enemyTracker.theBoss.GetComponent<GralandChase>().moveSpeed;
        save.BossActive = GameManager.instance.enemyTracker.theBoss.GetComponent<GralandChase>().gameObject.active;
        //       save.CurrentHealthInitialValue = PlayerController.instance.currentHealth.initialValue;
        save.CurrentHealthRuntimeValue = PlayerController.instance.currentHealth.RuntimeValue;
        save.MaxMagic = sliderHolder.playerInventory.maxMagic;
        save.CurrentMagic = sliderHolder.playerInventory.currentMagic;

        save.CurrentHearts = new int[5];
        for (int i = 0; i < 5; i++)
        {
            if (heartContainers.hearts[i].sprite== heartContainers.fullHeart)
            {
                save.CurrentHearts[i] = 1;
            }
            else if (heartContainers.hearts[i].sprite == heartContainers.halfFullHeart)
            {
                save.CurrentHearts[i] = 2;
            }
            else if (heartContainers.hearts[i].sprite == heartContainers.emptyHeart)
            {
                save.CurrentHearts[i] = 3;
            }
        }
        if (GameManager.instance.enemyTracker.enemyOnMap)
        {
            if(GameManager.instance.enemyTracker.bossOnMap)
            {
                save.BossPos = new float[3] { GameManager.instance.enemyTracker.theBoss.transform.position.x, GameManager.instance.enemyTracker.theBoss.transform.position.y, GameManager.instance.enemyTracker.theBoss.transform.position.z };

            }
            save.NumOfEnemies = GameManager.instance.enemyTracker.theEnemies.Length;
            bool[] deadOrAliveEnemy = new bool[GameManager.instance.enemyTracker.theEnemies.Length];
            float[,] enemiesPos = new float[GameManager.instance.enemyTracker.theEnemies.Length, 3];
            for (int i = 0; i < GameManager.instance.enemyTracker.theEnemies.Length; i++)
            {
                deadOrAliveEnemy[i] = GameManager.instance.enemyTracker.theEnemies[i].activeInHierarchy;//if the enemy is alive!!
                enemiesPos[i, 0] = GameManager.instance.enemyTracker.theEnemies[i].transform.position.x;
                enemiesPos[i, 1] = GameManager.instance.enemyTracker.theEnemies[i].transform.position.y;
                enemiesPos[i, 2] = GameManager.instance.enemyTracker.theEnemies[i].transform.position.z;
                
            }
            save.EnemiesPos = enemiesPos;
            save.DeadOrAliveEnemy = deadOrAliveEnemy;
        }
        formatter.Serialize(stream, save);
        stream.Close();
        CloseMenu();
    }

    public GameData LoadDataFile(int slot)
    {
        string path = Application.persistentDataPath + "/" + slot + "Save.MBAG";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData load = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return load;
        }
        else//shuld not be activate in gameMenu if file doesn't exists
        {
            Debug.Log("No file");
            return null;
        }
    }


    public IEnumerator SceneLoad(GameData data)
    {
        GameManager.instance.fadingBetweenAreas = true;
        FadeManager.instance.ScenenTransition("Load");
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);

        PlayerController.instance.mySprite.enabled = true;


        AudioManager.instance.StopMusic();
        SceneManager.LoadScene(data.SceneName);
        yield return new WaitForSecondsRealtime(1);
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
        LoadData(data);
        yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);
        GameManager.instance.gameOver = false;//after loading back from menu or feom gameOver wait until Transition finished and then can walk/open menu
        GameManager.instance.fadingBetweenAreas = false;
    }
    public void LoadData(GameData data)
    {
        GameManager.instance.numberOfElement = data.NumOfElement;    
        GameManager.instance.totalItems[0].ItemAmount = data.ItemsAmount[0];
        GameManager.instance.totalItems[1].ItemAmount = data.ItemsAmount[1];
        GameManager.instance.totalItems[2].ItemAmount = data.ItemsAmount[2];
        heartContainers.heartContainers.initialValue = data.HeartContainers;

        healthHolder.SetActive(true);
        PlayerController.instance.MyRigidbody.constraints = RigidbodyConstraints2D.None;
        PlayerController.instance.MyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        PlayerController.instance.imAlive = true;
        GameManager.instance.imReallyDead = true;
        GameManager.instance.enemyTracker.theBoss.GetComponent<GralandChase>().moveSpeed = data.BossSpeed;
        GameManager.instance.enemyTracker.theBoss.GetComponent<GralandChase>().gameObject.SetActive(data.BossActive);

        //        PlayerController.instance.currentHealth.initialValue = data.CurrentHealthInitialValue;
        PlayerController.instance.currentHealth.RuntimeValue = data.CurrentHealthRuntimeValue;
        sliderHolder.fireSlider.value = data.CurrentMagic;
        sliderHolder.playerInventory.maxMagic = data.MaxMagic;
        sliderHolder.playerInventory.currentMagic = data.CurrentMagic;
        for (int i = 0; i < 5; i++)
        {
            if (data.CurrentHearts[i]==1)
            {
                heartContainers.hearts[i].sprite = heartContainers.fullHeart;
            }
            else if (data.CurrentHearts[i] == 2)
            {
                heartContainers.hearts[i].sprite = heartContainers.halfFullHeart;
            }
            else if (data.CurrentHearts[i] == 3)
            {
                heartContainers.hearts[i].sprite = heartContainers.emptyHeart;
            }
        }

        for (int i = 0; i <4; i++)
        {
            GameManager.instance.playerStats[i].maxHP = data.PlayersHP[i];
            GameManager.instance.playerStats[i].maxMP = data.PlayersMP[i];
            GameManager.instance.playerStats[i].maxSP = data.PlayersSP[i];
            GameManager.instance.playerStats[i].currentHP = GameManager.instance.playerStats[i].maxHP;
            GameManager.instance.playerStats[i].currentMP = GameManager.instance.playerStats[i].maxMP;
            GameManager.instance.playerStats[i].currentSP = GameManager.instance.playerStats[i].maxSP;
            GameManager.instance.playerStats[i].playerLevel = data.PlayersLevel[i];
        }
        for (int i = 0; i < Elements.Length; i++)
        {
            if (i<= data.NumOfElement-1)//its -1 because Elements its from 0 to 2..its start from ice
            {
                Elements[i].SetActive(true);//show the element
                partyImages[i].SetActive(true);//show the img of the players
            }
            else
            {
                Elements[i].SetActive(false);//dont show the element
                partyImages[i].SetActive(false);//dont show the img of the players
            }
        }
        GameManager.instance.gotElement = data.ElementGot;
        PlayerController.instance.transform.position = new Vector3(data.PlayerPos[0], data.PlayerPos[1], data.PlayerPos[2]);
        GameManager.instance.addElement();//no need to call ElementGet only addElement
        if (data.EnemyOnMap)
        {
            if (data.BossOnMap)
            {
                GameManager.instance.enemyTracker.theBoss.transform.position = new Vector3(data.BossPos[0], data.BossPos[1], data.BossPos[2]);
            }
            for (int i = 0; i < GameManager.instance.enemyTracker.theEnemies.Length; i++)
            {
                GameManager.instance.enemyTracker.theEnemies[i].SetActive(data.DeadOrAliveEnemy[i]);
                GameManager.instance.enemyTracker.theEnemies[i].transform.position = new Vector3(data.EnemiesPos[i, 0], data.EnemiesPos[i, 1], data.EnemiesPos[i, 2]);
            }
        }
        
    }
    public void PrepareLoadData(int slot)
    {

        GameData data = LoadDataFile(slot);
        StartCoroutine(SceneLoad(data));
        CloseMenu();
    }






}
