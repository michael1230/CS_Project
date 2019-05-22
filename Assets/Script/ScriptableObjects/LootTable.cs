﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public Powerup thisLoot;//heart or fireBall
    public int lootChance;
}

//Data storage
[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;//array of all objects

    public Powerup LootPowerup()
    {
        int cumProb = 0;//cumulative probability
        int currentProb = Random.Range(0, 100);//current probability from 0 to 100 percent
        for (int i = 0; i < loots.Length; i++)
        {
            cumProb += loots[i].lootChance;//adds probabillity from unity the chance for a loot
            if (currentProb <= cumProb)//if bigger then we give it
            {
                return loots[i].thisLoot;
            }
        }
        return null;//for returning nothing
    }
}