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
        if (canMovePlayer)//if the flag is true
        {
           /* if (Input.anyKey == false) //if nothing is pressed 
            {
                theRB.velocity = Vector2.zero;//don't move
                myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation 
            }
            
            else*/ if (Input.GetButton(("Fire1")))//if ctrl is pressed 
            {
                theRB.velocity = Vector2.zero;//don't move
                myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation 
                StartCoroutine("OnCompleteAttackAnimation");//start the attack Coroutine
            }
            else if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical"))&&(myAnim.GetBool("isAttacking")==false))//if only arrows are pressed and the animation for attack is finished 
            {
                myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation 
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed;
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    myAnim.SetBool("isRunning", true);//start the running anim
                    theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * runSpeed;
                }
            }
            else
            {                       
                    theRB.velocity = Vector2.zero;//don't move
                    myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation                 
            }
        }
        else//if not
        {
            theRB.velocity = Vector2.zero;//don't move
        }
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

    IEnumerator OnCompleteAttackAnimation()//attack Coroutine
    {
        myAnim.Play("Attack");//start the attack anim
        myAnim.SetBool("isAttacking", true);//start the attack anim
        while (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)//wait until the attack is finished
            yield return null;
        myAnim.SetBool("isAttacking", false);//stop the attack anim
    }

}
