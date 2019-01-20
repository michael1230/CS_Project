using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleMove
{
    public string moveName;//the name of the move
    public int movePower;// the power(for damage)
    public int moveCost;//how much it cost
    public AttackEffect theEffect;//the effect itself
}
