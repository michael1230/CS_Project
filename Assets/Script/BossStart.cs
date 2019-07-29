using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    public GralandChase Boss;//the boos object
    public bool once=false;//only once
    // Use this for initialization
    void Start ()
    {
		
	}	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnTriggerEnter2D(Collider2D other)//check if the player entered the boss start object 
    {
        if (other.CompareTag("Player"))
        {
            Boss.gameObject.SetActive(true);//activate the boss
            if (once)
            {
                Boss.moveSpeed = 0;//start tieh speed of 0
            }
        }
    }
}
