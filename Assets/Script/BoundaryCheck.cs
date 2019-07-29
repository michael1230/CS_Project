using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    public PatrolLog myEnemy;//the enemy object
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnTriggerEnter2D(Collider2D other) //check if the player entered the BoundaryCheck area 
    {
        if (other.CompareTag("Player"))
        {
            myEnemy.enterOrExit = true;//tells the enemy that the player is near 
        }
    }
    private void OnTriggerExit2D(Collider2D other) //check if the player exited the BoundaryCheck area 
    {
        if (other.CompareTag("Player"))
        {
            myEnemy.enterOrExit = false;//tells the enemy that the player is far 
        }
    }

}
