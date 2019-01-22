using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttackSelect : MonoBehaviour
{

    public string attackName;
    public int attackCost;
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
        //BattleManager.instance.magicMenu.SetActive(false);
        //BattleManager.instance.OpenTargetMenu(spellName);
        //BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
    }
}
