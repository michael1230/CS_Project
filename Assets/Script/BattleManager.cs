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
    public GameObject battleCanves;
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public Transform playerAcionPosition;

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
        }
    }

    public IEnumerator PrepareforBattleStart(string[] enemiesToSpawn)//a method for activate the fade effect and then go to battle
    {
        GameManager.instance.battleActive = true;//rise the flag for GameManager
        FadeManager.instance.BattleTransition("Battle");
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySFX(10);
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        BattleStart(enemiesToSpawn);
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);//put the camera on the battle
        battleCanves.SetActive(false);
        battleScene.SetActive(true);//show the battleScene
        yield return new WaitUntil(() => FadeManager.instance.finishedTransition == true);
        battleCanves.SetActive(true);
    }
    public void BattleStart(string[] enemiesToSpawn)//a method for staring the battle(only runs once per battle)//add info player activation on gamemanager!
    {
        if (!battleActive)//if the battleActive is false
        {            
            battleActive = true;//make it true
            AudioManager.instance.PlayBGM(8);//turn on the battle music
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
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].currentSP = thePlayer.currentSP;
                            activeBattlers[i].maxSP = thePlayer.maxSP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].dexterity = thePlayer.dexterity;
                            activeBattlers[i].movesAvailable = thePlayer.movesAvailable;
                        }
                    }
                }
            }
            for (int i = 0; i < enemiesToSpawn.Length; i++)//boss position check needs to add!, add all enemy prefab to this!!!!!!!!!!!!!!!!!!
            {
                if (enemiesToSpawn[i] != "")//if the enemy name is not empty
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
                    ///StatusBuffsCheck(playerData);////
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
        StatusBuffsCheck(activeBattlers[currentTurn]);//check the status here
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
                
                  if (activeBattlers[i].isPlayer)
                    {
                    activeBattlers[i].anim.SetBool("Dying", true);
                    }             
                  else if(activeBattlers[i].isRegularEnemy)
                    {
                        activeBattlers[i].EnemyFade();
                    }

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
                 StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                // StartCoroutine(GameOverCo());
            }

            //battleScene.SetActive(false);
            //GameManager.instance.battleActive = false;
            //battleActive = false;
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
    public void EnemyAttack()// a method for enemy attack//// later for boss/////////////sp add////////////////////
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
        DealDamage(selectedTarget, enemyMove);//deal the damage
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
                playerData.bounusTurn[j]--;//mainus this turn
            }
        }
    }
    public void StatusBuffs(BattleMove move, int target)//a method to apply status buffs
    {
        if (move.moveTargetAll == false)//if the move is not on all targets
        {
            if (move.statusBuff == "Attack")//if the buff is Attack
            {
                activeBattlers[target].bounusTurn[0] = 3;//the bonus turn time is will last
                activeBattlers[target].statusBounus[0] = move.movePower;//the bonus itself
            }
            else if (move.statusBuff == "Defense")//if the buff is Defense
            {
                activeBattlers[target].bounusTurn[1] = 3;//the bonus turn time is will last
                activeBattlers[target].statusBounus[1] = move.movePower;//the bonus itself
            }
            StartCoroutine(animeteSelfMagicCo(target, move));///
        }
        else//all target
        {
            for (int i = 0; i < activeBattlers.Count; i++)//go in all the battlers
            {
                if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)//if it player and still alive
                {
                    if (move.statusBuff == "Attack")//if the buff is Attack
                    {
                        activeBattlers[i].bounusTurn[0] = 3;//the bonus turn time is will last
                        activeBattlers[i].statusBounus[0] = move.movePower;//the bonus itself
                    }
                    else if (move.statusBuff == "Defense")//if the buff is Defense
                    {
                        activeBattlers[i].bounusTurn[1] = 3;//the bonus turn time is will last
                        activeBattlers[i].statusBounus[1] = move.movePower;//the bonus itself
                    }
                }
            }
            StartCoroutine(animeteSelfSpecialCo(move));//
        }
    }
    public void DealDamage(int target, BattleMove move)//later to add miss, critical, res , magic? 
    {
        int damageToGive = 0;
        if (move.moveTargetAll == false)//if the move is only for one enemy
        {
            float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0];
            float defPwr = activeBattlers[target].defense + activeBattlers[target].statusBounus[1];
            float damageCalc = (atkPwr / defPwr) * move.movePower;
            damageToGive = Mathf.RoundToInt(damageCalc);
            if (activeBattlers[currentTurn].isPlayer)//or any kind of boss...for later to anim the bosses
            {
                if (move.isAttck())
                {
                    if(move.numOfSFXs=="1")
                    {
                        StartCoroutine(animeteAttckCo(target, damageToGive, move));
                    }
                    else if (move.numOfSFXs == "2")
                    {
                        StartCoroutine(animeteAttckCo2(target, damageToGive, move));
                    }
                }
                else if (move.isAttackMagic())
                {
                    StartCoroutine(animeteAttackMagicCo(target, damageToGive, move));
                }
            }
            else if (activeBattlers[currentTurn].isRegularEnemy == true)//for regular enemy numbers aka no anim
            {
                Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damageToGive);//make the damage appear on screen
                activeBattlers[target].currentHP -= move.movePower;
            }
        }////////////later add move for all palyer also!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        else//if the move is for all enemies 
        {//maybe another formula later
            float defPwr = 0;
            float atkPwr = 0;
            List<int> Enemies = new List<int>();//list of enemies
            for (int i = 0; i < activeBattlers.Count; i++)//for adding the enemies
            {
                if ((!activeBattlers[i].isPlayer)&&(activeBattlers[i].currentHP>0))
                {
                    Enemies.Add(i);
                    defPwr += (activeBattlers[i].defense + activeBattlers[i].statusBounus[1]);//add all of the defense of all the enemies
                }
            }
            atkPwr = (activeBattlers[currentTurn].strength + activeBattlers[currentTurn].statusBounus[0]) * Enemies.Count;//the strength of the current player times the number of enemies
            float damageCalc = (atkPwr / defPwr) * move.movePower;
            damageToGive = Mathf.RoundToInt(damageCalc);//to int
            StartCoroutine(animeteAttackSpecialCo(Enemies, damageToGive, move));
        }
        UpdateUIStats();//update the stats
        //Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);//for test
    }
    public void Dealdefense(BattleMove move, BattleItem item, int target, bool moveOrItem)//later to add miss, critical, res , magic? 
    {
        if (moveOrItem == false)//move
        {
            if (move.statusBuff == "HP")//if its a hp refile move then
            {
                if (move.moveTargetAll == false)
                {
                    activeBattlers[target].currentHP += move.movePower;
                    Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(move.movePower, "Health");//make the damage appear on screen
                    StartCoroutine(animeteSelfMagicCo(target, move));//
                }
                else
                {                  
                    StartCoroutine(animeteSelfSpecialCo(move));//
                }
            }
            else if (move.statusBuff != "")
            {
                StatusBuffs(move, target);
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
        }
        UpdateUIStats();//update the stats
        StartCoroutine(waitBeforeBackCo());
    }
    public IEnumerator animeteAttackSpecialCo(List<int> targets, int damage, BattleMove move)//for Attacking all enemy.. one effect 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        for (int i = 0; i < targets.Count; i++)//for every enemy
        {
            Instantiate(move.theEffect, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation);
            activeBattlers[targets[i]].currentHP -= damage;//take hp
            Instantiate(theDamageNumber, activeBattlers[targets[i]].transform.position, activeBattlers[targets[i]].transform.rotation).SetNotification(damage);//make the damage appear on screen
        }           
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator animeteAttckCo(int target , int damage,BattleMove move)//for Attacking one enemy.. one effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        activeBattlers[target].currentHP -= damage;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator animeteAttckCo2(int target, int damage, BattleMove move)//for Attacking one enemy.. two effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect2 == false);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        activeBattlers[target].currentHP -= damage;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator animeteAttackMagicCo(int target, int damage, BattleMove move)//for Attacking one enemy.. skill effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        activeBattlers[target].currentHP -= damage;//take hp
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetNotification(damage);//make the damage appear on screen
        yield return new WaitForSeconds(move.theEffect.effectLength);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        //NextTurn();
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator animeteSelfMagicCo(int target, BattleMove move)//for support one ally.. skill effects 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);       
        yield return new WaitForSeconds(move.theEffect.effectLength);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator animeteSelfSpecialCo(BattleMove move)//for support all enemy.. one effect 
    {
        activeBattlers[currentTurn].anim.SetBool(move.animateName, true);
        yield return new WaitWhile(() => activeBattlers[currentTurn].effect1 == false);
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                Instantiate(move.theEffect, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation);
                if (move.statusBuff == "HP")
                {
                    activeBattlers[i].currentHP += move.movePower;
                    Instantiate(theDamageNumber, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation).SetNotification(move.movePower, "Health");//make the damage appear on screen
                }
            }
        }
        yield return new WaitForSeconds(move.theEffect.effectLength);
        yield return new WaitWhile(() => activeBattlers[currentTurn].Idle == false);
        activeBattlers[currentTurn].anim.SetBool(move.animateName, false);
        StartCoroutine(moveBackAndNextTurnCo());
    }
    public IEnumerator moveBackAndNextTurnCo()//for moving back to original position
    {
        activeBattlers[currentTurn].effect1 = false;
        activeBattlers[currentTurn].effect2 = false;
        activeBattlers[currentTurn].Idle = false;
        int index = 0;
        activeBattlers[currentTurn].anim.SetBool("Move", true);       
        if (activeBattlers[currentTurn].charName=="Rain")
        {
            index = 0;
        }
        else if (activeBattlers[currentTurn].charName == "Roselia")
        {
            index = 1;
        }
        else if (activeBattlers[currentTurn].charName == "Aiden")
        {
            index = 2;
        }
        else if (activeBattlers[currentTurn].charName == "Sakura")
        {
            index = 3;
        }
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, playerPositions[index].transform);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != playerPositions[index].transform.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        BattleMenus.goToMenu(0, 0);
        BattleMenus.buttonSelect();
        NextTurn();
    }
    public IEnumerator waitBeforeBackCo()//wait for enemy
    {
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator moveToAtkPosAndActCo(BattleMove move, BattleItem item, int selectedTarget, bool offense)
    {
        activeBattlers[currentTurn].anim.SetBool("Move", true);       
        activeBattlers[currentTurn].moveToPostion(activeBattlers[currentTurn].transform, playerAcionPosition.transform);
        yield return new WaitWhile(() => activeBattlers[currentTurn].transform.position != playerAcionPosition.position);
        activeBattlers[currentTurn].anim.SetBool("Move", false);
        activeBattlers[currentTurn].move = false;
        if (offense == true)//no item
        {
            DealDamage(selectedTarget, move);//deal the damage
        }
        else//maybe item
        {
            if (move == null)
            {
                //Instantiate(item.theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                Dealdefense(null, item, selectedTarget, true);
            }
            else if (item == null)
            {
                //Instantiate(move.theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                Dealdefense(move, null, selectedTarget, false);
            }
        }
    }
    public void PlayerAction(BattleMove move, BattleItem item, int selectedTarget,bool offense)//a method for player attack//////////////add animation //////////////
    {
        BattleMenus.offButtons();
        StartCoroutine(moveToAtkPosAndActCo( move,  item,  selectedTarget,  offense));       
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
                    magicButtons[i].costText.text = magicButtons[i].theMove.moveMpCost.ToString();//show the move mp cost 
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
                    specialButtons[i].mpCostText.text = specialButtons[i].theMove.moveMpCost.ToString();//show the move mp cost 
                    specialButtons[i].spCostText.text = specialButtons[i].theMove.moveSpCost.ToString();//show the move sp cost 
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
        battleActive = false;
        BattleMenus.goToMenu(0, 0);
        BattleMenus.offMenu(false);
        GameManager.instance.battleActive = false;
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
        battleCanves.SetActive(false);
        battleScene.SetActive(false);
        
        activeBattlers.Clear();
        currentTurn = 0;
        /*if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardScreen(rewardXP, rewardItems);
        }*/

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }


}

