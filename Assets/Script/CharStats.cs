using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;//the name
    public float playerLevel;//the level
    public int currentHP;//the current HP
    public int maxHP;//the max HP
    public int currentMP;//the current MP
    public int maxMP;//the max MP
    public int currentSP;//the current SP
    public int maxSP;//the max SP
    public int strength;// the strength..the attack power
    public int defense;//the defense..the defense power
    public int dexterity;//didn't use in the final game
    public int[] initialStats;//the initial stats
    public List<BattleMove> movesAvailable = new List<BattleMove>();//a list of moves thats available now
    public List<BattleMove> movesSet1 = new List<BattleMove>();//first moves set
    public List<BattleMove> movesSet2 = new List<BattleMove>();//second moves set
    public List<BattleMove> movesSet3 = new List<BattleMove>();//third moves set
    public List<BattleMove> movesSet4 = new List<BattleMove>();//forte moves set
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void AddBonusElementStats(int [] bonusElement)//a method for adding the bonus when getting new element
    {
        playerLevel += bonusElement[0];
        maxHP += bonusElement[1];
        maxMP += bonusElement[2];
        maxSP += bonusElement[3];
        currentHP = maxHP;
        currentMP = maxMP;
        currentSP = maxSP;
    }
    public void ResetStats()//a method for reseting the stats..for new game
    {
        playerLevel = initialStats[0];
        maxHP = initialStats[1];
        maxMP = initialStats[2];
        maxSP = initialStats[3];
        currentHP = maxHP;
        currentMP = maxMP;
        currentSP = maxSP;
    }

}
