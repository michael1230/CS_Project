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
    public List<BattleMove> movesToAdd = new List<BattleMove>();//later
    public bool[] hasElement;
    public int[][] bonusElement;
    public Sprite charIamge;



    // Use this for initialization
    void Start()
    {
        hasElement = new bool[3];
        bonusElement = new int[3][];
        for (int i = 0; i < bonusElement.Length; i++)
        {
            bonusElement[i] = new int[7];
            for (int j = 0; j < bonusElement[i].Length; j++)
            {
                bonusElement[i][j] = 25;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddBonusElementStats(int whichElement)
    {
        playerLevel += bonusElement[whichElement][0];
        maxHP += bonusElement[whichElement][1];
        maxMP += bonusElement[whichElement][2];
        maxSP += bonusElement[whichElement][3];
        strength += bonusElement[whichElement][4];
        defense += bonusElement[whichElement][5];
        dexterity += bonusElement[whichElement][6];
        hasElement[whichElement] = true;
        currentHP = maxHP;
        currentMP = maxMP;
        currentSP = maxSP;
    }
    public void AddBonusElementMoves(int whichElement)//later
    {

    }
}
