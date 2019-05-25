using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{

    public bool isPlayer;
    public bool isRegularEnemy;
    public bool isMapBoss;
    public bool isGameBoss;
    public List<BattleMove> movesAvailable;

    public string charName;
    //public int level;
    public float level;
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

    public bool effect1;
    public bool damageNumbers;//  
    public bool Idle;
    public bool goToWin;

    public bool hasDied;
    public int[] bounusTurn;//0->attack 1->defense
    public int[] statusBounus;//0->attack 1->defense

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;


    public bool move;
    private Transform moveTo;

    private bool shouldFade;
    public float fadeSpeed = 1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
        if(move==true)
        {
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].transform.position = Vector2.MoveTowards(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].transform.position, moveTo.position, 2.5f * Time.deltaTime);
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
    public void AttackEffectOn()
    {
        effect1 = true;
    }

    public void AttackDamageNumbers()
    {
        damageNumbers = true;
    }

    public void ReturnToIdle()
    {
        Idle= true;
    }
    public void GoToWinAim()
    {
        goToWin = true;
    }


    public void moveToPostion(Transform currentPos, Transform targetPos)
    {
        //if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].transform.position != targetPos.position)
        if (currentPos.transform.position != targetPos.position)
        {
            moveTo = targetPos;
            move = true;
        }
        else
        {
            move = false;
        }
    }

}
