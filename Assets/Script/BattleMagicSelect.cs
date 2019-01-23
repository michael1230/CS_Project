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
            BattleManager.instance.magicMenu.SetActive(false);
            if (theMove.isAttackMagic())
            {
                BattleManager.instance.OpenTargetMenu(theMove);
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;
            }
            else if(theMove.isSelfMagic())
            {
                BattleManager.instance.OpenSelfMenu(theMove,null);
                BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= theMove.moveMpCost;
            }

    }
}
