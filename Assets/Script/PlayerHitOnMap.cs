﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitOnMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("breakable"))// tag for things that can be hit
        {
            other.GetComponent<potMap>().Smash();
        }
    }
}