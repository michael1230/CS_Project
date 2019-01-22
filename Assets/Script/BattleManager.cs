using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public GameObject[] playerInfoHolder;
    public GameObject battleMenuHolder;
    public GameObject battleMenuButtonsHolder;
    
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI currentMenuText;

    public DamageNumber theDamageNumber;

    public BattleMove[] movesList;
    public GameObject enemyAttackEffect;

    public Text[] playerHP;
    public Text[] playerMP;
    public Text[] playerSP;
    public Slider[] hpSlider;
    public Slider[] mpSlider;
    public Slider[] spSlider;
    public Image[] charImage;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public GameObject specialMenu;
    public BattleSpecialSelect[] specialButtons;

    public GameObject itemMenu;
    public BattleItemSelect[] itemButtons;

    public GameObject attackMenu;
    public BattleAttackSelect[] attackButtons;

    public GameObject selfMenu;
    public BattleTargetButton[] selfButtons;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;




    // Use this for initialization
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (battleActive)//if we are in a battle
        {
            if (turnWaiting)//if we are waiting for something
            {
                if (activeBattlers[currentTurn].isPlayer)//if the current Turn is of player
                {
                    battleMenuHolder.SetActive(true);//show the battle menu
                    currentPlayerText.text = activeBattlers[currentTurn].charName;//write the name of the current char
                    //currentMenuText.text = "Main";
                }
                else//if its not the player
                {
                    battleMenuHolder.SetActive(false);//turnoff the battle menu
                    StartCoroutine(EnemyMoveCo());//start the Coroutine for the enemy turn
                }
            }
            if (Input.GetKeyDown(KeyCode.N))///for test!!
            {
                NextTurn();
            }

            if (Input.GetKeyDown(KeyCode.K))///for test!!
            {
                activeBattlers[currentTurn].currentMP = activeBattlers[currentTurn].maxMP;
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)//a method for staring the battle(only runs once per battle)
    {
        if (!battleActive)//if the battleActive is false
        {
            battleActive = true;//make it true
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);//put the camera on the battle
            battleScene.SetActive(true);//show the battleScene
            GameManager.instance.battleActive = true;//rise the flag for GameManager
            AudioManager.instance.PlayBGM(8);//turn on the battle music
            for (int i = 0; i < playerPositions.Length; i++)//put all active players with theres stats 
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);//add the player to the list
                            CharStats thePlayer = GameManager.instance.playerStats[i];//for easy access
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].currentSP = thePlayer.currentSP;
                            activeBattlers[i].maxSP = thePlayer.maxSP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].dexterity = thePlayer.dexterity;

                        }
                    }
                }
            }
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")//if the enemy name is empty
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);//add the Enemy to the list
                        }
                    }
                }
            }
            turnWaiting = true;//rise the flag
            currentTurn = 0;//the first turn
            UpdateUIStats();//update the stats
            currentMenuText.text = "Main";//what is the current menu
        }
    }
    public void NextTurn()//a method for going to the next turn
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)//if it over the limit 
        {
            currentTurn = 0;//reset it
        }
        turnWaiting = true;//rise the flag
        currentMenuText.text = "Main";//what is the current menu
        UpdateBattle();////update the battle
        UpdateUIStats();//update the stats
    }
    public void UpdateBattle()//a method for handling dead player and enemies
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if (activeBattlers[i].currentHP == 0)
            {
                /*
                 * if (activeBattlers[i].isPlayer)
                    {
                    Animator anim;
                    anim = activeBattlers[i].GetComponent<Animator>();
                    anim.Play("Reg_Atk_2");
                    }
                */



                //Handle dead battler
                /*
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }*/

            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    // activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                // StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                // StartCoroutine(GameOverCo());
            }

            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;
        }
        else
        {
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }
    public IEnumerator EnemyMoveCo()//wait for enemy
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyAttack()// a method for enemy attack//// later for boss/////////////
    {
        List<int> players = new List<int>();//the list for all players
        for (int i = 0; i < activeBattlers.Count; i++)//add only alive players
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];//random select targets from the list
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);//random select an attack of the enemy
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])//if the enemy has the attack we selected
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);//make the effect appear on the target
                movePower = movesList[i].movePower;//save the move power
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);//white circle on the attacking enemy to know which one is attacking
        DealDamage(selectedTarget, movePower);//deal the damage
    }
    public void DealDamage(int target, int movePower)//later to add miss, critical, res , magic? 
    {
        float atkPwr = activeBattlers[currentTurn].strength;// + activeBattlers[currentTurn].wpnPower;
        float defPwr = activeBattlers[target].defense;//+ activeBattlers[target].armrPower;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);
       // Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);//for test
        activeBattlers[target].currentHP -= damageToGive;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);//make the damage appear on screen
        UpdateUIStats();//update the stats
    }
    public void UpdateUIStats()//a method for keeping the info up to date
    {
        for (int i = 0; i < playerInfoHolder.Length; i++)//a loop for the info
        {
            if (activeBattlers.Count > i)//for the next "if" we dont want activeBattlers[i] to not work because i is smaller then activeBattlers.Count
            {
                if (activeBattlers[i].isPlayer)//if its a player
                {
                    BattleChar playerData = activeBattlers[i];//for easy access
                    playerInfoHolder[i].gameObject.SetActive(true);//active the info
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;// Update the hp..the clmap is for not showing minus numbers
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;// Update the mp..the clmap is for not showing minus numbers
                    playerSP[i].text = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue) + "/" + playerData.maxSP;// Update the Sp..the clmap is for not showing minus numbers
                    hpSlider[i].maxValue = playerData.maxHP;
                    hpSlider[i].value = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue);
                    mpSlider[i].maxValue = playerData.maxMP;
                    mpSlider[i].value = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue);
                    spSlider[i].maxValue = playerData.maxSP;
                    spSlider[i].value = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue);
                }
                else//if its not a player
                {
                    playerInfoHolder[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerInfoHolder[i].gameObject.SetActive(false);
            }
        }
    }
    public void PlayerAttack(string moveName, int selectedTarget)//a method for player attack//////////////add animation //////////////
    {
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
        DealDamage(selectedTarget, movePower);//deal the damage
        battleMenuHolder.SetActive(false);//turn off the menu do prevent the player for pressing the buttons twice
        targetMenu.SetActive(false);//turn off the target menu
        battleMenuButtonsHolder.SetActive(true);//turn on the button on the main battle menu
        NextTurn();//next turn
    }
    public void OpenTargetMenu(string moveName)//a method for opening the target menu
    {
        targetMenu.SetActive(true);//open the target menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        List<int> Enemies = new List<int>();//list of enemies
        for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < targetButtons.Length; i++)//how many target buttons there are
        {
            Button firstButton;//the first button to select
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)//if the Enemies.Countis bigger then i(for no error in activeBattlers[Enemies[i]]) and the enemy hp is above 0(alive)
            {
                targetButtons[i].gameObject.SetActive(true);//turn on the button
                if (activeBattlers[Enemies[i]].currentHP > 0 && alreadySelected == false)//go in on the first enemy alive on the list and if we have yet to select the button 
                {
                    firstButton = targetButtons[i].gameObject.GetComponent<Button>();//get the button
                    firstButton.Select();//select it
                    alreadySelected = true;//rise the flag
                }
                targetButtons[i].moveName = moveName;//save the move name
                targetButtons[i].activeBattlerTarget = Enemies[i];//save the enemy target
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;//show its name
            }
            else//if the enemy is dead 
            {
                targetButtons[i].gameObject.SetActive(false);//turnoff the target
            }
        }
    }
    public void OpenSelfMenu(string moveName)//a method for opening the target menu
    {
        selfMenu.SetActive(true);//open the target menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        List<int> Enemies = new List<int>();//list of enemies
        for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < targetButtons.Length; i++)//how many target buttons there are
        {
            Button firstButton;//the first button to select
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)//if the Enemies.Countis bigger then i(for no error in activeBattlers[Enemies[i]]) and the enemy hp is above 0(alive)
            {
                targetButtons[i].gameObject.SetActive(true);//turn on the button
                if (activeBattlers[Enemies[i]].currentHP > 0 && alreadySelected == false)//go in on the first enemy alive on the list and if we have yet to select the button 
                {
                    firstButton = targetButtons[i].gameObject.GetComponent<Button>();//get the button
                    firstButton.Select();//select it
                    alreadySelected = true;//rise the flag
                }
                targetButtons[i].moveName = moveName;//save the move name
                targetButtons[i].activeBattlerTarget = Enemies[i];//save the enemy target
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;//show its name
            }
            else//if the enemy is dead 
            {
                targetButtons[i].gameObject.SetActive(false);//turnoff the target
            }
        }
    }
    public void OpenMagicMenu()//a method for opening the magic menu
    {
        magicMenu.SetActive(true);//open the magic menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        currentMenuText.text = "Magic";//show which menu we are at
        List<Button> activeMagicButon = new List<Button>();//a list for all the buttons in this menu
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)//go in if we still have moves to map
            {
                magicButtons[i].gameObject.SetActive(true);//turn on the button
                activeMagicButon.Add(magicButtons[i].gameObject.GetComponent<Button>());//add the button
               /* if (i==0)
                {
                    activeMagicButon[0].Select();
                } */             
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];//save the spell name
                magicButtons[i].nameText.text = magicButtons[i].spellName;//show the spell name 
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)//choose the right spell according to the name
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;//save the spell cost
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();//show the spell cost 
                    }
                }
                if(alreadySelected==false &&(activeBattlers[currentTurn].currentMP> magicButtons[i].spellCost))//go in on the first spell that the player can use on the list and if we have yet to select the button 
                {
                    magicButtons[i].gameObject.GetComponent<Button>().Select();
                    alreadySelected = true;
                }
            }
            else//if we are out of moves
            {               
                magicButtons[i].gameObject.SetActive(false);//turnoff the button
            }
        }
        for (int i = 0; i < activeMagicButon.Count; i++)//going on all the active buttons
        {
            //Debug.Log(activeMagicButon[i].GetComponent<BattleMagicSelect>().spellName);
            if(activeBattlers[currentTurn].currentMP<activeMagicButon[i].GetComponent<BattleMagicSelect>().spellCost)//check if the currnet player can use the move i
            {
                activeMagicButon[i].interactable = false; //if not then disable the button
            }
            else
            {
                activeMagicButon[i].interactable = true; //if not then disable the button
            }
        }
    }


