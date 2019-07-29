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
    public int itemIndex;//the index of the item button   moveTargetAll
    public bool moveTargetAll;//is this item affect all the party or not(was not in the final project because it was to easy)
    public AttackEffect theEffect;//the effect itself

    public bool ishpPotion()//a method for knowing if the item is hpPotion
    {
        if (theType == itemType.hpPotion)
            return true;
        return false;
    }
    public bool ismpPotion()//a method for knowing if the item is mpPotion
    {
        if (theType == itemType.mpPotion)
            return true;
        return false;
    }
    public bool isspPotion()//a method for knowing if the item is spPotion
    {
        if (theType == itemType.spPotion)
            return true;
        return false;
    }
    public bool isElixir()//a method for knowing if the item is Elixir(was not in the final project because it was to easy)
    {
        if (theType == itemType.Elixir)
            return true;
        return false;
    }
}
