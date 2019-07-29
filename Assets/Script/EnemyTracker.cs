using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public bool enemyOnMap;//are there enemies on this map?
    public bool bossOnMap;//is there a boss on this map?
    public GameObject theBoss;//the boss chase object
    public GameObject[] theEnemies;//all the enemies objects on this map

    public void EnemyMovment(bool move)//a method to enable and disable enemies and boss movement
    {
        if (move)//if the input is true then enable movement
        {
            if (enemyOnMap)//if there are enemies on the map
            {
                for (int i = 0; i < theEnemies.Length; i++)//enable movement for all of them
                {
                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.None;
                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

            }
            if (bossOnMap)//is there a boss on this map then enable his movement
            {
                theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.None;
                theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else//if not then disable movement
        {
            if (enemyOnMap)//if there are enemies on the map
            {
                for (int i = 0; i < theEnemies.Length; i++)//disable movement for all fg them
                {
                    theEnemies[i].GetComponent<logEnemy>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            if (bossOnMap)//is there a boss on this map then disable his movement
            {
                theBoss.GetComponent<GralandChase>().myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

}

