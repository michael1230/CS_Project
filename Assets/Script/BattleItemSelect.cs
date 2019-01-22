using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public string itemName;
    public int itemCost;
    public Text nameText;
    public Text amountText;

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
       // BattleManager.instance.magicMenu.SetActive(false);////////////////////
        //BattleManager.instance.OpenTargetMenu(artName);
        //BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= artCost;
    }
}
