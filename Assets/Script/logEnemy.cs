using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logEnemy : EnemyOnMap {

    public Transform target;
    public float chaseRaidius;
    public float attackRadius;
    public Transform homePosition; //for returning to base if not attacking

	// Use this for initialization
	void Start () {
        target = GameObject.FindWithTag("Player").transform; //finds the player location
	}
	
	// Update is called once per frame
	void Update () {
        CheckDistance();
	}

    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRaidius && Vector3.Distance(target.position, transform.position) >attackRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); //the log will move to the player
        }
    }

}
