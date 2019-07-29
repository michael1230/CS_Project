using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameData 
{
    private string sceneName;//the name of the active scene
    private int numOfElement;//the number of element we have collected..its also tells the party size and the Statbonus to add
    private bool enemyOnMap;//if we have enemies on this map
    private bool bossOnMap;//if we have boss on this map
    private int[] itemsAmount;//the amount of the items..0-hp,1-mp,2-sp
    private float[] playerPos;//the pos of the player..0-x,1-y,2-z 
    private float[] bossPos;//the pos of the Boss..0-x,1-y,2-z
    private int numOfEnemies;//the number of enemies on this map
    private bool[] deadOrAliveEnemy;//the number of enemies on this map
    private bool[] elementGot;//the number of element we have
    private float[,] enemiesPos;//the pos of the enemies..first index is the enemy number second index is the pos: 0-x,1-y,2-z
    private float[] playersLevel;//the level of the players
    private int[] playersHP;//the hp of the players
    private int[] playersMP;//the mp of the players
    private int[] playersSP;//the sp of the players
    private float bossSpeed;//the number of enemies on this map
    private bool bossActive;//if we have enemies on this map
    private float heartContainers;
    private float currentHealthInitialValue;
    private float currentHealthRuntimeValue;
    private float maxMagic;
    private float currentMagic;
    private int[] currentHearts;

    public string SceneName //getter and setter
    {
        get
        {
            return this.sceneName;
        }
        set
        {
            this.sceneName = value;
        }
    }
    public int NumOfElement//getter and setter
    {
        get
        {
            return this.numOfElement;
        }
        set
        {
            this.numOfElement = value;
        }
    }
    public bool EnemyOnMap//getter and setter
    {
        get
        {
            return this.enemyOnMap;
        }
        set
        {
            this.enemyOnMap = value;
        }
    }
    public bool BossOnMap//getter and setter
    {
        get
        {
            return this.bossOnMap;
        }
        set
        {
            this.bossOnMap = value;
        }
    }
    public int NumOfEnemies//getter and setter
    {
        get
        {
            return this.numOfEnemies;
        }
        set
        {
            this.numOfEnemies = value;
        }
    }
    public int[] ItemsAmount//getter and setter
    {
        get
        {
            return this.itemsAmount;
        }
        set
        {
            this.itemsAmount = value;
        }
    }
    public float[] PlayerPos//getter and setter
    {
        get
        {
            return this.playerPos;
        }
        set
        {
            this.playerPos = value;
        }
    }
    public float[] BossPos//getter and setter
    {
        get
        {
            return this.bossPos;
        }
        set
        {
            this.bossPos = value;
        }
    }
    public bool[] DeadOrAliveEnemy//getter and setter
    {
        get
        {
            return this.deadOrAliveEnemy;
        }
        set
        {
            this.deadOrAliveEnemy = value;
        }
    }
    public bool[] ElementGot//getter and setter
    {
        get
        {
            return this.elementGot;
        }
        set
        {
            this.elementGot = value;
        }
    }
    public float[,] EnemiesPos//getter and setter
    {
        get
        {
            return this.enemiesPos;
        }
        set
        {
            this.enemiesPos = value;
        }
    }
    public float[] PlayersLevel//getter and setter
    {
        get
        {
            return this.playersLevel;
        }
        set
        {
            this.playersLevel = value;
        }
    }
    public int[] PlayersHP//getter and setter
    {
        get
        {
            return this.playersHP;
        }
        set
        {
            this.playersHP = value;
        }
    }
    public int[] PlayersMP//getter and setter
    {
        get
        {
            return this.playersMP;
        }
        set
        {
            this.playersMP = value;
        }
    }
    public int[] PlayersSP//getter and setter
    {
        get
        {
            return this.playersSP;
        }
        set
        {
            this.playersSP = value;
        }
    }
    public float HeartContainers//getter and setter
    {
        get
        {
            return this.heartContainers;
        }
        set
        {
            this.heartContainers = value;
        }
    }
    public float CurrentHealthInitialValue//getter and setter
    {
        get
        {
            return this.currentHealthInitialValue;
        }
        set
        {
            this.currentHealthInitialValue = value;
        }
    }
    public float CurrentHealthRuntimeValue//getter and setter
    {
        get
        {
            return this.currentHealthRuntimeValue;
        }
        set
        {
            this.currentHealthRuntimeValue = value;
        }
    }
    public float MaxMagic//getter and setter
    {
        get
        {
            return this.maxMagic;
        }
        set
        {
            this.maxMagic = value;
        }
    }
    public float CurrentMagic//getter and setter
    {
        get
        {
            return this.currentMagic;
        }
        set
        {
            this.currentMagic = value;
        }
    }
    public int[] CurrentHearts//getter and setter
    {
        get
        {
            return this.currentHearts;
        }
        set
        {
            this.currentHearts = value;
        }
    }
    public float BossSpeed//getter and setter
    {
        get
        {
            return this.bossSpeed;
        }
        set
        {
            this.bossSpeed = value;
        }
    }
    public bool BossActive//getter and setter
    {
        get
        {
            return this.bossActive;
        }
        set
        {
            this.bossActive = value;
        }
    }
}
