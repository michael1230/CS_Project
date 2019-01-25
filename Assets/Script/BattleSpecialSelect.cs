using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpecialSelect : MonoBehaviour {

    public BattleMove theMove;
    public Text     nameText;
    public Text     mpCostText;
    public Text     spCostText;

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
        //BattleManager.instance.specialMenu.SetActive(false);

        if (theMove.isAttackSpecial())
        {
            BattleManager.instance.OpenTargetMenu(theMove,2);
        }
        else if(theMove.isSelfSpecial())
        {
            BattleManager.instance.OpenSelfMenu(theMove,null,2, false);
        }
        
        
    }
}
