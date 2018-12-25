using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float walkSpeed;//the speed of walk
    public float runSpeed;//the speed of run
    public Rigidbody2D theRB;//reference the Rigidbody2D
    public Animator myAnim;//reference the Animator
    public static PlayerController instance;//makes only one instance of player
    public bool canMovePlayer = true;//a flag to spot the player when needed

    public string areaTransitionName;//the name to next area
    private Vector3 bottomLeftLimit;//the first limit of the  map
    private Vector3 topRightLimit;//the second limit of the  map

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (canMovePlayer)
        {               
            if (Input.GetButton(("Fire1")))
            {
                //theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))*0;
                myAnim.SetBool("isRunning", false);
                myAnim.SetBool("isWalking", false);
                myAnim.SetBool("isAttacking", true);                
            }
            else if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && (Input.GetKey(KeyCode.LeftShift)))//if arrows and shift are pressed
            {
                myAnim.SetBool("isRunning", true);
                myAnim.SetBool("isWalking", true);
                myAnim.SetBool("isAttacking", false);
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * runSpeed;
            }
            else if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")))//if only arrows are pressed
            {
                myAnim.SetBool("isRunning", false);
                myAnim.SetBool("isWalking", true);
                myAnim.SetBool("isAttacking", false);
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed;
            }
            else //if nothing is pressed
            {
                myAnim.SetBool("isRunning", false);
                myAnim.SetBool("isWalking", false);
                myAnim.SetBool("isAttacking", false);
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed;
            }
            
            
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

       /* if(Input.GetButton("Fire1"))
        {
            canMovePlayer = false;
            myAnim.SetBool("isAttacking", true);
        }
        else
        {
            canMovePlayer = true;
            myAnim.SetBool("isAttacking", false);
        }
        */
        myAnim.SetFloat("moveX", theRB.velocity.x);//Update the x velocity 
        myAnim.SetFloat("moveY", theRB.velocity.y);//Update the y velocity 

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {//keep the player in the right direction 
            if (canMovePlayer)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }
        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

    }
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }



}
