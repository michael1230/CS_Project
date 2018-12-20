using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public float walkSpeed;
    public float runSpeed;
    public Rigidbody2D theRB;
    public Animator myAnim;
    public static PlayerController instance;
    public bool canMovePlayer = true;

    public string areaTransitionName;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

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
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * walkSpeed;

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        


    }
}
