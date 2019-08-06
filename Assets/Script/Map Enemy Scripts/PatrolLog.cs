using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : logEnemy {

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .1f;

    public Vector3 targetOldPosition;
    Vector3[] path;
    int targetIndex;

    public Transform[] pathDot; //array of dots for enemy to walk on
    public int currentPoint; //the first point from where enemy starts 
    public Transform currentGoal; //the dot enemy needs to reach
    public Transform AstarPoint; //if  A* search have ended then return to this point 
    public float roundingDistance; //the distance of the enemy from the dot that is OK Before changing to the next dot
    public Collider2D boundary; //boundary where the enemy will chase the player

    public bool enterOrExit; //boolean for the area where A* activates 
    public bool once=false; //////////////////////////
    //to start the coroutine(UpdatePath) only one time

    IEnumerator Moveback()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        toPoint = true;
    }

    public override void CheckDistance()//will to change to A star algorithm probably
    {
        if ((transform.position == AstarPoint.position)&&(target== AstarPoint.transform))
        {
            StartCoroutine(Moveback());
        }
        if ((enterOrExit == true))
            {
            toPoint = false;
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger) //enemy will start walking only if was in idle or was walking and not been attacked
                {
                target = targetPlayer; //update the target to be the player
                if (once==false)
                {
                    StartCoroutine(UpdatePath());
                    once = true;
                }
                ChangeState(EnemyState.walk); //change enemy state to walking
                anim.SetBool("wakeUp", true); //start the animation
            }
            }
        else if (enterOrExit == false)
        {
            once = false; //for searching the player again if needed
            if ((toPoint == true)) //if only the enemy was in the AstarPoint then go patrolling
            {
                if (Vector3.Distance(transform.position, pathDot[currentPoint].position) > roundingDistance && currentState != EnemyState.stagger) //if the enemy did not yet reached the dot and is not under attack then he moves
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position, pathDot[currentPoint].position, moveSpeed * Time.deltaTime); //to move to the next dot
                    changeAnim(temp - transform.position); //change animation according to the moving position
                    myRigidbody.MovePosition(temp); //change the position with the temp Value
                }
                else
                {
                    ChangeGoal(); //change the destination dot
                }
            }
            else
            {
                target = AstarPoint.transform; //update the target to be the AstarPoint for returning to patrol
            }
        }       
    }

    private void ChangeGoal() //change dot to the next dot in the array
    {
        if (currentPoint == pathDot.Length - 1) //if the enemy in the last point the reset
        {
            currentPoint = 0; //reset current dot to 0
            currentGoal = pathDot[0]; //reset current goal dot to currentPoint which is 0
        }
        else //else increase current point
        {
            currentPoint++; //the next dot in the array
            currentGoal = pathDot[currentPoint]; //current goal dot is currentPoint
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
                if (targetIndex >= path.Length)//targetIndex should be reset
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime); //for the enemy to move to the currentWaypoint position
            changeAnim(temp - transform.position); //change animation according to the moving position
            myRigidbody.MovePosition(temp); //change the position with the temp Value
            anim.SetBool("wakeUp", true); //start the animation
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
}
