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
    public PlayerState currentState; //state for the enum
    public float walkSpeed; //the speed of walk
    public float runSpeed; //the speed of run
    private Rigidbody2D myRigidbody; //reference the Rigidbody2D
    private Vector3 change; //players Vector3
    private Animator animator; //reference the Animator
    public bool canMovePlayer = true; //a flag to spot the player when needed
    public static PlayerController instance; //makes only one instance of player
    public string areaTransitionName; //the name to next area
    private Vector3 bottomLeftLimit; //the first limit of the  map
    private Vector3 topRightLimit; //the second limit of the  map
    public FloatValue currentHealth; //player current health
    public Signal playerHealthSignal; //reference to current health
    public Signal reduceMagic;
    public GameObject projectile;
    public MapInventory playerInventory;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;



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
        //is the player in an interaction
        if(currentState == PlayerState.interact)
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
            if (Input.GetKey(KeyCode.LeftControl) && currentState != PlayerState.attack && currentState != PlayerState.stagger)//attack state
            {
                StartCoroutine(AttackCo());
            }//Input.GetButtonDown("FireBall")
            else if (Input.GetKey(KeyCode.Z) && currentState != PlayerState.attack && currentState != PlayerState.stagger && playerInventory.currentMagic > 0)
            {
                StartCoroutine(SecondAttackCo());
            }
            else if ((currentState == PlayerState.walk || currentState == PlayerState.idle || currentState == PlayerState.run)
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
        yield return new WaitForSeconds(.3f);
        if(currentState != PlayerState.interact)
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
        if(playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            FireBall fireBall = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<FireBall>();
            fireBall.Setup(temp, ChooseFireBallDirection());
            reduceMagic.Raise();
        }

    }

    Vector3 ChooseFireBallDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX"))* Mathf.Rad2Deg;
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
            this.gameObject.SetActive(false);
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
        while(temp < numberOfFlashes)
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


    /*
    public enum PlayerState
    {
        walk,
        run,
        attack,
        interact,
        stagger,
        idle
    }
    public enum PlayerFace
    {
        Up,
        Down,
        Left,
        Right
    }

    public class PlayerController : MonoBehaviour
    {
        public PlayerFace currentFace;// set up a public variable for the PlayerFace
        // set up references to the hitboxes and attach them in the GUI
        public GameObject HitBoxDown;
        public GameObject HitBoxUp;
        public GameObject HitBoxLeft;
        public GameObject HitBoxRight;

        public PlayerState currentState; //state for the enum
        public float walkSpeed; //the speed of walk
        public float runSpeed; //the speed of run
        private Rigidbody2D myRigidbody; //reference the Rigidbody2D
        private Vector3 change; //players Vector3
        private Animator animator; //reference the Animator
        public bool canMovePlayer = true; //a flag to spot the player when needed
        public static PlayerController instance; //makes only one instance of player
        public string areaTransitionName; //the name to next area
        private Vector3 bottomLeftLimit; //the first limit of the  map
        private Vector3 topRightLimit; //the second limit of the  map
        public FloatValue currentHealth; //player current health
        public Signal playerHealthSignal; //reference to current health

        void Start()
        {
            currentFace = PlayerFace.Down;
            HitBoxDown.SetActive(false);
            HitBoxUp.SetActive(false);
            HitBoxLeft.SetActive(false);
            HitBoxRight.SetActive(false);

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
                UpdateFace();
                if (currentState == PlayerState.interact)
                {
                    return;
                }
                change = Vector3.zero; //reset how much the player moved
                change.x = Input.GetAxisRaw("Horizontal");//for moving
                change.y = Input.GetAxisRaw("Vertical");//for moving
                animator.SetBool("isRunning", false);//change animtion    
                animator.SetBool("moving", false);//change animtion
                if (Input.GetButtonDown("Fire1") && currentState != PlayerState.attack && currentState != PlayerState.stagger)//attack state
                {
                    UpdateFace();
                    StartCoroutine(AttackCo());
                    UpdateFace();
                }
                else if ((currentState == PlayerState.walk || currentState == PlayerState.idle || currentState == PlayerState.run)
                          && Input.GetButton("Fire3") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))//running state
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

        // update face functions
        private void UpdateFace()
        {
            if (change.x > 0)
            {
                currentFace = PlayerFace.Right;
            }
            else if (change.x < 0)
            {
                currentFace = PlayerFace.Left;
            }
            if (change.y > 0)
            {
                currentFace = PlayerFace.Up;
            }
            else if (change.y < 0)
            {
                currentFace = PlayerFace.Down;
            }
        }

        private IEnumerator AttackCo() //attack Coroutine
        {

            animator.SetBool("attacking", true);//start the attack anim
            currentState = PlayerState.attack;
            switch (currentFace)
            {
                case PlayerFace.Down:
                    HitBoxDown.SetActive(true);
                    break;
                case PlayerFace.Up:
                    HitBoxUp.SetActive(true);
                    break;
                case PlayerFace.Left:
                    HitBoxLeft.SetActive(true);
                    break;
                case PlayerFace.Right:
                    HitBoxRight.SetActive(true);
                    break;
            }


            yield return null;

            animator.SetBool("attacking", false);//stop the attack anim  
            yield return new WaitForSeconds(.3f);
            currentState = PlayerState.walk;
            HitBoxDown.SetActive(false);
            HitBoxUp.SetActive(false);
            HitBoxLeft.SetActive(false);
            HitBoxRight.SetActive(false);
        }

        void UpdateAnimationAndMove()//mooving function
        {
            if (change != Vector3.zero)//move only if player idle
            {
                UpdateFace();
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
                this.gameObject.SetActive(false);
            }

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
        }///*/


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
