using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour {

    public BattleMove theMove;
    public Text nameText;
    public Text costText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Press()
    {
        if (theMove.isAttackMagic())
        {
            BattleManager.instance.OpenTargetMenu(theMove,1);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;
        }
        else if (theMove.isSelfMagic())
        {
            BattleManager.instance.OpenSelfMenu(theMove, null,1,true);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;
        }

    }
}
