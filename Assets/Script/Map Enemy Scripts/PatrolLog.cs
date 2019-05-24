using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : logEnemy {

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .1f;

    public Vector3 targetOldPosition;
    Vector3[] path;
    int targetIndex;

    public Transform[] pathDot; //array of paths
    public int currentPoint;
    public Transform currentGoal;
    public Transform AstarPoint;
    public float roundingDistance;
   // public bool toPoint = true;

    public Collider2D boundary;

    public bool enterOrExit;
    public bool once=false;



    IEnumerator Moveback()//NEED TO PUT THE DOT IN THE MIDDLE AND ROUND IT 4.485 == 4.5
    {
        yield return new WaitForSecondsRealtime(0.1f);

        //yield return new WaitForSeconds(0.3f);
        //StopAllCoroutines();
        toPoint = true;
        //yield return null;
    }

    public override void CheckDistance()//will to change to A star algorithm probably
    {
        if ((transform.position == AstarPoint.position)&&(target== AstarPoint.transform))
        {
            StartCoroutine(Moveback());
        }
        /*if (Vector3.Distance(targetPlayer.position,
                 transform.position) <= chaseRaidius
                && Vector3.Distance(targetPlayer.position,
                    transform.position) > attackRadius
                && boundary.bounds.Contains(targetPlayer.transform.position))
        */

        if ((enterOrExit == true))
            {
            toPoint = false;
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                    && currentState != EnemyState.stagger)
                {
                target = targetPlayer;
                if (once==false)
                {
                    StartCoroutine(UpdatePath());
                    once = true;
                }
                //StartCoroutine(UpdatePath());
                ChangeState(EnemyState.walk);
                anim.SetBool("wakeUp", true);
                // toPoint = false;
                }
            }
            //else if (Vector3.Distance(targetPlayer.position, transform.position) > chaseRaidius || !boundary.bounds.Contains(targetPlayer.transform.position))
        else if (enterOrExit == false)
        {
            once = false;
            if ((toPoint == true))
            {
                if (Vector3.Distance(transform.position, pathDot[currentPoint].position) > roundingDistance && currentState != EnemyState.stagger)
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position,
                                       pathDot[currentPoint].position,//moves to the point
                                       moveSpeed * Time.deltaTime); //the log will move to the player
                    changeAnim(temp - transform.position);
                    myRigidbody.MovePosition(temp);
                }
                else
                {
                    ChangeGoal();
                }
            }
            else
            {
                target = AstarPoint.transform;
            }

        }       
    }

    private void ChangeGoal() //check to see what the current goal is
    {
        if (currentPoint == pathDot.Length - 1) //reset current goal to path 0
        {
            currentPoint = 0;
            currentGoal = pathDot[0];
        }
        else //increase current point
        {
            currentPoint++;
            currentGoal = pathDot[currentPoint];
        }
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            if (this.gameObject == isActiveAndEnabled && currentState != EnemyState.stagger)//for stopping if enemy was killed
            {
                StartCoroutine("FollowPath");
            }
            
        }
    }


    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
   
        
    }



    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                /*
                if (targetIndex >= path.Length)
                {
                    yield break;
                }*/
                if (targetIndex >= path.Length)//targetIndex should be reset
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            changeAnim(temp - transform.position);
            myRigidbody.MovePosition(temp);
            //ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
            //yield return new WaitForSecondsRealtime(10);
            yield return null;
        }
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.8f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }


    /*
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
    */
}
