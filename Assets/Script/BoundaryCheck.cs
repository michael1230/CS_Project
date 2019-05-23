using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCheck : MonoBehaviour
{
    public PatrolLog myEnemy;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    private void OnTriggerEnter2D(Collider2D other) //check if the player entered the sign area 
    {
        if (other.CompareTag("Player"))
        {
            myEnemy.enterOrExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) //check if the player exited the sign area 
    {
        if (other.CompareTag("Player"))
        {
            myEnemy.enterOrExit = false;
        }
    }

}
