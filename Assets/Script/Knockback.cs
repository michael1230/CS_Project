using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public float thrust; //the force of knockback hit
    public float knockTime; //stops enemy after the hit
    public float damage; //damage from hit

    private void OnTriggerEnter2D(Collider2D other)  //kncok after checking the tag
    {
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))// tag for things that can be smashed and only by the player
        {
            other.GetComponent<potMap>().Smash();
        }
        if (other.gameObject.CompareTag("SmallMapEnemy") || other.gameObject.CompareTag("Player"))//tag for things that can be knock
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();//gets the rigidbody
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;//count how much to knock
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if (other.gameObject.CompareTag("SmallMapEnemy") && other.isTrigger)//knock of the enemys
                {
                    hit.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;//while get hit the state is stagger
                    other.GetComponent<EnemyOnMap>().knock(hit, knockTime, damage);//how much hit and the knock time and damage
                }
                if (other.gameObject.CompareTag("Player"))//knock of the player
                {
                    if (other.GetComponent<PlayerController>().currentState != PlayerState.stagger)//allow the knock back if not already stagger
                    {
                        hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;//while get hit the state is stagger
                        other.GetComponent<PlayerController>().knock(knockTime, damage);//how much hit, knock time and damage
                    }
                }
            }
        }
        /*
                var player = other.GetComponent<PlayerController>();
                var otherIsPlayer = player != null;
                var thisIsPlayer = this.GetComponentInParent<PlayerController>() != null;

                var enemy = other.GetComponent<EnemyOnMap>();
                var otherIsEnemy = enemy != null;
                var thisIsEnemy = this.GetComponentInParent<EnemyOnMap>() != null;


                var breakable = other.GetComponent<potMap>();
                var targetHasBreakable = breakable != null;

                if (targetHasBreakable && thisIsPlayer) // tag for things that can be smashed and only by the player
                {
                    breakable.Smash();
                }

                if ((otherIsPlayer && thisIsEnemy) || (thisIsPlayer && otherIsEnemy)) //tag for things that can be knock
                {
                    Rigidbody2D hit = other.GetComponent<Rigidbody2D>(); //gets the rigidbody
                    if (hit != null)
                    {
                        Vector2 difference = hit.transform.position - transform.position; //count how much to knock
                        difference = difference.normalized * thrust;
                        hit.AddForce(difference, ForceMode2D.Impulse);

                        if (otherIsEnemy && other.isTrigger) //knock of the enemys
                        {
                            enemy.currentState = EnemyState.stagger; //while get hit the state is stagger
                            enemy.knock(hit, knockTime, damage); //how much hit and the knock time and damage
                        }

                        if (otherIsPlayer) //knock of the player
                        {
                            if (player.currentState != PlayerState.stagger) //allow the knock back if not already stagger
                            {
                                player.currentState = PlayerState.stagger; //while get hit the state is stagger
                                player.knock(knockTime, damage); //how much hit, knock time and damage
                            }
                        }
                    }
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
                }
            }*/
    }
}
