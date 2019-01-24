using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logEnemy : EnemyOnMap {

    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRaidius;
    public float attackRadius;
    public Transform homePosition; //for returning to base if not attacking
    public Animator anim;

	// Use this for initialization
	void Start () {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform; //finds the player location
	}
	
	// Update is called once per frame
	void FixedUpdate () {//check every 30 sec
        CheckDistance();
	}

    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRaidius && Vector3.Distance(target.position, transform.position) >attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); //the log will move to the player
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
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
