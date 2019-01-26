using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{

    public bool isPlayer;
    public bool isMapBoss;
    public bool isGameBoss;
    public List<BattleMove> movesAvailable;

    public string charName;
    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int currentSP;
    public int maxSP;
    public int strength;
    public int defense;
    public int dexterity;

    public Animator anim;

    public bool effect;
    public bool Idle;

    public bool hasDied;
    public int[] bounusTurn;//0->attack 1->defense
    public int[] statusBounus;//0->attack 1->defense

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

   // private bool shouldFade;
   // public float fadeSpeed = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (shouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }*/
    }

   /* public void EnemyFade()
    {
        shouldFade = true;
    }*/

    public void AttackEffectOn()
    {
        Debug.Log("on");
        effect= true;
    }
    public void ReturnToIdle()
    {
        Debug.Log("idle");
        Idle= true;
    }

    public bool AttackEffectCheck()
    {
        return effect;
    }
    public bool ReturnToIdleCheck()
    {
        return Idle;
    }


}
