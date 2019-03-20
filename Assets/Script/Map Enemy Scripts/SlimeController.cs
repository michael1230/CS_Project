using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour {

    public float moveSpeed; //slime mooving speed

    private Rigidbody2D myRigidBody;

    private bool moving; //is the slime mooving or not

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;

    public float waitToReload;
    private bool reloading;
    private GameObject playerRain;


    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();

        //timeBetweenMoveCounter = timeBetweenMove;
        //timeToMoveCounter = timeToMove;

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeBetweenMove * 1.25f);

    }

    // Update is called once per frame
    void Update () {

        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidBody.velocity = moveDirection;

            if(timeToMoveCounter < 0f)
            {
                moving = false;
                //timeBetweenMoveCounter = timeBetweenMove;

                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
            }
        }

        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidBody.velocity = Vector2.zero;

            if(timeBetweenMoveCounter < 0f)
            {
                moving = true;
                //timeToMoveCounter = timeToMove;
                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeBetweenMove * 1.25f);

                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
            }
        }
        if(reloading)
        {
            waitToReload -= Time.deltaTime;
            if(waitToReload < 0)
            {
               // Application.LoadLevel(Application.loadedLevel);
                playerRain.SetActive(true);
            }
        }
		
	}

    void OnCollisionEnter2D(Collision2D other) //kill the player on contact
    {
        if(other.gameObject.name == "Player_Rain")
        {
            //Destroy(other.gameObject);

            other.gameObject.SetActive(false);
            reloading = true;
            playerRain = other.gameObject;
        }

    }

}
