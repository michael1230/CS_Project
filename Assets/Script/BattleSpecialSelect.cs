using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpecialSelect : MonoBehaviour {

    public BattleMove theMove;//the move object
    public Text nameText;//the text for the name
    public Text mpCostText;//the text for the move mp cost
    public Text spCostText;//the text for the move sp cost

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
        if (theMove.isAttackSpecial())//if the move is AttackSpecial
        {
            BattleManager.instance.OpenTargetMenu(theMove,2);//open the target select menu
        }
        else if(theMove.isSelfSpecial())//if the move is SelfSpecial
        {
            BattleManager.instance.OpenSelfMenu(theMove,null,2, false);//open the self target select menu
        }               
    }
}
