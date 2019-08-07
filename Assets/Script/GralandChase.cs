using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GralandChase : MonoBehaviour
{
    public float moveSpeed;//moving speed
    const float minPathUpdateTime = .2f;//a fixed time to check if we need to update 
    const float pathUpdateMoveThreshold = .1f;//the Threshold of the distance between the old position and the new position
    public Rigidbody2D myRigidbody;//enemy Rigidbody2D
    public Transform target;//The target Transform position
    Vector3[] path;//The A* search path
    int targetIndex;//the index for the next node in the path
    public Animator anim;//reference the Animator
    // Use this for initialization
    void Start ()//start values
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
        StartCoroutine(UpdatePath());
        InvokeRepeating("SpeedMore", 1f, 1f);//call this method multiple times
    }
    // Update is called once per frame
    void Update ()
    {
        if (moveSpeed==12)//if the speed is 12 then stay at it
        {
            CancelInvoke();
        }
	}
    public void SpeedMore()//a method for increasing the speed
    {
        if (GameManager.instance.dontGainSpeed==false)
        {
            moveSpeed += 0.02f;
        }

    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)//this is the action method for the PathResult and PathRequest which is called in the FindPath method in APathfinding script
    {
        if (pathSuccessful)//if its true
        {
            path = newPath;// save the path
            targetIndex = 0;//reset for the next node
            StopCoroutine("FollowPath");//stop the current Coroutine
            StartCoroutine("FollowPath");//start the Coroutine
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
            Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);//to move to the next node
            ChangeAnim(temp.x , transform.position.x);//change animation according to the moving position
            myRigidbody.MovePosition(temp);//change the position with the temp Value
            yield return null;
        }
    }
    private void SetAnimFloat(Vector2 setVector)//change animation
    {
        anim.SetFloat("moveX", setVector.x);
    }
    public void ChangeAnim(float pointTo, float pointFrom)//change animation direction
    {
        if (pointFrom>0)//to know which calculation do to for the next side calculation
        {
            if (pointTo- pointFrom>=0)//if its bigger then 0 then
            {
                SetAnimFloat(Vector2.right);//the direction is right
            }
            else
            {
                SetAnimFloat(Vector2.left);//the direction is left
            }
        }
        else//if its negative then this is the other calculation
        {
            if (pointTo - pointFrom >= 0)//if its bigger then 0 then
            {
                SetAnimFloat(Vector2.left);//the direction is left
            }
            else
            {
                SetAnimFloat(Vector2.right);//the direction is right
            }
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
