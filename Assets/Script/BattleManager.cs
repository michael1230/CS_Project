using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;
    public GameObject battleCanves;

    //public GameObject battleImeg;
    public SpriteRenderer currenctBattleImeg;
    public Sprite[] battleImeges;
    //0 if forest
    //1 is desert

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public Transform playerAcionPosition;
    public Transform enemyAcionPosition;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public MenuNavigation BattleMenus;

    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI currentMenuText;

    public NotificationNumber theDamageNumber;

    public GameObject enemyAttackEffect;

    public PlayerInfoHandler[] playersInfos;

    public BattleTargetButton[] targetButtons;
    public BattleMagicSelect[] magicButtons;
    public BattleSpecialSelect[] specialButtons;
    public BattleItemSelect[] itemButtons;
    public BattleAttackSelect[] attackButtons;
    public BattleTargetButton[] selfButtons;
    public List<BattleChar> activeBattlers = new List<BattleChar>();
    public int currentTurn;
    public bool turnWaiting;
    public bool bossBattle;
    public bool impossibleBattle;


    public bool next=false;////////////////////////
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
                    BattleMenus.offMenu(true);
                    currentPlayerText.text = activeBattlers[currentTurn].charName;//write the name of the current char                   
                }
                else//if its not the player
                {
                    BattleMenus.offMenu(false);
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
                activeBattlers[currentTurn].currentSP = activeBattlers[currentTurn].maxSP;
            }
            if (Input.GetKeyDown(KeyCode.L))///for test!!
            { 
                for (int i = 0; i < GameManager.instance.totalItems.Length; i++)
                {
                    GameManager.instance.totalItems[i].ItemAmount = 10;
                }
            }
            if (Input.GetKeyDown(KeyCode.M))///for test!!
            {
                for (int i = 0; i < activeBattlers.Count; i++)
                {
                    if(!activeBattlers[i].isPlayer)
                    {
                        activeBattlers[i].currentHP = 1;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.J))///for test!!
            {
                for (int i = 0; i < activeBattlers.Count; i++)
                {
                    if (activeBattlers[i].isPlayer)
                    {
                        activeBattlers[i].currentHP = 1;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.P))///for test!!
            {
                for (int i = 0; i < activeBattlers.Count; i++)
                {
                    if (activeBattlers[i].isPlayer)
                    {
                        if(i==0)
                        {
                            activeBattlers[i].currentHP = 1;
                        }                           
                        else
                        {
                            activeBattlers[i].currentHP = 0;
                        }
                        
                    }
                }
            }
        }
    }

    public IEnumerator PrepareforBattleStart(string[] enemiesToSpawn)//a method for activate the fade effect and then go to battle
    {
        GameManager.instance.battleActive = true;//rise the flag for GameManager
        GameManager.instance.dialogActive = false;//for the knights
        FadeManager.instance.BattleTransition("Battle");
        AudioManager.instance.StopMusic();//stop the current music
        AudioManager.instance.PlaySFX(10);//the transition sound
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        BattleStart(enemiesToSpawn);
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);//put the camera on the battle
        battleCanves.SetActive(false);//wait until we fade in
        battleScene.SetActive(true);//show the battleScene
        yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);
        battleCanves.SetActive(true);//now show the canves
    }
    public void BattleStart(string[] enemiesToSpawn)//a method for staring the battle(only runs once per battle)//add info player activation on gamemanager!
    {
        if (!battleActive)//if the battleActive is false
        {            
            battleActive = true;//make it true
            for (int i = 0; i < playerPositions.Length; i++)//put all active players with theres stats 
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            playersInfos[i].gameObject.SetActive(true);//active the info
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);//add the player to the list
                            CharStats thePlayer = GameManager.instance.playerStats[i];//for easy access
                            playersInfos[i].hpSlider.maxValue = thePlayer.maxHP;
                            playersInfos[i].mpSlider.maxValue = thePlayer.maxMP;
                            playersInfos[i].spSlider.maxValue = thePlayer.maxSP;//////////////////////////
                            activeBattlers[i].level = thePlayer.playerLevel;
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].currentSP = thePlayer.maxSP;
                            activeBattlers[i].maxSP = thePlayer.maxSP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].dexterity = thePlayer.dexterity;
                            activeBattlers[i].movesAvailable = thePlayer.movesAvailable;
                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)//only 3 enemies at once!!!, add all enemy prefab to this!!!!!!!!!!!!!!!!!!
            {
                if (enemiesToSpawn[i] != "")//if the enemy name is not empty
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy;
                            if (enemyPrefabs[j].isMapBoss|| enemyPrefabs[j].isGameBoss)//put the boos in his position
                            {
                                newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[2].position, enemyPositions[i].rotation);
                                newEnemy.transform.parent = enemyPositions[2];
                                bossBattle = true;
                            }
                            else//if not then regular position
                            {
                                 newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                                newEnemy.transform.parent = enemyPositions[i];
                            }
                            
                            activeBattlers.Add(newEnemy);//add the Enemy to the list
                        }
                    }
                }
            }

            string sceneName = SceneManager.GetActiveScene().name;

            if ((sceneName == "MB_MapForBattle")|| (sceneName == "DeltaForest"))//later for forest battles
            {
                currenctBattleImeg.sprite = battleImeges[0];
                AudioManager.instance.PlayBGM(8);//turn on the battle music
            }
            else if (sceneName == "MB_SceneMoveTest")//later for other battles
            {
                currenctBattleImeg.sprite = battleImeges[1];
                AudioManager.instance.PlayBGM(10);//turn on the battle music
            }
            if (bossBattle == true)
            {
                if (enemiesToSpawn[0] == "Garland")//the first is always the boss!!!
                {
                    AudioManager.instance.PlayBGM(9);
                    if (GameManager.instance.numberOfElement<3)//check for elements number
                    {
                        impossibleBattle = true;
                    }
                }
                else if (enemiesToSpawn[0] == "DarkRoselia")//change to other boss name and music
                {
                    AudioManager.instance.PlayBGM(12);
                }

                /*else if (enemiesToSpawn[0] == "otherBoss")//change to other boss name and music
                {
                    AudioManager.instance.PlayBGM(9);
                }*/
            }
            turnWaiting = true;//rise the flag
            currentTurn = 0;//the first turn
            UpdateUIStats();//update the stats
            currentMenuText.text = "Main";//what is the current menu
        }
    }
    public void UpdateUIStats()//a method for keeping the info up to date///////later work on the playerinfo to move to BattleStart
    {
        for (int i = 0; i < playersInfos.Length; i++)//a loop for the info
        {
            if (activeBattlers.Count > i)//for the next "if" we dont want activeBattlers[i] to not work because i is smaller then activeBattlers.Count
            {
                if (activeBattlers[i].isPlayer)//if its a player
                {
                    BattleChar playerData = activeBattlers[i];//for easy access
                    playersInfos[i].playerHP.text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;// Update the hp..the clmap is for not showing minus numbers
                    playersInfos[i].playerMP.text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;// Update the mp..the clmap is for not showing minus numbers
                    playersInfos[i].playerSP.text = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue) + "/" + playerData.maxSP;// Update the Sp..the clmap is for not showing minus numbers
                    playersInfos[i].hpSlider.value = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue);
                    playersInfos[i].mpSlider.value = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue);
                    playersInfos[i].spSlider.value = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue); 
                    if (playerData.currentHP> playerData.maxHP)//if the currentHP is bigger them the maxHP then fix it
                    {
                        playerData.currentHP = playerData.maxHP;
                    }
                    if (playerData.currentMP > playerData.maxMP)//if the currentMP is bigger them the maxMP then fix it
                    {
                        playerData.currentMP = playerData.maxMP;
                    }
                    if (playerData.currentSP > playerData.maxSP)//if the currentSP is bigger them the maxSP then fix it
                    {
                        playerData.currentSP = playerData.maxSP;
                    }
                }
                else//if its not a player
                {
                    playersInfos[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playersInfos[i].gameObject.SetActive(false);
            }
        }
    }
    public void NextTurn()//a method for going to the next turn
    {
        activeBattlers[currentTurn].move = false;
        next = false;
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)//if it over the limit 
        {
            currentTurn = 0;//reset it
        }
        turnWaiting = true;//rise the flag
        
        currentMenuText.text = "Main";//what is the current menu
        UpdateBattle();////update the battle
        StatusBuffsCheck(activeBattlers[currentTurn]);//check the status here
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
                if ((activeBattlers[i].isPlayer)||(activeBattlers[i].isMapBoss) ||(activeBattlers[i].isGameBoss))
                {
                    activeBattlers[i].anim.SetBool("Dying", true);
                }
                else if (activeBattlers[i].isRegularEnemy)
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
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
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                 StartCoroutine(GameOverCo());
            }
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
        if(activeBattlers[currentTurn].isRegularEnemy==true)
        {
            yield return new WaitForSeconds(1f);
            EnemyAttack();
            yield return new WaitForSeconds(1f);
            NextTurn();
        }
        else if ((activeBattlers[currentTurn].isMapBoss==true)||(activeBattlers[currentTurn].isGameBoss==true))
        {
            StartCoroutine(MoveToEnemyAtkPosAndActCo());
        }
    }
    public void EnemyAttack()// a method for enemy attack
    {
        BattleMove enemyMove;
        List<int> players = new List<int>();//the list for all players
        for (int i = 0; i < activeBattlers.Count; i++)//add only alive players
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];//random select targets from the list
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Count);//random select an attack of the enemy
        enemyMove = activeBattlers[currentTurn].movesAvailable[selectAttack];
        Instantiate(activeBattlers[currentTurn].movesAvailable[selectAttack].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);//make the effect appear on the target
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);//white circle on the attacking enemy to know which one is attacking
        DealDamage(selectedTarget, enemyMove,false);//deal the damage
    }
    public void BossTrun()// a method for Boss attack
    {
        BattleMove enemyMove = activeBattlers[currentTurn].movesAvailable[0];//initialize to temp value
        List<int> players = new List<int>();//the list for all players
        int lowsetHpIndex;//for choosing the lowset hp target
        bool moveChosen = false;//a flag for knowing when the move was chosen 
        bool offense = false;//if its attack or heal/buff
        for (int i = 0; i < activeBattlers.Count; i++)//add only alive players
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        lowsetHpIndex = players[0];//initialize to temp value
        for (int i = 0; i < players.Count; i++)
        {
            if (activeBattlers[players[i]].currentHP <= activeBattlers[lowsetHpIndex].currentHP)//save the lowest hp
            {
                lowsetHpIndex = players[i];
            }
        }
        while (moveChosen==false)//do this until we chose the move
        {
            float minChance = 0;//the min Chance for this type of move
            float maxChance = 0;//the max Chance for this type of move
            bool successfullyChosen = false;//we have the Sp/Mp for this move
            int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Count);//random select an attack of the enemy
            enemyMove = activeBattlers[currentTurn].movesAvailable[selectAttack];//choose the move randomly
            switch (enemyMove.theType)
            {
                case BattleMove.moveType.Attack:
                    if (((enemyMove.moveSpCost>0)&&(activeBattlers[currentTurn].currentSP> enemyMove.moveSpCost))|| enemyMove.moveSpCost==0)//check if we can use this move
                    {
                        minChance = 75;
                        maxChance = 100;
                        offense = true;
                        successfullyChosen = true;//if we cant then true
                    }
                    else//if we cant then false
                    {
                        successfullyChosen = false;
                    }
                    break;
                case BattleMove.moveType.AttackSpecial:
                    if ((activeBattlers[currentTurn].currentSP > enemyMove.moveSpCost) && (activeBattlers[currentTurn].currentMP > enemyMove.moveMpCost))//check if we can use this move
                    {
                        minChance = 0;
                        maxChance = 35;
                        offense = true;
                        successfullyChosen = true;//if we cant then true
                    }
                    else//if we cant then false
                    {
                        successfullyChosen = false;
                    }
                    break;
                case BattleMove.moveType.SelfSpecial:
                    if ((activeBattlers[currentTurn].currentSP > enemyMove.moveSpCost) && (activeBattlers[currentTurn].currentMP > enemyMove.moveMpCost))//check if we can use this move
                    {
                        minChance = 35;
                        maxChance = 50;
                        offense = false;
                        successfullyChosen = true;//if we cant then true
                    }
                    else//if we cant then false
                    {
                        successfullyChosen = false;
                    }
                    break;
                case BattleMove.moveType.SelfMagic:
                    if (activeBattlers[currentTurn].currentMP > enemyMove.moveMpCost)//check if we can use this move
                    {
                        minChance = 35;
                        maxChance = 50;
                        offense = false;
                        successfullyChosen = true;//if we cant then true
                    }
                    else//if we cant then false
                    {
                        successfullyChosen = false;
                    }
                    break;
                case BattleMove.moveType.AttackMagic:
                    if (activeBattlers[currentTurn].currentMP > enemyMove.moveMpCost)//check if we can use this move
                    {
                        minChance = 50;
                        maxChance = 75;
                        offense = true;
                        successfullyChosen = true;//if we cant then true
                    }
                    else//if we cant then false
                    {
                        successfullyChosen = false;
                    }
                    break;
                default:
                    break;
            }
            if ((enemyMove.statusBuff=="HP")&& (activeBattlers[currentTurn].currentHP == activeBattlers[currentTurn].maxHP))
            {
                successfullyChosen = false;
            }
            if ((enemyMove.statusBuff == "ALL") && (activeBattlers[currentTurn].bounusTurn[0]>0))
            {
                successfullyChosen = false;
            }

            if (successfullyChosen == true)//if we can use the move then  
            {
                float chance = Random.Range(0, 100);
                if ((chance >= minChance) && (chance < maxChance))//check randomly if the chance is between the min max chance of the move type
                {
                    moveChosen = true;//if it its then stop loop
                }
            }
            else//if not then loop
            {
                moveChosen = false;
            }
        }
        if(enemyMove.statusBuff=="")
        {
            activeBattlers[currentTurn].currentMP -= enemyMove.moveMpCost;
            activeBattlers[currentTurn].currentSP -= enemyMove.moveSpCost;
        }
        else
        {
            float mpCost = (float)(activeBattlers[currentTurn].maxMP * (enemyMove.moveMpCost / 100.0));
            float spCost = (float)(activeBattlers[currentTurn].maxSP * (enemyMove.moveSpCost / 100.0));
            activeBattlers[currentTurn].currentMP -= Mathf.RoundToInt(mpCost);
            activeBattlers[currentTurn].currentSP -= Mathf.RoundToInt(spCost);
        }
        if (offense==true)//if the move its an attack move
        {
            int selectedTarget = players[Random.Range(0, players.Count)];//random select targets from the list
            if (selectedTarget != lowsetHpIndex)
            {
                float chance = 100 / players.Count + 5;//the chance to select lowset Hp target
                if (chance <= Random.Range(0, 100))//check randomly if we hit the chance
                {
                    selectedTarget = lowsetHpIndex;//if we do then select the lowset Hp target
                }
            }
            DealDamage(selectedTarget, enemyMove,false);//pass the target the move and a flag that tell that its an enemy            
        }
        else//if not offense
        {
            Dealdefense(enemyMove,null, currentTurn, false,false);
        }
    }
    public void StatusBuffsCheck(BattleChar playerData)//a method to check the status of the char;
    {
        for (int j = 0; j < playerData.bounusTurn.Length; j++)//status buff check
        {
            if (playerData.bounusTurn[j] == 0)//if the bonus time is passed then
            {
                playerData.statusBounus[j] = 0;//reset the bonus
            }
            else//if there still turn left
            {
                playerData.bounusTurn[j]--;//minus this turn
            }
        }
    }
    public void DealDamage(int target, BattleMove move,bool playerOrEnemy)//later to add miss, critical, res , magic? ,playerOrEnemy->true player,false enemy
    {
        int damageToGive = 0;
        if (move.moveTargetAll == false)//if the move is only for one enemy
        {
            float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0];
            float defPwr = activeBattlers[target].defense + activeBattlers[target].statusBounus[1];
            float damageCalc = (atkPwr / defPwr)* move.movePower* activeBattlers[currentTurn].level;//v3
            damageToGive = Mathf.RoundToInt(damageCalc);
            if ((impossibleBattle==true)&&(playerOrEnemy==true))//if impossibleBattle is true and the currentTurn is player then no damage!
            {
                damageToGive = 0;
            }
            else if ((impossibleBattle == true) && (playerOrEnemy == false))
            {
                damageToGive = 9999;
            }
            if (activeBattlers[currentTurn].isRegularEnemy == true)//for regular enemy numbers aka no anim
            {
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damageToGive);//make the damage appear on screen
                activeBattlers[target].currentHP -= damageToGive;
            }
            else 
            {
                if (move.isAttck())
                {
                  StartCoroutine(AnimeteAttckCo(target, damageToGive, move, playerOrEnemy));

                }
                else if (move.isAttackMagic())
                {
                    StartCoroutine(AnimeteAttackMagicCo(target, damageToGive, move, playerOrEnemy));
                }
            }
        }
        else//if the move is for all enemies 
        {//maybe another formula later
            float defPwr = 0;
            float atkPwr = 0;
            if (playerOrEnemy)//true->player
            {
                List<int> Enemies = new List<int>();//list of enemies
                for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                {
                    if ((!activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))
                    {
                        Enemies.Add(i);
                        defPwr += (activeBattlers[i].defense + activeBattlers[i].statusBounus[1]);//add all of the defense of all the enemies
                    }
                }
                atkPwr = (activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0]) * Enemies.Count;//the strength of the current player times the number of enemies
                float damageCalc = (atkPwr / defPwr) * move.movePower * activeBattlers[currentTurn].level;//v3
                damageToGive = Mathf.RoundToInt(damageCalc);//to int
                if ((impossibleBattle == true) && (playerOrEnemy == true))//if impossibleBattle is true and the currentTurn is player then no damage!
                {
                    damageToGive = 0;
                }
                else if ((impossibleBattle == true) && (playerOrEnemy == false))
                {
                    damageToGive = 9999;
                }
                StartCoroutine(AnimeteAttackSpecialCo(Enemies, damageToGive, move, playerOrEnemy));
            }
            else//false->enemy
            {
                List<int> Players = new List<int>();//list of enemies
                for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                {
                    if ((activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))
                    {
                        Players.Add(i);
                        defPwr += (activeBattlers[i].defense + activeBattlers[i].statusBounus[1]);//add all of the defense of all the players
                    }
                }
                atkPwr = (activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0]) * Players.Count;//the strength of the current enemy times the number of players
                float damageCalc = (atkPwr / defPwr) * move.movePower * activeBattlers[currentTurn].level;//v3
                damageToGive = Mathf.RoundToInt(damageCalc);//to int
                if ((impossibleBattle == true) && (playerOrEnemy == true))//if impossibleBattle is true and the currentTurn is player then no damage!
                {
                    damageToGive = 0;
                }
                else if ((impossibleBattle == true) && (playerOrEnemy == false))
                {
                    damageToGive = 9999;
                }
                StartCoroutine(AnimeteAttackSpecialCo(Players, damageToGive, move, playerOrEnemy));
            }
        }
        UpdateUIStats();//update the stats
    }
    public void Dealdefense(BattleMove move, BattleItem item, int target, bool moveOrItem, bool playerOrEnemy)//later to add miss, critical, res , magic? 
    {
        if (moveOrItem == false)//move
        {
            if (move.moveTargetAll == false)//one target
            {
                if ((move.statusBuff == "Attack") || (move.statusBuff == "ALL"))//if the buff is Attack
                {
                    activeBattlers[target].bounusTurn[0] = 3;//the bonus turn time is will last
                    float bonus = (float)(activeBattlers[target].strength * (move.movePower / 100.0));//add movePower percent to status
                    activeBattlers[target].statusBounus[0] = Mathf.RoundToInt(bonus);//the bonus itself
                }
                if ((move.statusBuff == "Defense") || (move.statusBuff == "ALL"))//if the buff is Defense
                {
                    activeBattlers[target].bounusTurn[1] = 3;//the bonus turn time is will last
                    float bonus = (float)(activeBattlers[target].defense * (move.movePower / 100.0));//add movePower percent to status
                    activeBattlers[target].statusBounus[1] = Mathf.RoundToInt(bonus);//the bonus itself
                }
                if (move.statusBuff == "HP")
                {
                    float bonus = (float)(activeBattlers[target].maxHP * (move.movePower / 100.0));//heal with movePower percent from max
                    activeBattlers[target].currentHP += Mathf.RoundToInt(bonus);
                    if (activeBattlers[target].currentHP > activeBattlers[target].maxHP)
                    {
                        activeBattlers[target].currentHP = activeBattlers[target].maxHP;
                    }
                    Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(Mathf.RoundToInt(bonus), "Health");//make the damage appear on screen
                }
                StartCoroutine(AnimeteSelfMagicCo(target, move, playerOrEnemy));
            }
            else//all targets
            {
                List<int> Targets = new List<int>();//list of enemies
                if (playerOrEnemy)//true->player
                {                     
                    for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                    {
                        if ((activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//list of players
                        {
                            Targets.Add(i);
                        }
                    }
                }
                else//false->enemy
                {
                    for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                    {
                        if (!(activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//list of enemy
                        {
                            Targets.Add(i);
                        }
                    }
                }
                for (int i = 0; i < Targets.Count; i++)//go in all the battlers
                {
                    if ((move.statusBuff == "Attack")|| (move.statusBuff == "ALL"))//if the buff is Attack
                    {
                        activeBattlers[Targets[i]].bounusTurn[0] = 3;//the bonus turn time is will last
                        float bonus = (float)(activeBattlers[Targets[i]].strength * (move.movePower / 100.0)) ;//add movePower percent to status
                        activeBattlers[Targets[i]].statusBounus[0] = Mathf.RoundToInt(bonus);//the bonus itself
                    }
                    if ((move.statusBuff == "Defense") || (move.statusBuff == "ALL"))//if the buff is Defense
                    {
                        activeBattlers[Targets[i]].bounusTurn[1] = 3;//the bonus turn time is will last
                        float bonus = (float)(activeBattlers[Targets[i]].defense * (move.movePower / 100.0));//add movePower percent to status
                        activeBattlers[Targets[i]].statusBounus[1] = Mathf.RoundToInt(bonus);//the bonus itself
                    }
                    if (move.statusBuff == "HP")
                    {
                        float bonus = (float)(activeBattlers[Targets[i]].maxHP * (move.movePower / 100.0));//heal with movePower percent from max
                        activeBattlers[Targets[i]].currentHP += Mathf.RoundToInt(bonus);
                        if (activeBattlers[Targets[i]].currentHP> activeBattlers[Targets[i]].maxHP)
                        {
                            activeBattlers[Targets[i]].currentHP = activeBattlers[Targets[i]].maxHP;
                        }
                    }
                }
                StartCoroutine(AnimeteSelfSpecialCo(Targets, move, playerOrEnemy));
            }            
        }
        else if (moveOrItem == true)//item
        {
            if (item.ishpPotion())
            {
                activeBattlers[target].currentHP += item.ItemHp;//add to current hp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemHp, "Health");//make the damage appear on screen
            }
            else if (item.ismpPotion())
            {
                activeBattlers[target].currentMP += item.ItemMp;//add to current mp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemMp, "Mana");//make the damage appear on screen
            }
            else if (item.isspPotion())
            {
                activeBattlers[target].currentSP += item.ItemSp;//add to current sp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemSp, "Special");//make the damage appear on screen
            }
            else if (item.isElixir())
            {
                activeBattlers[target].currentHP += item.ItemHp;//add to current hp
                activeBattlers[target].currentMP += item.ItemMp;//add to current mp
                activeBattlers[target].currentSP += item.ItemSp;//add to current sp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemSp);//make the damage appear on screen
            }
            StartCoroutine(AnimeteItemCo(target, item));
        }
        UpdateUIStats();//update the stats
    }
    public IEnumerator AnimeteItemCo(int target, BattleItem item)//for item, player only
    {
        activeBattlers[currentTurn].anim.SetBool("Magic_Standby", true);
        Instantiate(item.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        yield return new WaitForSecondsRealtime(item.theEffect.effectLength);
        activeBattlers[currentTurn].anim.SetBool("Magic_Standby", false);
        StartCoroutine(MoveBackAndNextTurnCo());
    }
    public IEnumerator AnimeteAttackSpecialCo(List<int> targets, int damage, BattleMove move,bool playerOrEnemy)//for Attacking all enemy.. one effect 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        if(move.animateName== "Limt_Atk")//if the move animName is magic then dont wait
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        }
        for (int i = 0; i < targets.Count; i++)//for every enemy
        {
            Instantiate(move.theEffect, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation);
            activeBattlers[targets[i]].currentHP -= damage;//take hp
            Instantiate(theDamageNumber, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation).SetNotification(damage);//make the damage appear on screen
        }           
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);
        if (move.animateName == "Limt_Atk")//if the move animName is magic then dont wait
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        }
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        if (playerOrEnemy == true)
        {
            StartCoroutine(MoveBackAndNextTurnCo());
        }
        else
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());
        }
    }
    public IEnumerator AnimeteAttckCo(int target , int damage,BattleMove move, bool playerOrEnemy)//for Attacking one target.. one effects ,playerOrEnemy->true player,false enemy
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        activeBattlers[target].currentHP -= damage;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        if (playerOrEnemy==true)
        {
            StartCoroutine(MoveBackAndNextTurnCo());
        }
        else
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());
        }
    }
    public IEnumerator AnimeteAttackMagicCo(int target, int damage, BattleMove move, bool playerOrEnemy)//for Attacking one enemy.. skill effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        activeBattlers[target].currentHP -= damage;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        if (playerOrEnemy == true)
        {
            StartCoroutine(MoveBackAndNextTurnCo());
        }
        else
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());
        }
    }
    public IEnumerator AnimeteSelfMagicCo(int target, BattleMove move,bool playerOrEnemy)//for support one ally.. skill effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        if (playerOrEnemy == true)
        {
            StartCoroutine(MoveBackAndNextTurnCo());
        }
        else
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());
        }
    }
    public IEnumerator AnimeteSelfSpecialCo(List<int> targets,BattleMove move, bool playerOrEnemy)//for support all enemy.. one effect 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        if (move.animateName == "Limt_Atk")//if the move animName is magic then dont wait
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        }
        for (int i = 0; i < targets.Count; i++)//for every enemy
        {
            Instantiate(move.theEffect, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation);
            if (move.statusBuff == "HP")
            {
                Instantiate(theDamageNumber, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation).SetNotification(move.movePower, "Health");//make the damage appear on screen
            }
        }
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);
        if (move.animateName == "Limt_Atk")//if the move animName is magic then dont wait
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        }
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        if (playerOrEnemy == true)
        {
            StartCoroutine(MoveBackAndNextTurnCo());
        }
        else
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());
        }
    }
    public IEnumerator MoveBackAndNextTurnCo()//for moving back to original position
    {
        activeBattlers[currentTurn].effect1 = false;
        activeBattlers[currentTurn].effect2 = false;
        activeBattlers[currentTurn].Idle = false;
        activeBattlers[currentTurn].anim.SetBool("Move", true);       
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, activeBattlers[currentTurn].transform.parent);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != activeBattlers[currentTurn].transform.parent.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        BattleMenus.goToMenu(0, 0);
        BattleMenus.buttonSelect();
        NextTurn();
    }
    public IEnumerator MoveBackEnemyAndNextTurnCo()//for moving back to original position
    {
        activeBattlers[currentTurn].effect1 = false;
        activeBattlers[currentTurn].effect2 = false;
        activeBattlers[currentTurn].Idle = false;
        activeBattlers[currentTurn].anim.SetBool("Move", true);
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, activeBattlers[currentTurn].transform.parent);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != activeBattlers[currentTurn].transform.parent.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        NextTurn();
    }
    public IEnumerator MoveToAtkPosAndActCo(BattleMove move, BattleItem item, int selectedTarget, bool offense)
    {
        activeBattlers[currentTurn].anim.SetBool("Move", true);
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, playerAcionPosition.transform);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != playerAcionPosition.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        activeBattlers[currentTurn].move = false;
        if (offense == true)//no item
        {
            DealDamage(selectedTarget, move,true);//deal the damage
        }
        else//maybe item
        {
            if (move == null)
            {
                Dealdefense(null, item, selectedTarget, true,true);
            }
            else if (item == null)
            {
                Dealdefense(move, null, selectedTarget, false,true);
            }
        }
    }
    public IEnumerator MoveToEnemyAtkPosAndActCo()
    {
        activeBattlers[currentTurn].anim.SetBool("Move", true);
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, enemyAcionPosition.transform);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != enemyAcionPosition.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        activeBattlers[currentTurn].move = false;
        BossTrun();
    }
    public void PlayerAction(BattleMove move, BattleItem item, int selectedTarget,bool offense)//a method for player attack//////////////add animation //////////////
    {
        BattleMenus.offButtons();
        StartCoroutine(MoveToAtkPosAndActCo( move,  item,  selectedTarget,  offense));       
    }
    public void OpenTargetMenu(BattleMove attackMove,int fromMenu)//a method for opening the target menu
    {
        List<int> Enemies = new List<int>();//list of enemies
        for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }
        for (int i = 0; i < targetButtons.Length; i++)//how many target buttons there are
        {
            if (attackMove.moveTargetAll == false)//if the move is only one enemy
            {
                if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)//if the Enemies.Count is bigger then i(for no error in activeBattlers[Enemies[i]]) and the enemy hp is above 0(alive)
                {
                    targetButtons[i].gameObject.SetActive(true);//turn on the button
                    targetButtons[i].theMove = attackMove;//save the move
                    targetButtons[i].activeBattlerTarget = Enemies[i];//save the enemy target
                    targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;//show its name
                }
                else//if the enemy is dead 
                {
                    targetButtons[i].gameObject.SetActive(false);//turnoff the target
                }
            }
            else//if the move is for all enemies 
            {
                if(i==0)//turn on only the first one
                {
                    targetButtons[0].gameObject.SetActive(true);//turn on the button
                    targetButtons[0].theMove = attackMove;//save the move
                    //targetButtons[0].activeBattlerTarget = Enemies[i];//save the enemy target
                    targetButtons[0].targetName.text = "All";//show "All" on the button
                }
                else//every other button
                {
                    targetButtons[i].gameObject.SetActive(false);//turnoff the target
                }
            }              
        }
        BattleMenus.goToMenu(5, fromMenu);//go to target menu and the previous is fromMenu
    }
    public void OpenSelfMenu(BattleMove selfMove, BattleItem selfItem, int fromMenu,bool moveOrItem)//a method for opening the self target menu
    {
        List<int> players = new List<int>();//list of enemies
        for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        for (int i = 0; i < selfButtons.Length; i++)//how many self buttons there are
        {
            if(moveOrItem==false)
            {
                if (selfMove.moveTargetAll == false)//if the move is only one enemy
                {
                    if (players.Count > i && activeBattlers[players[i]].currentHP > 0)//if the players.Count is bigger then i(for no error in activeBattlers[players[i]]) and the players hp is above 0(alive)
                    {
                        selfButtons[i].gameObject.SetActive(true);//turn on the button
                        selfButtons[i].theMove = selfMove;//save the move 
                        selfButtons[i].theItem = selfItem;//save the item 
                        selfButtons[i].activeBattlerTarget = players[i];//save the players target
                        selfButtons[i].targetName.text = activeBattlers[players[i]].charName;//show its name
                        selfButtons[i].moveOrItem = moveOrItem;//true item and false if move
                    }
                    else//if the enemy is dead 
                    {
                        selfButtons[i].gameObject.SetActive(false);//turnoff the target
                    }
                }
                else//if the move is for all enemies 
                {
                    if (i == 0)//turn on only the first one
                    {
                        selfButtons[0].gameObject.SetActive(true);//turn on the button
                        selfButtons[0].theMove = selfMove;//save the move 
                        selfButtons[0].theItem = selfItem;//save the item 
                        selfButtons[0].targetName.text = "All";//show "All" on the button
                        selfButtons[0].moveOrItem = moveOrItem;//true item and false if move
                    }
                    else//every other button
                    {
                        selfButtons[i].gameObject.SetActive(false);//turnoff the target
                    }
                }
             }
            else
            {
                if (selfItem.moveTargetAll == false)//if the move is only one enemy
                {
                    if (players.Count > i && activeBattlers[players[i]].currentHP > 0)//if the players.Count is bigger then i(for no error in activeBattlers[players[i]]) and the players hp is above 0(alive)
                    {
                        selfButtons[i].gameObject.SetActive(true);//turn on the button
                        selfButtons[i].theMove = selfMove;//save the move 
                        selfButtons[i].theItem = selfItem;//save the item 
                        selfButtons[i].activeBattlerTarget = players[i];//save the players target
                        selfButtons[i].targetName.text = activeBattlers[players[i]].charName;//show its name
                        selfButtons[i].moveOrItem = moveOrItem;//true item and false if move
                    }
                    else//if the enemy is dead 
                    {
                        selfButtons[i].gameObject.SetActive(false);//turnoff the target
                    }
                }
                else//if the move is for all enemies 
                {
                    if (i == 0)//turn on only the first one
                    {
                        selfButtons[0].gameObject.SetActive(true);//turn on the button
                        selfButtons[0].theMove = selfMove;//save the move 
                        selfButtons[0].theItem = selfItem;//save the item 
                        selfButtons[0].targetName.text = "All";//show "All" on the button
                        selfButtons[0].moveOrItem = moveOrItem;//true item and false if move
                    }
                    else//every other button
                    {
                        selfButtons[i].gameObject.SetActive(false);//turnoff the target
                    }
                }
            }
        }
        BattleMenus.goToMenu(6, fromMenu);//go to self menu and the previous is fromMenu
    }
    public void OpenMagicMenu()//a method for opening the magic menu
    {
        currentMenuText.text = "Magic";//show which menu we are at
        List<Button> activeMagicButon = new List<Button>();//a list for all the buttons in this menu
        for (int i = 0,j = 0; i < magicButtons.Length;)
        {
            if (activeBattlers[currentTurn].movesAvailable.Count > j)//go in if we still have moves to map
            {
                if (activeBattlers[currentTurn].movesAvailable[j].isAttackMagic() || activeBattlers[currentTurn].movesAvailable[j].isSelfMagic())//if the move is magic then
                {
                    magicButtons[i].gameObject.SetActive(true);//turn on the button
                    activeMagicButon.Add(magicButtons[i].gameObject.GetComponent<Button>());//add the button           
                    magicButtons[i].theMove = activeBattlers[currentTurn].movesAvailable[j];//add the move

                    magicButtons[i].nameText.text = magicButtons[i].theMove.moveName;//show the move name 
                    if (magicButtons[i].theMove.isSelfMagic())
                    {
                        magicButtons[i].costText.text = magicButtons[i].theMove.moveMpCost.ToString()+"%";//show the move mp cost 
                    }
                    else
                    {
                        magicButtons[i].costText.text = magicButtons[i].theMove.moveMpCost.ToString();//show the move mp cost 
                    }
                    i++;//next button
                    j++;//next move
                }
                else//if the move is not magic
                {
                    j++;//next move
                }
            }
            else//if we are out of moves
            {               
                magicButtons[i].gameObject.SetActive(false);//turnoff the button
                i++;//next button
            }
        }
        for (int i = 0; i < activeMagicButon.Count; i++)//going on all the active buttons
        {
            if(activeBattlers[currentTurn].currentMP<activeMagicButon[i].GetComponent<BattleMagicSelect>().theMove.moveMpCost)//check if the currnet player can use the move i
            {
                activeMagicButon[i].interactable = false; //usable then disable the button
            }
            else
            {
                activeMagicButon[i].interactable = true; //not usable then disable the button
            }
        }
        BattleMenus.goToMenu(1, 0);//go to magic menu and the previous is main battle menu
    }
    public void OpenAttackMenu()//a method for opening the magic menu
    {
        currentMenuText.text = "Attack";//show which menu we are at
        List<Button> activeAttackButon = new List<Button>();//a list for all the buttons in this menu
        for (int i = 0, j = 0; i < attackButtons.Length;)
        {
            if (activeBattlers[currentTurn].movesAvailable.Count > j)//go in if we still have moves to map
            {
                if (activeBattlers[currentTurn].movesAvailable[j].isAttck())
                {
                    attackButtons[i].gameObject.SetActive(true);//turn on the button
                    activeAttackButon.Add(attackButtons[i].gameObject.GetComponent<Button>());//add the button
                    attackButtons[i].theMove = activeBattlers[currentTurn].movesAvailable[j];//add the move
                    attackButtons[i].nameText.text = attackButtons[i].theMove.moveName;//show the move name 
                    attackButtons[i].costText.text = attackButtons[i].theMove.moveSpCost.ToString();//show the move cost 
                    i++;//next button
                    j++;//next move
                }
                else//if the move is not magic
                {
                    j++;//next move
                }
            }
            else//if we are out of moves
            {
                attackButtons[i].gameObject.SetActive(false);//turnoff the button
                i++;//next button
            }
        }
        for (int i = 0; i < activeAttackButon.Count; i++)//going on all the active buttons
        {
            if (activeBattlers[currentTurn].currentSP < activeAttackButon[i].GetComponent<BattleAttackSelect>().theMove.moveSpCost)//check if the currnet player can use the move i
            {
                activeAttackButon[i].interactable = false; //usable then disable the button
            }
            else
            {
                activeAttackButon[i].interactable = true; //not usable then disable the button
            }
        }
        BattleMenus.goToMenu(4, 0);//go to magic menu and the previous is main battle menu
    }   
    public void OpenSpecialMenu()//a method for opening the magic menu
    {
        currentMenuText.text = "Special";//show which menu we are at
        List<Button> activeSpecialButon = new List<Button>();//a list for all the buttons in this menu
        for (int i = 0, j = 0; i < specialButtons.Length;)
        {
            if (activeBattlers[currentTurn].movesAvailable.Count > j)//go in if we still have moves to map
            {
                if (activeBattlers[currentTurn].movesAvailable[j].isSelfSpecial() || activeBattlers[currentTurn].movesAvailable[j].isAttackSpecial())
                {
                    specialButtons[i].gameObject.SetActive(true);//turn on the button
                    activeSpecialButon.Add(specialButtons[i].gameObject.GetComponent<Button>());//add the button
                    specialButtons[i].theMove = activeBattlers[currentTurn].movesAvailable[j];//add the move
                    specialButtons[i].nameText.text = specialButtons[i].theMove.moveName;//show the move name 

                    if (specialButtons[i].theMove.isSelfSpecial())
                    {
                        specialButtons[i].mpCostText.text = specialButtons[i].theMove.moveMpCost.ToString() + "%";//show the move mp cost 
                        specialButtons[i].spCostText.text = specialButtons[i].theMove.moveSpCost.ToString() + "%";//show the move sp cost     
                    }
                    else
                    {
                        specialButtons[i].mpCostText.text = specialButtons[i].theMove.moveMpCost.ToString();//show the move mp cost 
                        specialButtons[i].spCostText.text = specialButtons[i].theMove.moveSpCost.ToString();//show the move sp cost                     
                    }
                    i++;//next button
                    j++;//next move
                }
                else//if the move is not magic
                {
                    j++;//next move
                }
            }            
            else//if we are out of moves
            {
                specialButtons[i].gameObject.SetActive(false);//turnoff the button
                i++;//next button
        }
        }
        for (int i = 0; i < activeSpecialButon.Count; i++)//going on all the active buttons
        {
            if ((activeBattlers[currentTurn].currentMP < activeSpecialButon[i].GetComponent<BattleSpecialSelect>().theMove.moveMpCost)|| (activeBattlers[currentTurn].currentSP < activeSpecialButon[i].GetComponent<BattleSpecialSelect>().theMove.moveSpCost))//check if the currnet player can use the move i
            {
                activeSpecialButon[i].interactable = false; //usable then disable the button
            }
            else
            {
                activeSpecialButon[i].interactable = true; //usable not then disable the button
            }
        }
        BattleMenus.goToMenu(2, 0);//go to Special menu and the previous is main battle menu
    }
    public void OpenItemMenu()//a method for opening the magic menu
    {
        currentMenuText.text = "Item";//show which menu we are at
        List<Button> activeItemButon = new List<Button>();//a list for all the buttons in this menu
        for (int i = 0;i < itemButtons.Length; i++)
        {
            if (GameManager.instance.totalItems.Length > i)//go in if we still have item to map
            {
                if(GameManager.instance.totalItems[i].ItemAmount>0)
                {
                    itemButtons[i].gameObject.SetActive(true);//turn on the button
                    activeItemButon.Add(itemButtons[i].gameObject.GetComponent<Button>());//add the button
                    itemButtons[i].theItem = GameManager.instance.totalItems[i];//save the spell name
                    itemButtons[i].nameText.text = itemButtons[i].theItem.ItemName;//show the spell name 
                    itemButtons[i].amountText.text = itemButtons[i].theItem.ItemAmount.ToString();//show the spell cost 
                    itemButtons[i].theItem.itemIndex = i;//save the index
                }
                else//if we are out of moves
                {
                    itemButtons[i].gameObject.SetActive(false);//turnoff the button
                }
            }
            else//if we are out of moves
            {
                itemButtons[i].gameObject.SetActive(false);//turnoff the button
            }
        }
        for (int i = 0; i < activeItemButon.Count; i++)//going on all the active buttons
        {
            if (activeItemButon[i].GetComponent<BattleItemSelect>().theItem.ItemAmount<0)//check if the currnet player can use the move i
            {
                activeItemButon[i].interactable = false; //if not then disable the button
            }
            else
            {
                activeItemButon[i].interactable = true; //if not then disable the button
            }
        }
        BattleMenus.goToMenu(3, 0);//go to item menu and the previous is main battle menu
    }
    public IEnumerator EndBattleCo()
    {
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayBGM(6);
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP > 0)
            {
                activeBattlers[i].anim.SetBool("Win_Full", true);
            }
        }
        BattleMenus.goToMenu(0, 0);
        BattleMenus.offMenu(false);
        BattleMenus.ShowVictoryPanel(true);
        battleActive = false;
        bossBattle = false;//reset every new battle
        impossibleBattle = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        BattleMenus.ShowVictoryPanel(false);
        yield return new WaitForSeconds(.5f);
        battleCanves.SetActive(false);
        FadeManager.instance.BattleTransition("FadeBlack");
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].maxHP;//reset to health
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].maxMP;//reset to MP
                        GameManager.instance.playerStats[j].currentSP = activeBattlers[i].currentSP;//keep the sp for next battle
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        BattleMenus.theInfoHolder.SetActive(true);
        battleCanves.SetActive(false);
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);
        GameManager.instance.battleActive = false;

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);        
    }
    public IEnumerator GameOverCo()
    {
        AudioManager.instance.StopMusic();//stop the battle music
        AudioManager.instance.PlaySFX(0);//start the death music
        battleActive = false;
        bossBattle = false;//reset every new battle
        impossibleBattle = false;
        //GameManager.instance.battleActive = false;
        battleCanves.SetActive(false);
        FadeManager.instance.ScenenTransition("GameOver");
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        GameManager.instance.battleActive = false;
        battleScene.SetActive(false);
        for (int i = 0; i < activeBattlers.Count; i++)

            Destroy(activeBattlers[i].gameObject);
        activeBattlers.Clear();
        SceneManager.LoadScene("GameOver");


    }
}

