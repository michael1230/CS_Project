using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

    public string charName;
    public int playerLevel ;
    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int strength;
    public int defense;
    public int dexterity;
    public bool[] hasElement;
    public int[][] bonusElement;
    //public MultiDArrays[] bonusElement;
    public Sprite charIamge;



    // Use this for initialization
    void Start()
    {
        hasElement = new bool[3];
        bonusElement = new int[3][];
        for (int i = 0; i < bonusElement.Length; i++)
        {
            bonusElement[i] = new int[6];
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

    public void AddBonusElement(int whichElement)
    {
        playerLevel +=  bonusElement[whichElement][0];
        maxHP +=        bonusElement[whichElement][1];
        maxMP +=        bonusElement[whichElement][2];
        strength +=     bonusElement[whichElement][3];
        defense +=      bonusElement[whichElement][4];
        dexterity +=    bonusElement[whichElement][5];
        hasElement[whichElement] = true;
        currentHP = maxHP;
        currentMP = maxMP;
    }

       
}
