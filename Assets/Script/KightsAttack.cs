using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KightsAttack : MonoBehaviour
{
    public GameObject player;//the player 
    public Sign triggerBox;//the Sign object
    private Animator animator; //reference the Animator
    // Use this for initialization
    void Start ()
    {
        player = FindObjectOfType<PlayerController>().gameObject;//find the player
        animator = GetComponent<Animator>();//get the Animator pf the player
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (triggerBox.once==true)////////////////////////////////////////////////////////////////////////////
        {
            animator.SetBool("move", true);////////////////////////////////////////////////////////////////////////////
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 7f * Time.deltaTime);////////////////////////////////////////////////////////////////////////////
        }
    }
}
