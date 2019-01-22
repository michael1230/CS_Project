using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpecialSelect : MonoBehaviour {

    public string   artName;
    public int      artCost;
    public Text     nameText;
    public Text     costText;

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
       // BattleManager.instance.magicMenu.SetActive(false);/////////////////
        //BattleManager.instance.OpenTargetMenu(artName);
        //BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= artCost;
    }
}
