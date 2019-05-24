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
                    //theEnemies[i].GetComponent<EnemyOnMap>().moveSpeed = 2; logEnemy

                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.None;
                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                if (bossOnMap)
                {
                    theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.None;
                    theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            else
            {
                if (enemyOnMap)
                {
                    //theEnemies[i].GetComponent<EnemyOnMap>().moveSpeed = 0;
                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                if (bossOnMap)
                {
                    theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }

}

