using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{

    public string charName;
    public int playerLevel = 1;
    public int currentHP=150;
    public int maxHP = 150;
    public int currentMP=50;
    public int maxMP = 50;
    public int strength=30;
    public int defense = 30;
    public int dexterity = 10;
    public bool hasFirstElement;
    public bool hasSecondElement;
    public bool hasThirdElement;
    public int[] bonusFirstElement;
    public int[] bonusSecondElement;
    public int[] bonusThirdElement;

    public Sprite charIamge;




    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddElement(int whichElement)
    {
        if(whichElement==1)
        {
            playerLevel += bonusFirstElement[0];
            maxHP += bonusFirstElement[1];
            maxMP += bonusFirstElement[2];
            strength += bonusFirstElement[3];
            defense += bonusFirstElement[4];
            dexterity+= bonusFirstElement[5];
        }
        else if (whichElement == 2)
        {
            playerLevel += bonusSecondElement[0];
            maxHP += bonusSecondElement[1];
            maxMP += bonusSecondElement[2];
            strength += bonusSecondElement[3];
            defense += bonusSecondElement[4];
            dexterity += bonusSecondElement[5];
        }
        else if (whichElement == 3)
        {
            playerLevel += bonusThirdElement[0];
            maxHP += bonusThirdElement[1];
            maxMP += bonusThirdElement[2];
            strength += bonusThirdElement[3];
            defense += bonusThirdElement[4];
            dexterity += bonusThirdElement[5];
        }


    }
}
