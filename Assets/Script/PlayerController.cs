using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    run,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerController : MonoBehaviour
{
    public PlayerState currentState;//state for the enum
    public float walkSpeed;//the speed of walk
    public float runSpeed;//the speed of run
    private Rigidbody2D myRigidbody;//reference the Rigidbody2D
    private Vector3 change;//players Vector3
    private Animator animator;//reference the Animator
    public bool canMovePlayer = true;//a flag to spot the player when needed
    public static PlayerController instance;//makes only one instance of player
    public string areaTransitionName;//the name to next area
    private Vector3 bottomLeftLimit;//the first limit of the  map
    private Vector3 topRightLimit;//the second limit of the  map

    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);//first direction of player
        animator.SetFloat("moveY", -1);//first direction of player

        //for not deleting the player bettwen sence
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

    void FixedUpdate()
    {
        if (canMovePlayer)//if the flag is true
        {
            change = Vector3.zero; //reset how much the player moved
            change.x = Input.GetAxisRaw("Horizontal");//for moving
            change.y = Input.GetAxisRaw("Vertical");//for moving
            animator.SetBool("isRunning", false);//change animtion
            animator.SetBool("moving", false);//change animtion
            if (Input.GetKey(KeyCode.LeftControl) && currentState != PlayerState.attack && currentState != PlayerState.stagger)//attack state
            {
                StartCoroutine(AttackCo());
            }
            else if ((currentState == PlayerState.walk || currentState == PlayerState.idle)
                      && Input.GetKey(KeyCode.LeftShift) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))//running state
            {
                animator.SetBool("moving", true);
                UpdateAnimationAndRun();
            }
            else if (currentState == PlayerState.walk || currentState == PlayerState.idle)//walking state
            {

                UpdateAnimationAndMove();
            }
            //keep the camera inside the bounds
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        }
    }

    private IEnumerator AttackCo() //attack Coroutine
    {
        animator.SetBool("attacking", true);//start the attack anim
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);//stop the attack anim
        yield return new WaitForSeconds(.31f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()//mooving function
    {
        if (change != Vector3.zero)//move only if player idle
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();//normlize the vector for walking diagonally the same speed as the sides
        myRigidbody.MovePosition(transform.position + change * walkSpeed * Time.fixedDeltaTime);//move position
    }

    void UpdateAnimationAndRun()//running function
    {
        if (change != Vector3.zero)//move only if player idle
        {
            RunCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void RunCharacter()
    {
        change.Normalize();//normlize the vector for running diagonally the same speed as the sides
        myRigidbody.MovePosition(transform.position + change * runSpeed * Time.fixedDeltaTime);//move position
    }

    public void knock(float knockTime)//function for knock effect when colliding with enemy
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime) //knock the player
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)//for the camera
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

    /*

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
            if (Input.GetButton(("Fire1")))//if ctrl is pressed 
            {
                theRB.velocity = Vector2.zero;//don't move
                myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation 
                StartCoroutine("OnCompleteAttackAnimation");//start the attack Coroutine
            }
            else if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical"))&&(myAnim.GetBool("isAttacking")==false))//if only arrows are pressed and the animation for attack is finished 
            {
                myAnim.SetBool("isRunning", false);//if we came from Running then stop running animation 

               // theRB.velocity.Normalize();

                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed;
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    myAnim.SetBool("isRunning", true);//start the running anim

                   // theRB.velocity.Normalize();


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

    */

}
