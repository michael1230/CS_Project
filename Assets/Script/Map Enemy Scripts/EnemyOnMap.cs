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

    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public bool isAlive;
 //   public Vector2 homePosition;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;

    private void Awake()
    {
        health = maxHealth.initialValue;
        isAlive = true;
    }

    private void OnEnable()
    {
        //transform.position = homePosition;
        health = maxHealth.initialValue;
        currentState = EnemyState.idle;
    }

    private void TakeDamage(float damage) //enemy damage after hit
    {
        health -= damage;
        if(health <= 0)
        {
            DeathEffect();
            MakeLoot();//after death drop item
            this.gameObject.SetActive(false); //better then destroy for memory
            isAlive = false;
        }
    }

    private void MakeLoot()// for hearts and powerUp
    {
        if (thisLoot != null)
        {
            Powerup current = thisLoot.LootPowerup();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);//drop in the enemy place, Quaternion.identity == no rotation 
            }
        }
    }

    private void DeathEffect() //effect for enemy death
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }


    public void knock(Rigidbody2D myRigidbody, float knockTime, float damage)//enemy knock call function
    {
        if (this.gameObject == isActiveAndEnabled)//for stopping if enemy was killed
        {
            StartCoroutine(KnockCo(myRigidbody, knockTime));
        }
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime) //knock the enemy
    {
        if (myRigidbody != null)
        {
            //yield return new WaitForSeconds(knockTime);
            yield return new WaitForSecondsRealtime(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
