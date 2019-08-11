using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Powerup
{
    public FloatValue playerHealth;//players current health (every heart is 2 health points)
    public FloatValue heartContainers;//the max hearts shown on the scene which is player health
    public float amountToIncrease;//amount to increase player health

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter2D(Collider2D other)//for player collecting the heart
    {
        if (other.CompareTag("Player") && !other.isTrigger)//if it is player
        {
            playerHealth.RuntimeValue += amountToIncrease;//increase health
            if (playerHealth.RuntimeValue > heartContainers.RuntimeValue * 2f)//check if heart container is full
            {
                playerHealth.RuntimeValue = heartContainers.RuntimeValue * 2f;//set player health to max that can be 
            }
            powerupSignal.Raise();//tell the ui to update the hearts
            Destroy(this.gameObject);//erase the heart on map
        }
    }

}
