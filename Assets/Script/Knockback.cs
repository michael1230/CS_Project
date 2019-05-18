using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

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
        }
        else if ((otherIsPlayer && thisIsEnemy) || (thisIsPlayer && otherIsEnemy) || (other.gameObject.CompareTag("FireBall") && thisIsEnemy) || (this.gameObject.CompareTag("FireBall") && otherIsEnemy))//tag for things that can be knock
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
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
    }
}

