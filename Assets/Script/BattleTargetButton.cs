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
    public bool moveOrItem;/////////////////////////////////////////////////////false move
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
        if (moveOrItem==false)
        {
            if ((theMove.isAttackMagic()) || (theMove.isAttackSpecial()) || (theMove.isAttck()))
            {
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= theMove.moveSpCost;
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, true);
            }
            else if ((theMove.isSelfMagic()) || (theMove.isSelfSpecial()))
            {
                float mpCost= (float)(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].maxMP * (theMove.moveMpCost / 100.0));
                float spCost = (float)(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].maxSP * (theMove.moveSpCost / 100.0));
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= Mathf.RoundToInt(mpCost);
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= Mathf.RoundToInt(spCost);
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);
            }
        }
        else
        {
            GameManager.instance.totalItems[theItem.itemIndex].ItemAmount--;
            BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);
        }
    }
}
