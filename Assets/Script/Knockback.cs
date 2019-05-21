using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    //public Transform thisEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        var otherIsPlayer = player != null;
        var thisIsPlayer = this.GetComponentInParent<PlayerController>() != null;

        var enemy = other.GetComponent<EnemyOnMap>();
        var otherIsEnemy = enemy != null;
        var thisIsEnemy = this.GetComponentInParent<EnemyOnMap>() != null;

        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<potMap>().Smash();
        }

        if (otherIsEnemy && thisIsEnemy) //for 2 different enemys colliding but not to overlap
        {
            Rigidbody2D otherRigidBody = other.GetComponent<Rigidbody2D>();
            if (otherRigidBody != null)
            {
                Vector2 difference = otherRigidBody.transform.position - transform.position;
                difference = difference.normalized;
                otherRigidBody.AddForce(difference, ForceMode2D.Force);


            }
        }//|| (other.gameObject.CompareTag("FireBall") && thisIsEnemy) || (this.gameObject.CompareTag("FireBall") && otherIsEnemy)
        else if ((otherIsPlayer && thisIsEnemy) || (thisIsPlayer && otherIsEnemy) || (this.tag == "Player" && otherIsEnemy) || (other.gameObject.CompareTag("FireBall") && thisIsEnemy) || (this.gameObject.CompareTag("FireBall") && otherIsEnemy))//tag for things that can be knock
        {
            //StartCoroutine(waitfortime());
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            // thisEnemy.position = new Vector2(thisEnemy.transform.position.x - 5, thisEnemy.transform.position.y - 5);
            if (hit != null)
            {
                //transform.position = new Vector2(transform.position.x-5, transform.position.y-5);
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("SmallMapEnemy") && other.isTrigger)
                {
                    hit.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;
                    other.GetComponent<EnemyOnMap>().knock(hit, knockTime, damage);
                }
                if (other.gameObject.CompareTag("Player"))
                {
                    if (other.GetComponent<PlayerController>().currentState != PlayerState.stagger)
                    {
                        hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;
                        other.GetComponent<PlayerController>().knock(knockTime, damage);
                    }
                }
            }
        }
        /*
        else if(other.tag=="tempAttack" || other.gameObject.CompareTag("FireBall"))
        {
            //Rigidbody2D hit = this.GetComponent<PatrolLog>().myRigidbody;
        
            //Vector2 difference = other.transform.position - transform.position;
            //Vector2 difference = transform.position - other.transform.position;
            if (this.gameObject == isActiveAndEnabled)//for stopping if enemy was killed
            {
                StartCoroutine(waitfortime());
            }
            //StartCoroutine(waitfortime());
            //this.GetComponent<EnemyOnMap>().knock(hit, other.GetComponent<Knockback>().knockTime, other.GetComponent<Knockback>().damage);
        }

    }


    public IEnumerator waitfortime()
    {
        //yield return new WaitForSecondsRealtime(2);
        //float movespeed= this.GetComponent<EnemyOnMap>().moveSpeed;//פעם שנייה זה אפס לכן צריך להיות משהו מוגדר
        float movespeed = 2;//אולי לשמור בגיימאנגר או משהו 
        //this.GetComponent<EnemyOnMap>().moveSpeed = 0;
        Rigidbody2D hit = this.GetComponent<Rigidbody2D>();
        Vector2 difference =  GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().transform.position- hit.transform.position ;
        difference = difference.normalized * thrust;
        hit.AddForce(difference, ForceMode2D.Impulse);
        //hit.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;
        //this.GetComponent<EnemyOnMap>().knock(hit, knockTime, damage);
        //yield return new WaitForSeconds(knockTime);
       
        //currentState = EnemyState.idle;
      
        yield return new WaitForSecondsRealtime(2);
        hit.velocity = Vector2.zero;
        hit.GetComponent<EnemyOnMap>().currentState = EnemyState.idle;
        hit.velocity = Vector2.zero;
        Debug.Log("here");
        //this.GetComponent<EnemyOnMap>().moveSpeed = movespeed;
    }
    */
    }
}

