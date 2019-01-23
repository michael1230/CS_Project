using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleItem : MonoBehaviour
{
    public enum itemType { hpPotion, mpPotion, spPotion, Elixir };
    public itemType theType;
    public string ItemName;//the name of the move
    public int ItemAmount;// the power(for damage) 
    public int ItemHp;//how much it cost
    public int ItemMp;//how much it cost
    public int ItemSp;//how much it cost
    public AttackEffect theEffect;//the effect itself

    public bool ishpPotion()
    {
        if (theType == itemType.hpPotion)
            return true;
        return false;
    }
    public bool ismpPotion()
    {
        if (theType == itemType.mpPotion)
            return true;
        return false;
    }
    public bool isspPotion()
    {
        if (theType == itemType.spPotion)
            return true;
        return false;
    }
    public bool isElixir()
    {
        if (theType == itemType.Elixir)
            return true;
        return false;
    }
}
