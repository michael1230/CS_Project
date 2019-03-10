﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logEnemy : EnemyOnMap {

    private Rigidbody2D myRigidbody;
    public Transform target;//moving the enemy
    public float chaseRaidius;
    public float attackRadius;
    public Transform homePosition; //for returning to base if not attacking
    public Animator anim;

	// Use this for initialization
	void Start () {
        currentState = EnemyState.idle;//first state is idle
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
	}
	
	// Update is called once per frame
	void FixedUpdate () {//check every 30 sec
        CheckDistance();//check the distance bettwen log and player
	}

    void CheckDistance()
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
        else if(Vector3.Distance(target.position, 
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

    private void changeAnim(Vector2 direction)//change animation direction
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
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

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
