using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

    public string charName;
    public int playerLevel;

    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int currentSP;
    public int maxSP;
    public int strength;
    public int defense;
    public int dexterity;
    public List<BattleMove> movesAvailable = new List<BattleMove>();
    public List<BattleMove> movesSet1 = new List<BattleMove>();
    public List<BattleMove> movesSet2 = new List<BattleMove>();
    public List<BattleMove> movesSet3 = new List<BattleMove>();
    public List<BattleMove> movesSet4 = new List<BattleMove>();
    //public bool[] hasElement;
    //public int[][] bonusElement;
    public Sprite charIamge;



    // Use this for initialization
    void Start()
    {
        //hasElement = new bool[3];
        /*bonusElement = new int[3][];
        for (int i = 0; i < bonusElement.Length; i++)
        {
            bonusElement[i] = new int[7];
            for (int j = 0; j < bonusElement[i].Length; j++)
            {
                bonusElement[i][j] = 25;
            }
        }
        Debug.Log(bonusElement[0][1]);*/

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddBonusElementStats(int [] bonusElement)
    {
        playerLevel += bonusElement[0];
        maxHP += bonusElement[1];
        maxMP += bonusElement[2];
        maxSP += bonusElement[3];
        currentHP = maxHP;
        currentMP = maxMP;
        currentSP = maxSP;
    }
}
