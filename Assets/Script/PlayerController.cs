 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState //collection of constants for players different states
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
    public PlayerState currentState; //state for the enum
    public float walkSpeed; //the speed of walk
    public float runSpeed; //the speed of run
    private Rigidbody2D myRigidbody; //reference the Rigidbody2D
    private Vector3 change; //players Vector3
    private Animator animator; //reference the Animator

    public bool canMovePlayer = true; //a flag to spot the player when needed
    public bool canMoveAnimte = true; //a flag to spot the player when needed

    public static PlayerController instance; //makes only one instance of player
    public string areaTransitionName; //the name to next area
    private Vector3 bottomLeftLimit; //the first limit of the  map
    private Vector3 topRightLimit; //the second limit of the  map
    public FloatValue currentHealth; //player current health
    public Signal playerHealthSignal; //reference to current health
    public Signal reduceMagic; //reference to the MagicBar
    public GameObject projectile; //GameObject for the FireBall
    public MapInventory playerInventory; //reference to Inventory
    public bool imAlive;

    [Header("IFrame Stuff")] //for when player gets hit
    public Color flashColor; //the color while been hit
    public Color regularColor; //normal player color
    public float flashDuration; //how long will be the flashColor
    public int numberOfFlashes; //number of flashColor
    public Collider2D triggerCollider; //for recognizing when player gets hit
    public SpriteRenderer mySprite; //change players sprite

    public Rigidbody2D MyRigidbody //for set/get player Rigidbody2D
    {
        get
        {
            return this.myRigidbody;
        }
        set
        {
            this.myRigidbody = value;
        }
    }

    void Start()
    {
        currentState = PlayerState.walk; //player first state is walking
        animator = GetComponent<Animator>(); //get the Animator of the player
        myRigidbody = GetComponent<Rigidbody2D>(); //get the Rigidbody2D of the player
        animator.SetFloat("moveX", 0);//first direction of player
        animator.SetFloat("moveY", -1);//first direction of player
        imAlive = true; //there is a player

        //for not deleting the player between scene
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
        //is the player in an interaction do not do anything 
        if (currentState == PlayerState.interact)
        {
            return;
        }
        if (canMovePlayer)//if the flag is true
        {/*
            if (currentState == PlayerState.interact)
            {
                return;
            }*/
            change = Vector3.zero; //reset how much the player moved every frame
            change.x = Input.GetAxisRaw("Horizontal");//for moving Horizontal 
            change.y = Input.GetAxisRaw("Vertical");//for moving Vertical
            animator.SetBool("isRunning", false);//change animation of running to false    
            animator.SetBool("moving", false);//change animation of moving to false     
            if ((Input.GetKey(KeyCode.LeftControl) && currentState != PlayerState.attack && currentState != PlayerState.stagger) && (GameManager.instance.canAttack)) //normal attack
            {
                StartCoroutine(AttackCo()); //start Coroutine for normal attack
            }
            else if ((Input.GetKey(KeyCode.Z) && currentState != PlayerState.attack && currentState != PlayerState.stagger && playerInventory.currentMagic > 0) && (GameManager.instance.canAttack)) //fire attack
            {
                StartCoroutine(SecondAttackCo()); //start Coroutine for fire attack
            }
            else if (((currentState == PlayerState.walk || currentState == PlayerState.idle || currentState == PlayerState.run)
                      && Input.GetKey(KeyCode.LeftShift) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))) && (canMoveAnimte == true)) //running state
            {
                animator.SetBool("moving", true);
                UpdateAnimationAndRun(); //update animation to running animation
            }
            else if ((currentState == PlayerState.walk || currentState == PlayerState.idle) && (canMoveAnimte == true))//walking state
            {

                UpdateAnimationAndMove(); //update animation to moving animation
            }
            //keep the camera inside the bounds
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        }//Clamps the given value between the given minimum float and maximum float values. Returns the given value if it is within the min and max range.
    }

    private IEnumerator AttackCo() //normal attack Coroutine
    {
        animator.SetBool("attacking", true);//start the attack animation
        currentState = PlayerState.attack; //player is attacking
        yield return null; //wait for frame
        animator.SetBool("attacking", false);//stop the attack animation
        yield return new WaitForSeconds(.3f); //wait until attack animation is finished
        if (currentState != PlayerState.interact) //if player finished interacting the he can walk
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecondAttackCo() //attack Coroutine
    {
        animator.SetBool("attackingFire", true); //start the fire attack animation
        currentState = PlayerState.attack; //player is attacking
        yield return null; //wait for frame
        MakeFireBall(); //makes the fire ball
        animator.SetBool("attackingFire", false); //stop the fire attack animation
        yield return new WaitForSeconds(.3f);//wait until attack animation is finished
        if (currentState != PlayerState.interact) //if player finished interacting the he can walk
        {
            currentState = PlayerState.walk;
        }
    }

    private void MakeFireBall() //makes the fire ball
    {
        if (playerInventory.currentMagic > 0) //if player still got magic in the magic bar
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY")); //for the blend tree animation of the fireBall
            FireBall fireBall = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<FireBall>(); //makes a fireBall clone thats in the player position
            fireBall.Setup(temp, ChooseFireBallDirection()); //for moving the fire ball in 
            reduceMagic.Raise(); //send signal to decrease magic player has
        }

    }

    Vector3 ChooseFireBallDirection() //for the fireBall direction
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg; //Returns the angle in radians whose Tan is y / x.
        return new Vector3(0, 0, temp);
    }

    void UpdateAnimationAndMove()//moving function
    {
        if (change != Vector3.zero) //only if there is change in the movement then update animation
        {
            MoveCharacter(); //for changing position
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false); //after finished stop moving animation
        }
    }

    void MoveCharacter()
    {
        change.Normalize();//normalize the vector for walking diagonally the same speed as the sides
        myRigidbody.MovePosition(transform.position + change * walkSpeed * Time.fixedDeltaTime);//move position
    }

    void UpdateAnimationAndRun()//running function
    {
        if (change != Vector3.zero) //only if there is change in the movement then update animation
        {
            RunCharacter(); //for changing position
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false); //after finished stop running animation
        }
    }

    void RunCharacter()
    {
        change.Normalize();//normalize the vector for running diagonally the same speed as the sides
        myRigidbody.MovePosition(transform.position + change * runSpeed * Time.fixedDeltaTime);//move position
    }

    public void knock(float knockTime, float damage)//function for knock and damage effect when colliding with enemy
    {
        currentHealth.RuntimeValue -= damage;//lost off health after enemy hit
        playerHealthSignal.Raise();//send signal to change the hearts
        if (currentHealth.RuntimeValue > 0)//if there is still health
        {
            StartCoroutine(KnockCo(knockTime)); //start coroutine with how long the knock back is
        }
        else
        {//kill the player
            imAlive = false;
            mySprite.enabled = false;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }

    private IEnumerator KnockCo(float knockTime) //knock the player
    {
        if (myRigidbody != null) //if player is not dead
        {
            StartCoroutine(FlashCo()); //start flashing red after getting hit
            yield return new WaitForSeconds(knockTime); //for the knock back time
            currentState = PlayerState.idle; //change to idle while been knock back
            myRigidbody.velocity = Vector2.zero; //let the Rigidbody move freely for the effect of the knock back
        }
    }
    private IEnumerator FlashCo()//for flashing after hit
    {
        int temp = 0;
        triggerCollider.enabled = false; //for only one in a time
        while (temp < numberOfFlashes) //a loop for a number of flashes
        {
            mySprite.color = flashColor; //change color to red
            yield return new WaitForSeconds(flashDuration); //for how long player stays red
            mySprite.color = regularColor; //change to normal color
            yield return new WaitForSeconds(flashDuration); //for how long player stays normal color
            temp++; //next loop
        }
        triggerCollider.enabled = true; //activate triggerCollider again
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)//for the camera
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

    public void PlyerIdle(bool anime)//a method to stop player animation
    {
        if (!anime)
        {
            canMoveAnimte = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("moving", false);
            animator.SetBool("attacking", false);
            animator.SetBool("attackingFire", false);
        }
        else
        {
            canMoveAnimte = true;
        }
    }
}