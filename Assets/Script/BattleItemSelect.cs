using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleItemSelect : MonoBehaviour
{
    public BattleItem theItem;
    public int itemIndex;
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
        BattleManager.instance.OpenSelfMenu(null,theItem,3);
        GameManager.instance.totalItems[itemIndex].ItemAmount--;

    }
}
