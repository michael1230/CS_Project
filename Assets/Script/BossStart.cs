using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    public GralandChase Boss;
    public bool once=false;
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
            Boss.gameObject.SetActive(true);
            if (once)
            {
                Boss.moveSpeed = 0;
            }
        }
    }
}
