using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public BattleItem theItem;//the item object
    public Text nameText;//text for the name
    public Text amountText;//text for the amount
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
        BattleManager.instance.OpenSelfMenu(null,theItem,3,true);//open the self target select menu
    }
}
