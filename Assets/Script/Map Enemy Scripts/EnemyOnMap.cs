using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class EnemyOnMap : MonoBehaviour {

    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }
    private void TakeDamage(float damage) //enemy damage after hit
    {
        health -= damage;
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //better then destroy for memory
        }
    }

    public void knock(Rigidbody2D myRigidbody, float knockTime, float damage)//enemy knock call function
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime) //knock the enemy
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
