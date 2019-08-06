using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;//only one BattleManager object
    private bool battleActive;//to know if we are in battle
    public GameObject battleScene;//the canves holdeer
    public GameObject battleCanves;//the canves itself
    public SpriteRenderer currenctBattleImeg;//the currect battle image 
    public Sprite[] battleImeges;//an array of all the battle imeges
    //0 if forest
    //1 is desert
    //2 is ice
    //3 is dark
    public Transform[] playerPositions;//an array of position for the players
    public Transform[] enemyPositions;//an array of position for the enemies
    public Transform playerAcionPosition;//the position that the player move into to make action
    public Transform enemyAcionPosition;//the position that the enemies(only bosses) move into to make action
    public BattleChar[] playerPrefabs;//an array of BattleChar for the players
    public BattleChar[] enemyPrefabs;//an array of BattleChar for the enemies
    public MenuNavigation BattleMenus;//a MenuNavigation object to navigate the battle menu
    public TextMeshProUGUI currentPlayerText;//a text that shows who the current player is
    public TextMeshProUGUI currentMenuText;//a text that shows the current menu
    public NotificationNumber theDamageNumber;//NotificationNumber object that shows the damage number on the screen 
    public GameObject enemyAttackEffect;//an effect around the enemy so we know that this enemy has attacked
    public PlayerInfoHandler[] playersInfos;//an array of PlayerInfoHandler object that shows players information on the screen
    public BattleTargetButton[] targetButtons;//an array of BattleTargetButton button objects for choosing the enemy to attack
    public BattleMagicSelect[] magicButtons;//an array of BattleMagicSelect button objects for choosing a magic move
    public BattleSpecialSelect[] specialButtons;//an array of BattleSpecialSelect button objects for choosing a special move
    public BattleItemSelect[] itemButtons;//an array of BattleItemSelect button objects for choosing an item 
    public BattleAttackSelect[] attackButtons;//an array of BattleAttackSelect button objects for choosing a attack move
    public BattleTargetButton[] selfButtons;//an array of BattleTargetButton button objects for choosing a player to buff
    public List<BattleChar> activeBattlers = new List<BattleChar>();// a list(because it will change in the course of the battle) of all the active Battlers
    public int currentTurn;//an int that tells which turn it is
    public bool turnWaiting;//to know if we are waiting for something(between the turns)
    public bool bossBattle;//if its a boss battle
    public bool lastBossBattle;//if its the last battle
    public bool impossibleBattle;//if we cant win this battle
    // Use this for initialization
    void Start()
    {
        instance = this;//only one BattleManager object
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
                    BattleMenus.offMenu(true);//show the menus in the players turns
                    currentPlayerText.text = activeBattlers[currentTurn].charName;//write the name of the current char                   
                }
                else//if its not the player
                {
                    BattleMenus.offMenu(false);//don't show the menus in the enemies turns
                    StartCoroutine(EnemyMoveCo());//start the Coroutine for the enemy turn
                }
            }
            if (GameManager.instance.cheatsON==true)//for cheats for test only
            {
                if (Input.GetKeyDown(KeyCode.N))//next turn
                {
                    NextTurn();
                }

                if (Input.GetKeyDown(KeyCode.K))//full hp and mp
                {
                    activeBattlers[currentTurn].currentMP = activeBattlers[currentTurn].maxMP;
                    activeBattlers[currentTurn].currentSP = activeBattlers[currentTurn].maxSP;
                }
                if (Input.GetKeyDown(KeyCode.L))//fill to 10 items each
                {
                    for (int i = 0; i < GameManager.instance.totalItems.Length; i++)
                    {
                        GameManager.instance.totalItems[i].ItemAmount = 10;
                    }
                }
                if (Input.GetKeyDown(KeyCode.M))//1 hp for all enemies
                {
                    for (int i = 0; i < activeBattlers.Count; i++)
                    {
                        if (!activeBattlers[i].isPlayer)
                        {
                            activeBattlers[i].currentHP = 1;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.J))//1 hp for all players
                {
                    for (int i = 0; i < activeBattlers.Count; i++)
                    {
                        if (activeBattlers[i].isPlayer)
                        {
                            activeBattlers[i].currentHP = 1;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.P))//1 hp for rain and 0 hp for the rest
                {
                    for (int i = 0; i < activeBattlers.Count; i++)
                    {
                        if (activeBattlers[i].isPlayer)
                        {
                            if (i == 0)
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
    }
    public IEnumerator PrepareforBattleStart(string[] enemiesToSpawn)//a method for activate the fade effect and then go to battle
    {
        if (GameManager.instance.battleActive == false)//if the battle already start then dont start again
        {
            GameManager.instance.battleActive = true;//rise the flag for GameManager
            GameManager.instance.dialogActive = false;//for the knights
            FadeManager.instance.BattleTransition("Battle");//fate into battle effect
            AudioManager.instance.StopMusic();//stop the current music
            AudioManager.instance.PlaySFX(10);//the transition sound
            yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//wait until the screen is black
            BattleStart(enemiesToSpawn);//start the battle with enemiesToSpawn enemies
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);//put the camera on the battle
            battleCanves.SetActive(false);//wait until we fade in
            battleScene.SetActive(true);//show the battleScene
            yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);//now the screen shows the battle 
            battleCanves.SetActive(true);//now show the canves
        }       
    }
    public void BattleStart(string[] enemiesToSpawn)//a method for staring the battle(only runs once per battle)
    {
        if (!battleActive)//if the battleActive is false
        {            
            battleActive = true;//make it true
            for (int i = 0; i < playerPositions.Length; i++)//put all active players with theres stats 
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)//go on all the playerPrefabs
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)//if the name of playerPrefabs[j] is equal to GameManager.instance.playerStats[i] name then add this player
                        {
                            playersInfos[i].gameObject.SetActive(true);//active the info
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);//put into the position
                            newPlayer.transform.parent = playerPositions[i];//make the playerPositions parent of this player
                            activeBattlers.Add(newPlayer);//add the player to the list
                            CharStats thePlayer = GameManager.instance.playerStats[i];//for easy access
                            playersInfos[i].hpSlider.maxValue = thePlayer.maxHP;
                            playersInfos[i].mpSlider.maxValue = thePlayer.maxMP;
                            playersInfos[i].spSlider.maxValue = thePlayer.maxSP;
                            activeBattlers[i].level = thePlayer.playerLevel;
                            activeBattlers[i].currentHP = thePlayer.maxHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.maxMP;
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
            for (int i = 0; i < enemiesToSpawn.Length; i++)//for each enemy name in enemiesToSpawn(3 max)
            {
                if (enemiesToSpawn[i] != "")//if the enemy name is not empty
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)//go on all the enemyPrefabs
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])//if the name of enemyPrefabs[j] is equal to enemiesToSpawn[i]
                        {
                            BattleChar newEnemy;//the enemy object
                            if (enemyPrefabs[j].isMapBoss|| enemyPrefabs[j].isGameBoss)//if its a boss
                            {
                                newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[2].position, enemyPositions[i].rotation);//put the boos in his position
                                newEnemy.transform.parent = enemyPositions[2];//make the enemyPositions parent of this boos
                                bossBattle = true;//rise the flag to know that this is a boss battle
                            }
                            else//if not then 
                            {
                                newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);//regular position
                                newEnemy.transform.parent = enemyPositions[i];//make the enemyPositions parent of this enemy
                            }                           
                            activeBattlers.Add(newEnemy);//add the Enemy to the list
                        }
                    }
                }
            }
            string sceneName = SceneManager.GetActiveScene().name;//get that name of this scene
            if ((sceneName == "MB_MapForBattle")|| (sceneName == "DeltaForest") || (sceneName == "DeltaForestKnight"))//if its the Forests or MB_MapForBattle scenes then
            {
                currenctBattleImeg.sprite = battleImeges[0];//show the Forest battle image
                AudioManager.instance.PlayBGM(8);//turn on the battle music
            }
            else if ((sceneName == "MB_SceneMoveTest")|| (sceneName == "ChronoDesert") || (sceneName == "ChronoDesertKnight"))//if its the Deserts or MB_SceneMoveTest scenes then
            {
                currenctBattleImeg.sprite = battleImeges[1];//show the Desert battle image
                AudioManager.instance.PlayBGM(10);//turn on the battle music
            }
            else if ((sceneName == "MB_jkljkl") || (sceneName == "IceAge") || (sceneName == "IceAgeKnight"))//if its the Ices or MB_jkljkl scenes then
            {
                currenctBattleImeg.sprite = battleImeges[2];//show the Ice battle image
                AudioManager.instance.PlayBGM(16);//turn on the battle music
            }
            else if ((sceneName == "MB_jklddjkl") || (sceneName == "DarkLand"))//if its the Dark or MB_jklddjkl scenes then
            {
                currenctBattleImeg.sprite = battleImeges[3];//show the Dark battle image
                AudioManager.instance.PlayBGM(19);//turn on the battle music
            }
            if (bossBattle == true)//if its a boss battle (enemiesToSpawn[0] is the boss)
            {
                if (enemiesToSpawn[0] == "Garland")//if its Garland
                {
                    if (GameManager.instance.numberOfElement<3)//check for elements number to know if its the end battle or not
                    {
                        impossibleBattle = true;//cant win battle
                        AudioManager.instance.PlayBGM(9);//music for that battle
                        lastBossBattle = false;//its not the last battle
                    }
                    else
                    {
                        lastBossBattle = true;//its is the last battle
                    }
                }
                else if (enemiesToSpawn[0] == "DarkRoselia")//if its DarkRoselia
                {
                    AudioManager.instance.PlayBGM(12);//music for that battle
                }
                else if (enemiesToSpawn[0] == "DarkAiden")//if its DarkAiden
                {
                    AudioManager.instance.PlayBGM(14);//music for that battle
                }
                else if (enemiesToSpawn[0] == "DarkSakura")//if its DarkSakura
                {
                    AudioManager.instance.PlayBGM(17);//music for that battle
                }
            }
            turnWaiting = true;//rise the flag
            currentTurn = 0;//the first turn
            UpdateUIStats();//update the stats
            currentMenuText.text = "Main";//what is the current menu
        }
    }
    public void UpdateUIStats()//a method for keeping the info up to date and showing only the current party info
    {
        for (int i = 0; i < playersInfos.Length; i++)//a loop for the info
        {
            if (activeBattlers.Count > i)//for the next "if"...because we want to check activeBattlers[i] and if i is smaller then activeBattlers.Count it will be an error
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
                    playersInfos[i].gameObject.SetActive(false);//dont show
                }
            }
            else//if i is bigger then again dont show
            {
                playersInfos[i].gameObject.SetActive(false);
            }
        }
    }
    public void NextTurn()//a method for going to the next turn
    {
        activeBattlers[currentTurn].move = false;//the player is not moving
        currentTurn++;//next turn
        if (currentTurn >= activeBattlers.Count)//if it over the limit 
        {
            currentTurn = 0;//reset it
        }
        turnWaiting = true;//rise the flag        
        currentMenuText.text = "Main";//the first menu is Main
        UpdateBattle();////update the battle
        StatusBuffsCheck(activeBattlers[currentTurn]);//check the status here
        UpdateUIStats();//update the stats
    }
    public void UpdateBattle()//a method for handling dead player and enemies
    {
        bool allEnemiesDead = true;//bool to know if all players are dead
        bool allPlayersDead = true;//bool to know if all enemies are dead
        for (int i = 0; i < activeBattlers.Count; i++)//go on all the activeBattlers
        {
            if (activeBattlers[i].currentHP < 0)//if its hp is bellow 0
            {
                activeBattlers[i].currentHP = 0;//fix it to 0
            }
            if (activeBattlers[i].currentHP == 0)//if its hp 0
            {
                if ((activeBattlers[i].isPlayer)||(activeBattlers[i].isMapBoss) ||(activeBattlers[i].isGameBoss))//if iots a player or a boss
                {
                    activeBattlers[i].anim.SetBool("Dying", true);//start its Dying animation
                }
                else if (activeBattlers[i].isRegularEnemy)//else its an regular enemy
                {
                    activeBattlers[i].EnemyFade();//fade 
                }
            }
            else//its hp is not 0
            {
                if (activeBattlers[i].isPlayer)//if it is a player
                {
                    allPlayersDead = false;//put false because not all of the players are dead
                }
                else//if it is a enemy
                {
                    allEnemiesDead = false;//put false because not all of the enemies are dead
                }
            }
        }
        if (allEnemiesDead || allPlayersDead)//if one of them is true then check which one
        {
            if (allEnemiesDead)//if all enemies are dead then
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else//if all players are dead then
            {
                //end battle in failure
                 StartCoroutine(GameOverCo());
            }
        }
        else//if neither is true then
        {
            while (activeBattlers[currentTurn].currentHP == 0)//while the activeBattlers[currentTurn] is 0 
            {
                currentTurn++;//go to next turn and next activeBattlers
                if (currentTurn >= activeBattlers.Count)//if currentTurn is more then the total amount of activeBattlers
                {
                    currentTurn = 0;//reset it
                }
            }
        }
    }
    public IEnumerator EnemyMoveCo()//wait for enemy move
    {
        turnWaiting = false;//we are waiting 
        if(activeBattlers[currentTurn].isRegularEnemy==true)//if its a Regular Enemy
        {
            yield return new WaitForSeconds(1f);//wait
            EnemyAttack();//call the EnemyAttack method
            yield return new WaitForSeconds(1f);//wait
            NextTurn();//go to next turn
        }
        else if ((activeBattlers[currentTurn].isMapBoss==true)||(activeBattlers[currentTurn].isGameBoss==true))//if its a boss then
        {
            StartCoroutine(MoveToEnemyAtkPosAndActCo());//call the MoveToEnemyAtkPosAndActCo method
        }
    }
    public void EnemyAttack()// a method for enemy attack
    {
        BattleMove enemyMove;//the BattleMove of the enemy
        List<int> players = new List<int>();//the list for all players
        for (int i = 0; i < activeBattlers.Count; i++)//go on all the activeBattlers
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)//add only alive players
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];//random select targets from the list
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Count);//random select an attack of the enemy
        enemyMove = activeBattlers[currentTurn].movesAvailable[selectAttack];//the randomly selected attack
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
            if ((enemyMove.statusBuff=="HP")&& (activeBattlers[currentTurn].currentHP == activeBattlers[currentTurn].maxHP))//if the move is heal move and the hp is already max then choose again
            {
                successfullyChosen = false;
            }
            if ((enemyMove.statusBuff == "ALL") && (activeBattlers[currentTurn].bounusTurn[0]>0))//if the move is buff move and the boss already buffed then choose again
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
        if(enemyMove.statusBuff=="")//if its not a buff/heal then the cost is
        {
            activeBattlers[currentTurn].currentMP -= enemyMove.moveMpCost;
            activeBattlers[currentTurn].currentSP -= enemyMove.moveSpCost;
        }
        else//if its is a buff/heal then the cost is
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
                float chance = 100 / players.Count + 5;//the chance to select lowest Hp target is  percent bigger then the rest
                if (chance <= Random.Range(0, 100))//check randomly if we hit the chance
                {
                    selectedTarget = lowsetHpIndex;//if we do then select the lowest Hp target
                }
            }
            DealDamage(selectedTarget, enemyMove,false);//call DealDamage with : target, move,bool for player or enemy
        }
        else//if not offense
        {
            DealSupport(enemyMove,null, currentTurn, false,false);//call DealSupport with : move,null item,target,bool for item or move,bool for player or enemy
        }
    }
    public void StatusBuffsCheck(BattleChar playerData)//a method to check the buff status of the char;
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
    public void DealDamage(int target, BattleMove move,bool playerOrEnemy)//a method to deal damage..playerOrEnemy:true->player,false->enemy
    {
        int damageToGive = 0;//initialize the damage
        if (move.moveTargetAll == false)//if the move is only for one target
        {
            float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0];//atkPwr is the currentTurn strength + bunusStrength(its 0 if there no buff)
            float defPwr = activeBattlers[target].defense + activeBattlers[target].statusBounus[1];//defPwr is the target defense + bunusDefense(its 0 if there no buff)
            float damageCalc = (atkPwr / defPwr)* move.movePower* activeBattlers[currentTurn].level;//the damage is : (atkPwr / defPwr) * movePower * level
            damageToGive = Mathf.RoundToInt(damageCalc);//Round To Int the damageCalc
            if ((impossibleBattle==true)&&(playerOrEnemy==true))//if impossibleBattle is true and the currentTurn is player then cant deal any damage
            {
                damageToGive = 0;
            }
            else if ((impossibleBattle == true) && (playerOrEnemy == false))//if impossibleBattle is true and the currentTurn is enemy(the boss) then the damage is 9999
            {
                damageToGive = 9999;
            }
            if (activeBattlers[currentTurn].isRegularEnemy == true)//if regular enemy then no animation
            {
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damageToGive);//make the damage appear on screen
                activeBattlers[target].currentHP -= damageToGive;//deal the damage
            }
            else// if its not 
            {
                if (move.isAttck())//the move is attack
                {
                  StartCoroutine(AnimeteAttckCo(target, damageToGive, move, playerOrEnemy));//call the AnimeteAttckCo with: the targets,the damage, the move and bool for player or enemy

                }
                else if (move.isAttackMagic())//the move is magic
                {
                    StartCoroutine(AnimeteAttackMagicCo(target, damageToGive, move, playerOrEnemy));//call the AnimeteAttackMagicCo with: the targets,the damage, the move and bool for player or enemy
                }
            }
        }
        else//if the move is for all target 
        {
            float defPwr = 0;//initialize the defPwr
            float atkPwr = 0;//initialize the atkPwr
            if (playerOrEnemy)//if its player then
            {
                List<int> Enemies = new List<int>();//list of enemies
                for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                {
                    if ((!activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//add alive only enemy
                    {
                        Enemies.Add(i);
                        defPwr += (activeBattlers[i].defense + activeBattlers[i].statusBounus[1]);//add all of the defense of all the enemies
                    }
                }
                atkPwr = (activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0]) * Enemies.Count;//atkPwr is the (currentTurn strength + bunusStrength(its 0 if there no buff)) * the amount of targets
                float damageCalc = (atkPwr / defPwr) * move.movePower * activeBattlers[currentTurn].level;//the damage is : (atkPwr / defPwr) * movePower * level
                damageToGive = Mathf.RoundToInt(damageCalc);//Round To Int the damageCalc
                if ((impossibleBattle == true) && (playerOrEnemy == true))//if impossibleBattle is true and the currentTurn is player then cant deal any damage
                {
                    damageToGive = 0;
                }
                else if ((impossibleBattle == true) && (playerOrEnemy == false))//if impossibleBattle is true and the currentTurn is enemy(the boss) then the damage is 9999
                {
                    damageToGive = 9999;
                }
                StartCoroutine(AnimeteAttackSpecialCo(Enemies, damageToGive, move, playerOrEnemy));//call the AnimeteAttackSpecialCo with: the targets,the damage, the move and bool for player or enemy(because its all target its have to be AnimeteAttackSpecialCo)
            }
            else//if its enemy then
            {
                List<int> Players = new List<int>();//list of players
                for (int i = 0; i < activeBattlers.Count; i++)//for adding the players
                {
                    if ((activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//add alive only player
                    {
                        Players.Add(i);
                        defPwr += (activeBattlers[i].defense + activeBattlers[i].statusBounus[1]);//add all of the defense of all the players
                    }
                }
                atkPwr = (activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0]) * Players.Count;//atkPwr is the (currentTurn strength + bunusStrength(its 0 if there no buff)) * the amount of targets
                float damageCalc = (atkPwr / defPwr) * move.movePower * activeBattlers[currentTurn].level;//the damage is : (atkPwr / defPwr) * movePower * level
                damageToGive = Mathf.RoundToInt(damageCalc);//Round To Int the damageCalc
                if ((impossibleBattle == true) && (playerOrEnemy == true))//if impossibleBattle is true and the currentTurn is player then cant deal any damage
                {
                    damageToGive = 0;
                }
                else if ((impossibleBattle == true) && (playerOrEnemy == false))//if impossibleBattle is true and the currentTurn is enemy(the boss) then the damage is 9999
                {
                    damageToGive = 9999;
                }
                StartCoroutine(AnimeteAttackSpecialCo(Players, damageToGive, move, playerOrEnemy));//call the AnimeteAttackSpecialCo with: the targets,the damage, the move and bool for player or enemy(because its all target its have to be AnimeteAttackSpecialCo)
            }
        }
        UpdateUIStats();//update the stats
    }
    public void DealSupport(BattleMove move, BattleItem item, int target, bool moveOrItem, bool playerOrEnemy)//a method to deal support..moveOrItem:true->item,false->move..playerOrEnemy:true->player,false->enemy
    {
        if (moveOrItem == false)//if its move
        {
            if (move.moveTargetAll == false)//if its one target
            {
                if ((move.statusBuff == "Attack") || (move.statusBuff == "ALL"))//if the buff is Attack or All
                {
                    activeBattlers[target].bounusTurn[0] = 3;//the bonus turn time is will last
                    float bonus = (float)(activeBattlers[target].strength * (move.movePower / 100.0));//add movePower percent to status
                    activeBattlers[target].statusBounus[0] = Mathf.RoundToInt(bonus);//the bonus itself
                }
                if ((move.statusBuff == "Defense") || (move.statusBuff == "ALL"))//if the buff is Defense or All
                {
                    activeBattlers[target].bounusTurn[1] = 3;//the bonus turn time is will last
                    float bonus = (float)(activeBattlers[target].defense * (move.movePower / 100.0));//add movePower percent to status
                    activeBattlers[target].statusBounus[1] = Mathf.RoundToInt(bonus);//the bonus itself
                }
                if (move.statusBuff == "HP")//if the buff is heal
                {
                    float bonus = (float)(activeBattlers[target].maxHP * (move.movePower / 100.0));//heal with movePower percent from max
                    activeBattlers[target].currentHP += Mathf.RoundToInt(bonus);//the heal itself
                    if (activeBattlers[target].currentHP > activeBattlers[target].maxHP)//if the currentHP is above the maxHP
                    {
                        activeBattlers[target].currentHP = activeBattlers[target].maxHP;//fix it
                    }
                    Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(Mathf.RoundToInt(bonus), "Health");//make the heal appear on screen
                }
                StartCoroutine(AnimeteSelfMagicCo(target, move, playerOrEnemy));//call AnimeteSelfMagicCo with: the target,the move and bool for player or enemy
            }
            else//if its all targets
            {
                List<int> Targets = new List<int>();//list of targets
                if (playerOrEnemy)//if its player
                {                     
                    for (int i = 0; i < activeBattlers.Count; i++)//for adding the player
                    {
                        if ((activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//add alive only players
                        {
                            Targets.Add(i);
                        }
                    }
                }
                else//if its enemy
                {
                    for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
                    {
                        if (!(activeBattlers[i].isPlayer) && (activeBattlers[i].currentHP > 0))//add alive only enemies
                        {
                            Targets.Add(i);
                        }
                    }
                }
                for (int i = 0; i < Targets.Count; i++)//go in all the targets
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
                    if (move.statusBuff == "HP")//if the buff is heal
                    {
                        float bonus = (float)(activeBattlers[Targets[i]].maxHP * (move.movePower / 100.0));//heal with movePower percent from max
                        activeBattlers[Targets[i]].currentHP += Mathf.RoundToInt(bonus);//the heal itself
                        if (activeBattlers[Targets[i]].currentHP> activeBattlers[Targets[i]].maxHP)//if the currentHP is above the maxHP
                        {
                            activeBattlers[Targets[i]].currentHP = activeBattlers[Targets[i]].maxHP;//fix it
                        }
                    }
                }
                StartCoroutine(AnimeteSelfSpecialCo(Targets, move, playerOrEnemy));//call AnimeteSelfSpecialCo with: the target,the move and bool for player or enemy
            }            
        }
        else if (moveOrItem == true)//if its an item
        {
            if (item.ishpPotion())//if its hp potion
            {
                activeBattlers[target].currentHP += item.ItemHp;//add to current hp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemHp, "Health");//make the heal appear on screen
            }
            else if (item.ismpPotion())//if its mp potion
            {
                activeBattlers[target].currentMP += item.ItemMp;//add to current mp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemMp, "Mana");//make the heal appear on screen
            }
            else if (item.isspPotion())//if its sp potion
            {
                activeBattlers[target].currentSP += item.ItemSp;//add to current sp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemSp, "Special");//make the heal appear on screen
            }
            else if (item.isElixir())//if its elixir(wasn't if the final game because to easy)
            {
                activeBattlers[target].currentHP += item.ItemHp;//add to current hp
                activeBattlers[target].currentMP += item.ItemMp;//add to current mp
                activeBattlers[target].currentSP += item.ItemSp;//add to current sp
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(item.ItemSp);//make the damage heal on screen
            }
            StartCoroutine(AnimeteItemCo(target, item));//call the AnimeteItemCo with: target and the item
        }
        UpdateUIStats();//update the stats
    }
    public IEnumerator AnimeteItemCo(int target, BattleItem item)//a method for item using animation
    {
        activeBattlers[currentTurn].anim.SetBool("Magic_Standby", true);//start the animation
        Instantiate(item.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);//show the effect
        yield return new WaitForSecondsRealtime(item.theEffect.effectLength);//wait for the effect time
        activeBattlers[currentTurn].anim.SetBool("Magic_Standby", false);//stop the animation(and return to idle)
        StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
    }
    public IEnumerator AnimeteAttackSpecialCo(List<int> targets, int damage, BattleMove move,bool playerOrEnemy)//a method for special attack animation
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);//start the animation(the animation itself is the animateName in the move)
        if (move.animateName== "Limt_Atk")//we need to wait only if the move is "Limt_Atk"
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);//wait while the bool effect1 is false
        }
        for (int i = 0; i < targets.Count; i++)//for every target
        {
            Instantiate(move.theEffect, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation);//show the effect
            activeBattlers[targets[i]].currentHP -= damage;//deal damage
            Instantiate(theDamageNumber, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation).SetNotification(damage);//make the damage appear on screen
        }           
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);//wait for the effect time
        if (move.animateName == "Limt_Atk")//we need to wait only if the move is "Limt_Atk"
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);//wait while the bool Idle is false
        }
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);//stop the animation(and return to idle)
        if (playerOrEnemy == true)//if its  player
        {
            StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
        }
        else//if its enemy
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());//call the MoveBackEnemyAndNextTurnCo
        }
    }
    public IEnumerator AnimeteAttckCo(int target , int damage,BattleMove move, bool playerOrEnemy)//a method for attack animation
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);//start the animation(the animation itself is the animateName in the move)
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);//wait while the bool effect1 is false
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);//show the effect
        activeBattlers[target].currentHP -= damage;//deal damage
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);//wait while the bool Idle is false
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);//stop the animation(and return to idle)
        if (playerOrEnemy==true)//if its  player
        {
            StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
        }
        else//if its enemy
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());//call the MoveBackEnemyAndNextTurnCo
        }
    }
    public IEnumerator AnimeteAttackMagicCo(int target, int damage, BattleMove move, bool playerOrEnemy)//a method for magic attack  animation
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);//start the animation(the animation itself is the animateName in the move)
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);//show the effect
        activeBattlers[target].currentHP -= damage;//deal damage
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);//wait for the effect time
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);//stop the animation(and return to idle)
        if (playerOrEnemy == true)//if its  player
        {
            StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
        }
        else//if its enemy
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());//call the MoveBackEnemyAndNextTurnCo
        }
    }
    public IEnumerator AnimeteSelfMagicCo(int target, BattleMove move,bool playerOrEnemy)//a method for support animation
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);//start the animation(the animation itself is the animateName in the move)
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);//show the effect
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);//wait for the effect time
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);//stop the animation(and return to idle)
        if (playerOrEnemy == true)//if its  player
        {
            StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
        }
        else//if its enemy
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());//call the MoveBackEnemyAndNextTurnCo
        }
    }
    public IEnumerator AnimeteSelfSpecialCo(List<int> targets,BattleMove move, bool playerOrEnemy)//a method for support Special animation
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);//start the animation(the animation itself is the animateName in the move)
        if (move.animateName == "Limt_Atk")//we need to wait only if the move is "Limt_Atk"
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);//wait while the bool effect1 is false
        }
        for (int i = 0; i < targets.Count; i++)//for every target
        {
            Instantiate(move.theEffect, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation);//show the effect
            if (move.statusBuff == "HP")//if its a heal then
            {
                Instantiate(theDamageNumber, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation).SetNotification(move.movePower, "Health");//make the heal appear on screen
            }
        }
        yield return new WaitForSecondsRealtime(move.theEffect.effectLength);//wait for the effect time
        if (move.animateName == "Limt_Atk")//we need to wait only if the move is "Limt_Atk"
        {
            yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);//wait while the bool Idle is false
        }
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);//stop the animation(and return to idle)
        if (playerOrEnemy == true)//if its  player
        {
            StartCoroutine(MoveBackAndNextTurnCo());//call the MoveBackAndNextTurnCo
        }
        else//if its enemy
        {
            StartCoroutine(MoveBackEnemyAndNextTurnCo());//call the MoveBackEnemyAndNextTurnCo
        }
    }
    public IEnumerator MoveBackAndNextTurnCo()//a method for player moving back to original position
    {
        activeBattlers[currentTurn].effect1 = false;//reset
        activeBattlers[currentTurn].damageNumbers = false;//reset
        activeBattlers[currentTurn].Idle = false;//reset
        activeBattlers[currentTurn].anim.SetBool("Move", true);//start the move animation       
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, activeBattlers[currentTurn].transform.parent);//move the activeBattler
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != activeBattlers[currentTurn].transform.parent.position);//wait while the activeBattler is not in the original position
        activeBattlers[currentTurn].anim.SetBool("Move", false);//stop the animation(and return to idle)
        BattleMenus.goToMenu(0, 0);//first menu
        BattleMenus.buttonSelect();//select a button
        NextTurn();//next turn
    }
    public IEnumerator MoveBackEnemyAndNextTurnCo()//a method for enemy moving back to original position
    {
        activeBattlers[currentTurn].effect1 = false;//reset
        activeBattlers[currentTurn].damageNumbers = false;//reset
        activeBattlers[currentTurn].Idle = false;//reset
        activeBattlers[currentTurn].anim.SetBool("Move", true);//start the move animation 
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, activeBattlers[currentTurn].transform.parent);//move the activeBattler
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != activeBattlers[currentTurn].transform.parent.position);//wait while the activeBattler is not in the original position
        activeBattlers[currentTurn].anim.SetBool("Move", false);//stop the animation(and return to idle)
        NextTurn();//next turn
    }
    public IEnumerator MoveToAtkPosAndActCo(BattleMove move, BattleItem item, int selectedTarget, bool offense)//a method for moving player to attack position and take action
    {
        activeBattlers[currentTurn].anim.SetBool("Move", true);//start the move animation 
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, playerAcionPosition.transform);//move the activeBattler
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != playerAcionPosition.position);//wait while the activeBattler is not in the attack position
        activeBattlers[currentTurn].anim.SetBool("Move", false);//stop the animation(and return to idle)
        activeBattlers[currentTurn].move = false;//reset
        if (offense == true)//no item
        {
            DealDamage(selectedTarget, move,true);//deal the damage
        }
        else//maybe item
        {
            if (move == null)//if item then
            {
                DealSupport(null, item, selectedTarget, true,true);//call the DealSupport with:null move,item.target,item true,player true..(only player can call MoveToAtkPosAndActCo)
            }
            else if (item == null)//if move then
            {
                DealSupport(move, null, selectedTarget, false,true);//call the DealSupport with:move,null item.target,move true,player true..(only player can call MoveToAtkPosAndActCo)
            }
        }
    }
    public IEnumerator MoveToEnemyAtkPosAndActCo()//a method for moving enemy(bosses) to attack position and take action
    {
        activeBattlers[currentTurn].anim.SetBool("Move", true);//start the move animation 
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, enemyAcionPosition.transform);//move the activeBattler
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != enemyAcionPosition.position);//wait while the activeBattler is not in the attack position
        activeBattlers[currentTurn].anim.SetBool("Move", false);//stop the animation(and return to idle)
        activeBattlers[currentTurn].move = false;//reset
        BossTrun();//next turn
    }
    public void PlayerAction(BattleMove move, BattleItem item, int selectedTarget,bool offense)//a method for player attack
    {
        BattleMenus.offButtons();//turn off the buttons
        StartCoroutine(MoveToAtkPosAndActCo( move,  item,  selectedTarget,  offense));//call the MoveToAtkPosAndActCo with: move,item,target and bool for damage or support 
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
            if(moveOrItem==false)//if move then
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
            else//if item then
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
                    if (magicButtons[i].theMove.isSelfMagic())//if the move is Self Magic then show "%" at the end of the cost
                    {
                        magicButtons[i].costText.text = magicButtons[i].theMove.moveMpCost.ToString()+"%";//show the move mp cost 
                    }
                    else//if not then no "%"
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

                    if (specialButtons[i].theMove.isSelfSpecial())//if the move is Self Special then show "%" at the end of the cost
                    {
                        specialButtons[i].mpCostText.text = specialButtons[i].theMove.moveMpCost.ToString() + "%";//show the move mp cost 
                        specialButtons[i].spCostText.text = specialButtons[i].theMove.moveSpCost.ToString() + "%";//show the move sp cost     
                    }
                    else//if not then no "%"
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
    public IEnumerator EndBattleCo()//a method for ending the battle in victory
    {
        AudioManager.instance.StopMusic();//stop the music
        AudioManager.instance.PlayBGM(6);//start win music
        for (int i = 0; i < activeBattlers.Count; i++)//fore each alive player
        {
            if (activeBattlers[i].currentHP > 0)
            {
                activeBattlers[i].anim.SetBool("Win_Full", true);//start win animation
            }
        }
        if ((bossBattle==true)&&(lastBossBattle == false))//if it was boss battle and not lastBossBattle then 
        {
            GameManager.instance.ElementGet();//call the ElementGet
        }
        else if ((bossBattle == true) && (lastBossBattle == true))//if it was boss battle and lastBossBattle then 
        {
            GameManager.instance.gameBeat=true;//we beat the game
        }
        BattleMenus.goToMenu(0, 0);//reset the menus
        BattleMenus.offMenu(false);//dont show the battle menu
        BattleMenus.ShowVictoryPanel(true);//show the victory menu
        battleActive = false;//reset the battle
        bossBattle = false;//reset every new battle
        impossibleBattle = false;//reset
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));//with until space press
        BattleMenus.ShowVictoryPanel(false);//turn off the victory menu
        yield return new WaitForSeconds(.5f);//wait for 0.5 second
        battleCanves.SetActive(false);//turn off the canves
        FadeManager.instance.BattleTransition("FadeBlack");//start the fade in effect with FadeBlack
        for (int i = 0; i < activeBattlers.Count; i++)//for each activeBattlers
        {
            if (activeBattlers[i].isPlayer)//if its player
            {
                for (int j = 0; j < GameManager.instance.playerStats.Length; j++)//go on all the playerStats in GameManager
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)//find the right playerStats
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].maxHP;//reset to health
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].maxMP;//reset to MP
                        GameManager.instance.playerStats[j].currentSP = activeBattlers[i].currentSP;//keep the sp for next battle
                    }
                }
            }
            Destroy(activeBattlers[i].gameObject);
        }
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//wait until the screen is black
        PlayerController.instance.MyRigidbody.constraints = RigidbodyConstraints2D.None;//enable the player to move
        PlayerController.instance.MyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;//freeze the Rotation
        BattleMenus.theInfoHolder.SetActive(true);//show the info holder
        battleCanves.SetActive(false);//turn off the canves
        battleScene.SetActive(false);//turn off the holder
        activeBattlers.Clear();//clear the list
        currentTurn = 0;//reset the currentTurn
        yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);//with until the screen is back
        GameManager.instance.battleActive = false;//tell GameManager that the battle is over
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);//start the right music for the scene     
    }
    public IEnumerator GameOverCo()//a method for ending the battle in gameover
    {
        AudioManager.instance.StopMusic();//stop the battle music
        AudioManager.instance.PlaySFX(0);//start the death music
        battleActive = false;//reset
        bossBattle = false;//reset
        impossibleBattle = false;//reset
        battleCanves.SetActive(false);//turn off the canves
        FadeManager.instance.ScenenTransition("GameOver");//fade in with the GameOver effect
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//with until the screen is black
        GameManager.instance.battleActive = false;//tell GameManager that the battle is over
        battleScene.SetActive(false);//turn of the battle scene
        for (int i = 0; i < activeBattlers.Count; i++)//for each activeBattlers
            Destroy(activeBattlers[i].gameObject);//destroy it
        activeBattlers.Clear();//clear the activeBattlers list
        SceneManager.LoadScene("GameOver");//go to GameOver scene


    }
}

