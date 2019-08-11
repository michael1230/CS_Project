using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : Powerup {

	// Use this for initialization
	void Start () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)//when player pick ups powerUp
    {
       if(other.gameObject.CompareTag("Player"))//if it is the player
        {
            powerupSignal.Raise();//send signal
            Destroy(this.gameObject);//erase on the map
        }
    }
}
