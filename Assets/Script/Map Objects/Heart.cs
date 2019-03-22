using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Powerup
{
    public FloatValue playerHealth; //players current health
    public FloatValue heartContainers; //for not increasing the hearts more then there is
    public float amountToIncrease; //amount increace player health

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter2D(Collider2D other) //for player collecting the heart
    {
        if (other.CompareTag("Player") && !other.isTrigger) //if it is player
        {
            playerHealth.RuntimeValue += amountToIncrease; //increase health
            if (playerHealth.initialValue > heartContainers.RuntimeValue * 2f) //check heart container
            {
                playerHealth.initialValue = heartContainers.RuntimeValue * 2f;
            }
            powerupSignal.Raise(); //tell the ui to update the hearts
            Destroy(this.gameObject);
        }
    }

}
