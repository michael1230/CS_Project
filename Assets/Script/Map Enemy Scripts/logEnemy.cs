using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logEnemy : EnemyOnMap {

    public Rigidbody2D myRigidbody;//enemy Rigidbody2D
    public Transform target;//moving the enemy to given target (player or dot) 
    public Transform targetPlayer;//moving the enemy to player position
    public float chaseRadius;//the area which the enemy follows player
    public float attackRadius;//the area which the enemy attacks player
    public bool toPoint;//boolean for activating enemy return patrolling if A* stopped

    [Header("Animator")]
    public Animator anim;//reference the Animator

    // Use this for initialization
    void Start () {
        currentState = EnemyState.idle;//first state is idle
        myRigidbody = GetComponent<Rigidbody2D>();//get the Rigidbody2D of enemy
        anim = GetComponent<Animator>();//get the Animator of the enemy
        target = GameObject.FindWithTag("Player").transform;//first target is player
        targetPlayer = GameObject.FindWithTag("Player").transform;//finds the player location
        anim.SetBool("wakeUp", true);//start the animation
        ChangeState(EnemyState.walk);//first state
        toPoint = true;//first value is true for the enemy to move between the dots
}

    // Update is called once per frame
    void FixedUpdate ()
    {//check every 30 sec
        CheckDistance();//check the distance between log and player
	}

    public virtual void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)//move the enemy if only the player is between the chase radius and attack radius
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)//if enemy was idle or walking and not been staggered(attacked) then he can attack player
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);//the enemy will move to the player
                changeAnim(temp - transform.position);//change animation according to the moving position
                myRigidbody.MovePosition(temp);//change the position with the temp Value
                ChangeState(EnemyState.walk);//change enemy state to walking
                anim.SetBool("wakeUp", true);//start the animation
            }
        }
        else if(Vector3.Distance(target.position, transform.position) > chaseRadius)//if player is not in the chaseRaidius the enemy goes to sleep 
        {
            anim.SetBool("wakeUp", false);
        }
    }
    
    private void SetAnimFloat(Vector2 setVector)//change animation
    {
        anim.SetFloat("moveX", setVector.x);//change according to x axis
        anim.SetFloat("moveY", setVector.y);//change according to y axis
    }

    public void changeAnim(Vector2 direction)//change animation direction
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))//for moving to the sides, checks absolute value  
            {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);//animation with the right direction
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);//animation with the left direction
            }
        }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))//for moving up or down, checks absolute value 
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);//animation with the up direction
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);//animation with the down direction
            }

        }
    }
    
    public void ChangeState(EnemyState newState)//for changing enemy states
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    
}
