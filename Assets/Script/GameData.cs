using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool[] elementGot;//the number of enemies on this map
    private float[,] enemiesPos;//the pos of the enemies..first index is the enemy number second index is the pos: 0-x,1-y,2-z

    public string SceneName
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
    public int NumOfElement
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
    public bool EnemyOnMap
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
    public bool BossOnMap
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
    public int NumOfEnemies
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
    public int[] ItemsAmount
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
    public float[] PlayerPos
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
    public float[] BossPos
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
    public bool[] DeadOrAliveEnemy
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
    public bool[] ElementGot
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
    
    public float[,] EnemiesPos
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

}
