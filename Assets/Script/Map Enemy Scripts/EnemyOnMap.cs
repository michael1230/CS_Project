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
    public EnemyState currentState;//for changing states

    [Header("Enemy Stats")]
    public FloatValue maxHealth;//max health
    public float health;//current health
    public int baseAttack;//the force of enemy attack
    public float moveSpeed;//moving speed

    [Header("Death Effects")]
    public GameObject deathEffect;//reference to enemy death effect
    private float deathEffectDelay = 1f;//delay to enable seeing the death effect happen
    public LootTable thisLoot;//reference to the LootTable of heart/fireBall 

    private void Awake()//Initial value on wake
    {
        health = maxHealth.initialValue;//enemy starts with max health 
    }

    private void OnEnable()//Initial value when enabled
    {
        health = maxHealth.initialValue;
        currentState = EnemyState.idle;//first state enemy doesn't move
    }

    private void TakeDamage(float damage)//enemy damage after getting hit
    {
        health -= damage;
        if(health <= 0)//if no more health enemy is dead
        {
            DeathEffect();
            MakeLoot();//after death drop item
            this.gameObject.SetActive(false);//better then destroy for memory
        }
    }

    private void MakeLoot()//for hearts and powerUp
    {
        if (thisLoot != null)
        {
            Powerup current = thisLoot.LootPowerup();//get powerUp if probability gives one
            if (current != null)//if we got loot then
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);//drop in the enemy place, Quaternion.identity == no rotation 
            }
        }
    }

    private void DeathEffect()//effect for enemy death
    {
        if (deathEffect != null)//if there is a death effect for this enemy
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);//active the effect in the last place of the enemy that died
            Destroy(effect, deathEffectDelay);//after it is done it vanishes
        }
    }


    public void knock(Rigidbody2D myRigidbody, float knockTime, float damage)//enemy knock call function
    {
        if (this.gameObject == isActiveAndEnabled)//for stopping if enemy was killed
        {
            StartCoroutine(KnockCo(myRigidbody, knockTime));
        }
        TakeDamage(damage);//after knockBack give damage to enemy
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime) //knock the enemy
    {
        if (myRigidbody != null) //if there is myRigidbody for enemy
        {
            yield return new WaitForSecondsRealtime(knockTime);//for how long the knockBack
            currentState = EnemyState.idle;//change enemy state to idle for not attacking while been knock back
            myRigidbody.velocity = Vector2.zero;//let the Rigidbody move freely for the effect of the knock back
        }
    }
}
