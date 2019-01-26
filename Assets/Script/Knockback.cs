using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float thrust; //the force of rain knockback hit
    public float knockTime; //stops enemy after the hit

    private void OnTriggerEnter2D(Collider2D other)  //kncok the enemy after checking the tag
    {
        if (other.gameObject.CompareTag("breakable"))// tag for things that can be hit
        {
            other.GetComponent<potMap>().Smash();
        }
        if (other.gameObject.CompareTag("SmallMapEnemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if(other.gameObject.CompareTag("SmallMapEnemy"))
                {
                    hit.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;
                    other.GetComponent<EnemyOnMap>().knock(hit, knockTime);
                }
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(hit));
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy) //knock the enemy not too far away
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            //enemy.GetComponent<EnemyOnMap>().currentState = EnemyState.idle;
        }
    }
}
