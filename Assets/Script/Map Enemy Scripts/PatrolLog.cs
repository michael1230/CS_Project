using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : logEnemy {

    public Transform[] path; //array of paths
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    public override void CheckDistance()//will to change to A star algorithm probably
    {
        if (Vector3.Distance(target.position,
                             transform.position) <= chaseRaidius
            && Vector3.Distance(target.position,
                                transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                   target.position,
                                                   moveSpeed * Time.deltaTime); //the log will move to the player
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                //ChangeState(EnemyState.walk);
                anim.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position,
                          transform.position) > chaseRaidius)
        {
            if(Vector3.Distance(transform.position, path[currentPoint].position) >roundingDistance && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                   path[currentPoint].position,//moves to the point
                                   moveSpeed * Time.deltaTime); //the log will move to the player
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
            }
            else
            {
                ChangeGoal();
            }
        }
    }

    private void ChangeGoal() //check to see what the current goal is
    {
        if(currentPoint == path.Length - 1) //reset current goal to path 0
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else //increase current point
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}
