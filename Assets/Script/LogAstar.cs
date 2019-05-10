using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogAstar : EnemyOnMap
{

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .1f;

    public Rigidbody2D myRigidbody;
    public Transform target;//moving the enemy
    public Vector3 targetOldPosition;
    public float chaseRaidius;
    public float attackRadius;
    public Transform homePosition; //for returning to base if not attacking

    Vector3[] path;
    int targetIndex;

    [Header("Animator")]
    public Animator anim;

    // Use this for initialization
    /*void Start()
    {
        currentState = EnemyState.idle;//first state is idle
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        targetOldPosition = target.position;
        anim.SetBool("wakeUp", true);
        ChangeState(EnemyState.walk);
    }*/


    void Start()
    {
        currentState = EnemyState.idle;//first state is idle
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
        targetOldPosition = target.position;
        anim.SetBool("wakeUp", true);
        ChangeState(EnemyState.walk);
        StartCoroutine(UpdatePath());
    }



    // Update is called once per frame
    /*void Update()
    {//check every 30 sec
     //CheckDistance();//check the distance bettwen log and player
    if (Vector3.Distance(targetOldPosition, target.position) > 0)
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        targetOldPosition = target.position;
    }
    }*/

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
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

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);

            //changeAnim(temp - transform.position);
            // myRigidbody.MovePosition(Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime));
            ChangeState(EnemyState.walk);
            anim.SetBool("wakeUp", true);
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





    public virtual void CheckDistance()//will to change to A star algorithm probably
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
                ChangeState(EnemyState.walk);
                anim.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position,
                          transform.position) > chaseRaidius)
        {
            anim.SetBool("wakeUp", false);
        }
    }
    private void SetAnimFloat(Vector2 setVector)//change animation
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

    public void changeAnim(Vector2 direction)//change animation direction
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }

        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
