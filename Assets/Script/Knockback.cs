using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour {

    public float thrust; //the force of rain knockback hit
    public float knockTime; //stops enemy after the hit

    private void OnTriggerEnter2D(Collider2D other)  //kncok the enemy after checking the tag
    {
        if (other.gameObject.CompareTag("SmallMapEnemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                enemy.GetComponent<EnemyOnMap>().currentState = EnemyState.stagger;
                Vector2 difference = enemy.transform.position - transform.position;
                difference = difference.normalized * thrust;
                enemy.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(enemy));
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody2D enemy) //knock the enemy not too far away
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemy.velocity = Vector2.zero;
            enemy.GetComponent<EnemyOnMap>().currentState = EnemyState.idle;
        }
    }
}
