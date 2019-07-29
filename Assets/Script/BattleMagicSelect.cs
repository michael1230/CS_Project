using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour {

    public BattleMove theMove;//the move object
    public Text nameText;//the text for the name
    public Text costText;//the text for the move cost

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
        if (theMove.isAttackMagic())//if the move is AttackMagic
        {
            BattleManager.instance.OpenTargetMenu(theMove,1);//open the target select menu
        }
        else if (theMove.isSelfMagic())//if the move is SelfMagic
        {
            BattleManager.instance.OpenSelfMenu(theMove, null,1,false);//open the self target select menu
        }

    }
}
