using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;//the force that player knocks things back
    public float knockTime;//time for how long is the knockBack 
    public float damage;//the damage from the knockBack

    private void OnTriggerEnter2D(Collider2D other)//start when another object enters a trigger collider attached to this object
    {
        var player = other.GetComponent<PlayerController>();//get reference to player
        var otherIsPlayer = player != null;//if other is player
        var thisIsPlayer = this.GetComponentInParent<PlayerController>() != null;//else if this is player then get reference to player, which is parent 

        var enemy = other.GetComponent<EnemyOnMap>();//get reference to enemy
        var otherIsEnemy = enemy != null;//if other is enemy
        var thisIsEnemy = this.GetComponentInParent<EnemyOnMap>() != null;//else if this is enemy then get reference to player

        if (otherIsEnemy && thisIsEnemy)//for 2 different enemies colliding but not to overlap
        {
            Rigidbody2D otherRigidBody = other.GetComponent<Rigidbody2D>();//get reference to other enemy Rigidbody2D to move it position
            if (otherRigidBody != null)//if it has a Rigidbody2D
            {
                Vector2 difference = otherRigidBody.transform.position - transform.position;//difference between the Vectors of otherIsEnemy to thisIsEnemy
                difference = difference.normalized;//normalized the difference, makes it length of 1 but keeps the same direction
                otherRigidBody.AddForce(difference, ForceMode2D.Force);//force is applied continuously along the direction of the force vector
            }
        }
        else if ((otherIsPlayer && thisIsEnemy) || (thisIsPlayer && otherIsEnemy) || (this.tag == "Player" && otherIsEnemy) || (other.gameObject.CompareTag("FireBall") && thisIsEnemy) || (this.gameObject.CompareTag("FireBall") && otherIsEnemy))//knock back between enemy and player or enemy and fireBall
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();//get reference to other Rigidbody2D to move it
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;//difference between the Vectors 
                difference = difference.normalized * thrust;//normalized the difference and add the thrust we want our enemy or player to be knockback
                hit.AddForce(difference, ForceMode2D.Impulse);//force is applied continuously along the direction of the force vector
                if (other.gameObject.CompareTag("SmallMapEnemy") && other.isTrigger)//if the enemy is the one been knockback and is not already been knockback then 
                {
                    hit.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;//change enemy state to stagger while been knockback
                    other.GetComponent<EnemyOnMap>().knock(hit, knockTime, damage);//active the knock in EnemyOnMap script and give it the hit, time and damage
                }
                if (other.gameObject.CompareTag("Player") && other.GetComponent<PlayerController>().currentState != PlayerState.stagger)//if the player is the one been knockback and is not already been knockback then 
                {
                    hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;//change player state to stagger while been knockback
                    other.GetComponent<PlayerController>().knock(knockTime, damage);//active the knock in PlayerController script and give it the time and damage
                }
            }
        }
    }
}

