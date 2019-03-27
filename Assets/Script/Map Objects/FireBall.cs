using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public float speed;
    public Rigidbody2D myRigidbody;

	// Use this for initialization
	void Start () {
		
	}

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SmallMapEnemy"))
        {
            Destroy(this.gameObject);
        }
        
        
    }
}
