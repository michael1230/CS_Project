using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttackSelect : MonoBehaviour
{

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
        BattleManager.instance.OpenTargetMenu(theMove,4);
        BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= theMove.moveSpCost;
    }
}
