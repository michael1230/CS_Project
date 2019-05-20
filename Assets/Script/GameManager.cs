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

    public CharStats[] playerStats;//work on activate playerStats if the number of element!!!!

    public BattleItem[] totalItems;

    public int numberOfElement = 0;
    public int activePartyMemberIndex = 1;

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

        /* playerStats[1].gameObject.SetActive(false);//for test is comma when game is ready delete comme and keep the lines!!!!
         playerStats[2].gameObject.SetActive(false);
         playerStats[3].gameObject.SetActive(false);*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || battleActive|| gameOver)
        {
            PlayerController.instance.canMovePlayer = false;
        }
        else
        {
            PlayerController.instance.canMovePlayer = true;
        }
     
        if (Input.GetKeyDown(KeyCode.T))//for test
        {
            addElement();
        }
        if (Input.GetKeyDown(KeyCode.Y))//for test
        {
            addPartyMember();
        }

    }

    public void ElementGet( int index)
    {
        //a bool for who was defeated 
        gotElement[index] = true;
        addElement();
    }

    public void addElement()
    {
        if (numberOfElement == 0)//add the first element (now we have two) need to add moves!
        {

            for (int i = 0; i < playerStats.Length; i++)
            {
                playerStats[i].AddBonusElementStats(bonusSecondElement);
             ;
            }
            numberOfElement++;
            addPartyMember();
            //AddBonusElementMoves need to add moves!
        }
        else if (numberOfElement ==1)
        {

            for (int i = 0; i < playerStats.Length; i++)
            {
                playerStats[i].AddBonusElementStats(bonusThirdElement);
                ;
            }
            numberOfElement++;
            addPartyMember();
            //AddBonusElementMoves need to add moves!
        }
        else if (numberOfElement ==2)
        {

            for (int i = 0; i < playerStats.Length; i++)
            {
                playerStats[i].AddBonusElementStats(bonusFourthElement);
                ;
            }
            numberOfElement++;
            addPartyMember();
            //AddBonusElementMoves need to add moves!
        }
    }
    public void addPartyMember()
    {
        if(activePartyMemberIndex<4)
        {
            playerStats[activePartyMemberIndex].gameObject.SetActive(true);
            activePartyMemberIndex++;
        }
    }

}
