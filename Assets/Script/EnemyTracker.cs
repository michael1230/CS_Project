using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public bool enemyOnMap;
    public bool bossOnMap;
    public GameObject theBoss;
    public GameObject[] theEnemies;




    public void EnemyMovment(bool move)
    {
        for (int i = 0; i < theEnemies.Length; i++)
        {
            if (move)
            {
                if (enemyOnMap)
                {
                    theEnemies[i].GetComponent<EnemyOnMap>().moveSpeed = 2;
                }
                if (bossOnMap)
                {
                    theBoss.GetComponent<EnemyOnMap>().moveSpeed = 2;//later other script!!!!!
                }
            }
            else
            {
                if (enemyOnMap)
                {
                    theEnemies[i].GetComponent<EnemyOnMap>().moveSpeed = 0;
                }
                if (bossOnMap)
                {
                    theBoss.GetComponent<EnemyOnMap>().moveSpeed = 0;//later other script!!!!!
                }
            }
        }
    }

}

