 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState //collection of constants for players movements
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
    public GameObject projectile; //reference to the FireBall
    public MapInventory playerInventory; //reference to Inventory
    public bool imAlive;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    public Rigidbody2D MyRigidbody
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
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);//first direction of player
        animator.SetFloat("moveY", -1);//first direction of player
        imAlive = true;

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
        //is the player in an interaction
        if (currentState == PlayerState.interact)
        {
            return;
        }
        if (canMovePlayer)//if the flag is true
        {
            if (currentState == PlayerState.interact)
            {
                return;
            }
            change = Vector3.zero; //reset how much the player moved
            change.x = Input.GetAxisRaw("Horizontal");//for moving
            change.y = Input.GetAxisRaw("Vertical");//for moving
            animator.SetBool("isRunning", false);//change animtion    
            animator.SetBool("moving", false);//change animtion     (Input.GetKey(KeyCode.LeftControl)  (Input.GetButtonDown("Fire1")
            if ((Input.GetKey(KeyCode.LeftControl) && currentState != PlayerState.attack && currentState != PlayerState.stagger) && (GameManager.instance.canAttack))//attack state
            {
                StartCoroutine(AttackCo());
            }//Input.GetButtonDown("FireBall")
            else if ((Input.GetKey(KeyCode.Z) && currentState != PlayerState.attack && currentState != PlayerState.stagger && playerInventory.currentMagic > 0) && (GameManager.instance.canAttack))
            {
                StartCoroutine(SecondAttackCo());
            }
            else if (((currentState == PlayerState.walk || currentState == PlayerState.idle || currentState == PlayerState.run)
                      && Input.GetKey(KeyCode.LeftShift) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))) && (canMoveAnimte == true))//running state
            {
                animator.SetBool("moving", true);
                UpdateAnimationAndRun();
            }
            else if ((currentState == PlayerState.walk || currentState == PlayerState.idle) && (canMoveAnimte == true))//walking state
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
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecondAttackCo() //attack Coroutine
    {
        animator.SetBool("attackingFire", true);//start the attack anim
        currentState = PlayerState.attack;
        yield return null;
        MakeFireBall();
        animator.SetBool("attackingFire", false);//stop the attack anim
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private void MakeFireBall()
    {
        if (playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            FireBall fireBall = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<FireBall>();
            fireBall.Setup(temp, ChooseFireBallDirection());
            reduceMagic.Raise();
        }

    }

    Vector3 ChooseFireBallDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
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

    public void knock(float knockTime, float damage)//function for knock and damage effect when colliding with enemy
    {
        currentHealth.RuntimeValue -= damage;//lost off health after enemy hit
        StartCoroutine(KnockCo(knockTime));
        playerHealthSignal.Raise();//send signal to change the hearts0
        if (currentHealth.RuntimeValue > 0)//if there is still health
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {//kill the player- for now just inactive
            imAlive = false;
            mySprite.enabled = false;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //this.gameObject.SetActive(false);
        }

    }

    private IEnumerator KnockCo(float knockTime) //knock the player
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    private IEnumerator FlashCo()//for flashinng after hit
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)//for the camera
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }

    public void PlyerIdle(bool anime)//a method to  stooping player anim
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