using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : logEnemy {

    const float minPathUpdateTime = .2f;//a fixed time to check if we need to update 
    const float pathUpdateMoveThreshold = .1f;//the Threshold of the distance between the old position and the new position
    Vector3[] path;//The A* search path
    int targetIndex;//the index for the next node in the path
    public Transform[] pathDot;//array of points for enemy to walk on
    public int currentPoint;//the point enemy needs to reach
    public Transform AstarPoint;//if A* search have ended then return to this point 
    public float roundingDistance;//the distance of the enemy from the point that is OK Before changing to the next point
    public Collider2D boundary;//boundary where the enemy will chase the player
    public bool enterOrExit;//boolean for the area where A* activates 
    public bool once=false;//to start the coroutine(UpdatePath) only one time
    IEnumerator Moveback()//a Coroutine to return to regular patrol 
    {
        yield return new WaitForSecondsRealtime(0.1f);
        toPoint = true;
    }
    public override void CheckDistance()//checks when to start the A*
    {
        if ((transform.position == AstarPoint.position)&&(target== AstarPoint.transform))//if we reach AstarPoint and AstarPoint is our target
        {
            StartCoroutine(Moveback());//coroutine for returning to regular patrol 
        }
        if ((enterOrExit == true))//if player inside the boundary
            {
            toPoint = false;//don't return to patrol
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)//enemy will start walking only if was in idle or was walking and not been attacked
                {
                    target = targetPlayer;//update the target to be the player
                    if (once==false)//if we have not yet to StartCoroutine UpdatePath() with the current target
                    {
                        StartCoroutine(UpdatePath());
                        once = true;//only once
                    }
                    ChangeState(EnemyState.walk);//change enemy state to walking
                    anim.SetBool("wakeUp", true);//start the animation
                }
            }
        else if (enterOrExit == false)//if player is not inside the boundary
        {
            once = false;//for searching the player again if needed
            if ((toPoint == true))//while toPoint is true do the patrol
            {
                if (Vector3.Distance(transform.position, pathDot[currentPoint].position) > roundingDistance && currentState != EnemyState.stagger)//if the enemy did not yet reached the dot and is not under attack then he moves
                {
                    Vector3 temp = Vector3.MoveTowards(transform.position, pathDot[currentPoint].position, moveSpeed * Time.deltaTime);//to move to the next point
                    changeAnim(temp - transform.position);//change animation according to the moving position
                    myRigidbody.MovePosition(temp);//change the position with the temp Value
                }
                else
                {
                    ChangeGoal();//change the destination point
                }
            }
            else
            {
                target = AstarPoint.transform;//update the target to be the AstarPoint for returning to patrol
            }
        }       
    }
    private void ChangeGoal()//change point to the next point in the array
    {
        if (currentPoint == pathDot.Length - 1)//if the enemy in the last point then reset
        {
            currentPoint = 0;//reset current point to 0 (the first point)
        }
        else//else increase current point
        {
            currentPoint++;//for the next point in the array
        }
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)//this is the action method for the PathResult and PathRequest which is called in the FindPath method in APathfinding script
    {
        if (pathSuccessful && currentState != EnemyState.stagger)
        {
            path = newPath;//save the path
            targetIndex = 0;//reset for the next node
            StopCoroutine("FollowPath");//stop the current Coroutine
            if (this.gameObject == isActiveAndEnabled)//if this enemy is active(alive) and is not been attacked
            {
                StartCoroutine("FollowPath");//start the Coroutine
            }            
        }
    }
    IEnumerator UpdatePath()//a Coroutine to request a path and update it
    {
        if (Time.timeSinceLevelLoad < .3f)//wait for the system to load
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));//request a new path
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;//sqr of the Threshold..the sqr is easier to calculate for the system
        Vector3 targetPosOld = target.position;//save the current position of the target
        while (true)// do always
        {
            yield return new WaitForSeconds(minPathUpdateTime);//wait a fixed time
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)//check if the distance between the old position and the new position is bigger then the Threshold
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));//request a new path 
                targetPosOld = target.position;//save the current position of the target
            }
        }       
    }    
    IEnumerator FollowPath()//a Coroutine to start the movement of the enemy to the player
    {
        Vector3 currentWaypoint = path[0];//the first node
        while (true)//do this always
        {
            if (transform.position == currentWaypoint)//if this object position is the currentWaypoint
            {
                targetIndex++;//next node
                if (targetIndex >= path.Length)//if we have reached or passed the target position
                {
                    targetIndex = 0;//reset
                    path = new Vector3[0];//reset
                    yield break;
                }
                currentWaypoint = path[targetIndex];//the currentWaypoint is the next node
            }
            if (currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);//for the enemy to move to the currentWaypoint position
                changeAnim(temp - transform.position);//change animation according to the moving position
                myRigidbody.MovePosition(temp);//change the position with the temp Value
                anim.SetBool("wakeUp", true);//start the animation
            }
            /*
            Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);//for the enemy to move to the currentWaypoint position
            changeAnim(temp - transform.position);//change animation according to the moving position
            myRigidbody.MovePosition(temp);//change the position with the temp Value
            anim.SetBool("wakeUp", true);//start the animation*/
            yield return null;
        }
    }  
    public void OnDrawGizmos()//a method for showing gizmo
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
