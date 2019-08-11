using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public float speed;//the speed of the fireBall
    public Rigidbody2D myRigidbody;//fireBall Rigidbody2D

    // Use this for initialization
    void Start () {
		
	}
    void Update()
    {
        Object.Destroy(gameObject, 2.0f);//fireBall is destroy after 5 seconds
    }

    public void Setup(Vector2 velocity, Vector3 direction)//set up for the fire ball when it's shot
    {
        myRigidbody.velocity = velocity.normalized * speed;//moving the rigidbody of fireBall with speed and normalize it for moving the same speed in all direction
        transform.rotation = Quaternion.Euler(direction);//change the rotation of the fireBall in the correct direction 
    }

    public void OnTriggerEnter2D(Collider2D other)//for hitting the enemy
    {
        if(other.gameObject.CompareTag("SmallMapEnemy"))//if its the enemy it is destroyed
        {
            Destroy(this.gameObject);
        }
        
        
    }
}
