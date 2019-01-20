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

    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI currentMenuText;

    public BattleMove[] movesList;
    //public GameObject enemyAttackEffect;

    public Text[] hpText;
    public Text[] mpText;
    public Slider[] hpSlider;
    public Slider[] mpSlider;
    public Image[] charImage;


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


        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    battleMenuHolder.SetActive(true);
                }
                else
                {
                    battleMenuHolder.SetActive(false);

                    //enemy should attack
                     StartCoroutine(EnemyMoveCo());
                }
            }

            if (Input.GetKeyDown(KeyCode.N))///for test!!
            {
                NextTurn();
            }
        }

    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if (!battleActive)
        {
            battleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            GameManager.instance.battleActive = true;
            //AudioManager.instance.PlaySFX(8);
            AudioManager.instance.PlayBGM(8);
            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].dexterity = thePlayer.dexterity;

                        }
                    }


                }
            }
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
            turnWaiting = true;
            currentTurn = 0;

        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

         UpdateBattle();
        // UpdateUIStats();
    }

    public void UpdateBattle()
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
        /*else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }*/
    }

    public IEnumerator EnemyMoveCo()//wait for enemy
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
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

        //activeBattlers[selectedTarget].currentHP -= 70;//for test

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);//randome select an attack of the enemy
       // int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])//if the enemy has the attack we selected
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);//make the effect appear on the target
                //movePower = movesList[i].movePower;
                activeBattlers[selectedTarget].currentHP -= movesList[i].movePower;
            }
        }

       // Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

       // DealDamage(selectedTarget, movePower);
    }
}

