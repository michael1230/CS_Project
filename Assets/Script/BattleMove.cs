using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleMove : MonoBehaviour
{
    public enum moveType { Attack, AttackSpecial, SelfSpecial, SelfMagic, AttackMagic };
    public moveType theType= moveType.Attack;//////////////
    public string moveName;//the name of the move
    public string statusBuff;//the name of the move
    public string animateName;//the name of the move
    public int movePower;// the power(for damage) 
    public int moveMpCost;//how much it cost
    public int moveSpCost;//how much it cost
    public bool moveTargetAll;//how much it cost
    public AttackEffect theEffect;//the effect itself

    public void copy(BattleMove other)
    {
        this.theType = other.theType;
        this.moveName = other.moveName;
        this.movePower = other.movePower;
        this.moveMpCost = other.moveMpCost;
        this.moveSpCost = other.moveSpCost;
        this.theEffect = other.theEffect;
    }


    public bool isAttck()
    {
        if (theType == moveType.Attack)
            return true;
        return false;
    }
    public bool isAttackSpecial()
    {
        if (theType == moveType.AttackSpecial)
            return true;
        return false;
    }
    public bool isSelfSpecial()
    {
        if (theType == moveType.SelfSpecial)
            return true;
        return false;
    }
    public bool isSelfMagic()
    {
        if (theType == moveType.SelfMagic)
            return true;
        return false;
    }
    public bool isAttackMagic()
    {
        if (theType == moveType.AttackMagic)
            return true;
        return false;
    }
    /*
    public bool isEmpthy()
    {
        if (this == null)
            return true;
        return false;
    }
    */
}
