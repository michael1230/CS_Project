﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    public BattleMove theMove;
    public BattleItem theItem;
    public int activeBattlerTarget;//the target we selected 
    public Text targetName;//the name of the target
    public bool moveOrItem;/////////////////////////////////////////////////////true move
    // public Button theButton;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void Press()// true no item 
    {
        if (moveOrItem==true)
        {
            if ((theMove.isAttackMagic()) || (theMove.isAttackSpecial()) || (theMove.isAttck()))
            {
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, true);
            }
            else if ((theMove.isSelfMagic()) || (theMove.isSelfSpecial()))
            {
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);
            }
        }
        else
        {
            BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);
        }
    }
}