/*
    public void OpenAttackMenu()//a method for opening the magic menu
    {
        attackMenu.SetActive(true);//open the attack menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        currentMenuText.text = "Attack";//show which menu we are at
        List<Button> activeAttackButon = new List<Button>();//a list for all the buttons in this menu
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < attackButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)//go in if we still have moves to map
            {
                attackButtons[i].gameObject.SetActive(true);//turn on the button
                activeAttackButon.Add(attackButtons[i].gameObject.GetComponent<Button>());//add the button
                attackButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];//save the spell name
                attackButtons[i].nameText.text = attackButtons[i].spellName;//show the spell name 
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == attackButtons[i].spellName)//choose the right spell according to the name
                    {
                        attackButtons[i].spellCost = movesList[j].moveCost;//save the spell cost
                        attackButtons[i].costText.text = attackButtons[i].spellCost.ToString();//show the spell cost 
                    }
                }
                if (alreadySelected == false && (activeBattlers[currentTurn].currentMP > attackButtons[i].spellCost))//go in on the first spell that the player can use on the list and if we have yet to select the button 
                {
                    attackButtons[i].gameObject.GetComponent<Button>().Select();
                    alreadySelected = true;
                }
            }
            else//if we are out of moves
            {
                attackButtons[i].gameObject.SetActive(false);//turnoff the button
            }
        }
        for (int i = 0; i < activeAttackButon.Count; i++)//going on all the active buttons
        {
            //Debug.Log(activeMagicButon[i].GetComponent<BattleMagicSelect>().spellName);
            if (activeBattlers[currentTurn].currentMP < activeAttackButon[i].GetComponent<BattleMagicSelect>().spellCost)//check if the currnet player can use the move i
            {
                activeAttackButon[i].interactable = false; //if not then disable the button
            }
            else
            {
                activeAttackButon[i].interactable = true; //if not then disable the button
            }
        }
    }
    */
    public void OpenSpecialMenu()//a method for opening the magic menu
    {
        magicMenu.SetActive(true);//open the magic menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        currentMenuText.text = "Magic";//show which menu we are at
        List<Button> activeMagicButon = new List<Button>();//a list for all the buttons in this menu
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)//go in if we still have moves to map
            {
                magicButtons[i].gameObject.SetActive(true);//turn on the button
                activeMagicButon.Add(magicButtons[i].gameObject.GetComponent<Button>());//add the button
                                                                                        /* if (i==0)
                                                                                         {
                                                                                             activeMagicButon[0].Select();
                                                                                         } */
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];//save the spell name
                magicButtons[i].nameText.text = magicButtons[i].spellName;//show the spell name 
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)//choose the right spell according to the name
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;//save the spell cost
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();//show the spell cost 
                    }
                }
                if (alreadySelected == false && (activeBattlers[currentTurn].currentMP > magicButtons[i].spellCost))//go in on the first spell that the player can use on the list and if we have yet to select the button 
                {
                    magicButtons[i].gameObject.GetComponent<Button>().Select();
                    alreadySelected = true;
                }
            }
            else//if we are out of moves
            {
                magicButtons[i].gameObject.SetActive(false);//turnoff the button
            }
        }
        for (int i = 0; i < activeMagicButon.Count; i++)//going on all the active buttons
        {
            //Debug.Log(activeMagicButon[i].GetComponent<BattleMagicSelect>().spellName);
            if (activeBattlers[currentTurn].currentMP < activeMagicButon[i].GetComponent<BattleMagicSelect>().spellCost)//check if the currnet player can use the move i
            {
                activeMagicButon[i].interactable = false; //if not then disable the button
            }
            else
            {
                activeMagicButon[i].interactable = true; //if not then disable the button
            }
        }
    }
    public void OpenItemMenu()//a method for opening the magic menu
    {
        magicMenu.SetActive(true);//open the magic menu
        battleMenuButtonsHolder.SetActive(false);//turnoff the buttons on the main battle menu because its interrupting the navigation for button
        currentMenuText.text = "Magic";//show which menu we are at
        List<Button> activeMagicButon = new List<Button>();//a list for all the buttons in this menu
        bool alreadySelected = false;//a flag for knowing if we have already selected the button
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)//go in if we still have moves to map
            {
                magicButtons[i].gameObject.SetActive(true);//turn on the button
                activeMagicButon.Add(magicButtons[i].gameObject.GetComponent<Button>());//add the button
                                                                                        /* if (i==0)
                                                                                         {
                                                                                             activeMagicButon[0].Select();
                                                                                         } */
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];//save the spell name
                magicButtons[i].nameText.text = magicButtons[i].spellName;//show the spell name 
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)//choose the right spell according to the name
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;//save the spell cost
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();//show the spell cost 
                    }
                }
                if (alreadySelected == false && (activeBattlers[currentTurn].currentMP > magicButtons[i].spellCost))//go in on the first spell that the player can use on the list and if we have yet to select the button 
                {
                    magicButtons[i].gameObject.GetComponent<Button>().Select();
                    alreadySelected = true;
                }
            }
            else//if we are out of moves
            {
                magicButtons[i].gameObject.SetActive(false);//turnoff the button
            }
        }
        for (int i = 0; i < activeMagicButon.Count; i++)//going on all the active buttons
        {
            //Debug.Log(activeMagicButon[i].GetComponent<BattleMagicSelect>().spellName);
            if (activeBattlers[currentTurn].currentMP < activeMagicButon[i].GetComponent<BattleMagicSelect>().spellCost)//check if the currnet player can use the move i
            {
                activeMagicButon[i].interactable = false; //if not then disable the button
            }
            else
            {
                activeMagicButon[i].interactable = true; //if not then disable the button
            }
        }
    }


}

