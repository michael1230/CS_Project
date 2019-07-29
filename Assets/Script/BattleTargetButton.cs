using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    public BattleMove theMove;//the move object
    public BattleItem theItem;//the item object
    public int activeBattlerTarget;//the target we selected 
    public Text targetName;//the text for the name
    public bool moveOrItem;//a bool for knowing if move or item (false move)
    // Use this for initialization
    void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {

    }
    public void Press()
    {
        if (moveOrItem==false)//if false then its move so
        {
            if ((theMove.isAttackMagic()) || (theMove.isAttackSpecial()) || (theMove.isAttck()))//if its Attack then
            {
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;//subtract moveMpCost from the currentMP
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= theMove.moveSpCost;//subtract moveSpCost from the currentSP
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, true);//call the PlayerAction method
            }
            else if ((theMove.isSelfMagic()) || (theMove.isSelfSpecial()))//if its Self then
            {
                float mpCost= (float)(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].maxMP * (theMove.moveMpCost / 100.0));//the moveMpCost percent from the maxMP
                float spCost = (float)(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].maxSP * (theMove.moveSpCost / 100.0));//the moveSpCost percent from the maxSP
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= Mathf.RoundToInt(mpCost);//subtract mpCost from the currentMP
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= Mathf.RoundToInt(spCost);//subtract spCost from the currentSP
                BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);//call the PlayerAction method
            }
        }
        else//else then its item so
        {
            GameManager.instance.totalItems[theItem.itemIndex].ItemAmount--;//subtract 1 from the ItemAmount
            BattleManager.instance.PlayerAction(theMove, theItem, activeBattlerTarget, false);//call the PlayerAction method
        }
    }
}
