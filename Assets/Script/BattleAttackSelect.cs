using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttackSelect : MonoBehaviour
{
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
        BattleManager.instance.OpenTargetMenu(theMove,4);//open the target select menu
    }
}
