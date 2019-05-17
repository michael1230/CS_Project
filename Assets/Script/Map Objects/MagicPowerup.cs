using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : Powerup {

	// Use this for initialization
	void Start () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
       if(other.gameObject.CompareTag("Player"))
        {
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
