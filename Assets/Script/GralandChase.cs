using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GralandChase : MonoBehaviour
{
    public string enemyName;
    public float moveSpeed;
    public bool isAlive;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .1f;

    public Rigidbody2D myRigidbody;
    public Transform target;//moving the enemy
    public Vector3 targetOldPosition;
    Vector3[] path;
    int targetIndex;
    public Animator anim;

    // Use this for initialization
    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
        StartCoroutine(UpdatePath());
        InvokeRepeating("SpeedMore", 1f, 1f);
    }

    // Update is called once per frame
    void Update ()
    {
        if (moveSpeed==12)
        {
            CancelInvoke();
        }
	}

    public void SpeedMore()
    {
        if (GameManager.instance.noMenu==false)
        {
            moveSpeed += 0.05f;
        }

    }

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
                if (targetIndex >= path.Length)//targetIndex should be reset
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            Vector3 temp = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime); //the log will move to the player
            ChangeAnim(temp.x , transform.position.x);
            myRigidbody.MovePosition(temp);

            yield return null;
        }
    }

    private void SetAnimFloat(Vector2 setVector)//change animation
    {
        anim.SetFloat("moveX", setVector.x);
    }

    public void ChangeAnim(float pointTo, float pointFrom)//change animation direction
    {
        if (pointFrom>0)
        {
            if (pointTo- pointFrom>=0)
            {
                SetAnimFloat(Vector2.right);
            }
            else
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else
        {
            if (pointTo - pointFrom >= 0)
            {
                SetAnimFloat(Vector2.left);
            }
            else
            {
                SetAnimFloat(Vector2.right);
            }
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
