using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{

    public bool isPlayer;//a bool to know if this is a player
    public bool isRegularEnemy;//a bool to know if this is a Regular Enemy
    public bool isMapBoss;//a bool to know if this is a MapBoss
    public bool isGameBoss;//a bool to know if this is a GameBoss
    public List<BattleMove> movesAvailable;//a list of moves thats available now
    public string charName;//the name
    public float level;//the level
    public int currentHP;//the current HP
    public int maxHP;//the max HP
    public int currentMP;//the current MP
    public int maxMP;//the max MP
    public int currentSP;//the current SP
    public int maxSP;//the max SP
    public int strength;//the strength..the attack power
    public int defense;//the defense..the defense power
    public int dexterity;//didn't use in the final game
    public Animator anim;//this object Animator
    public bool effect1;//a bool to know when to play the effect
    public bool damageNumbers;//a bool to know when to show the damageNumbers
    public bool Idle;//a bool to know when to return to idle state(when the anim to the move is finished)
    public bool goToWin;//a bool to know when to go to win state(when the anim of winFull is finished)
    public int[] bounusTurn;//an array thats tell the amount of turns left for the bonus...0->attack 1->defense
    public int[] statusBounus;//an array of the bonus itself 0->attack 1->defense
    public SpriteRenderer theSprite;//this object SpriteRenderer
    public bool move;//a bool to know if this object still movers(didn't reach the distention)
    private Transform moveTo;//where to move
    private bool shouldFade;//a bool to know if this object is dead and needs to fade out
    public float fadeSpeed = 1f;//the speed of the fade

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)//if we need to fade
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            //the color is changed to red and the a is going down to zero with Mathf.MoveTowards and the speed is fadeSpeed * Time.deltaTime
            if (theSprite.color.a == 0)//when its 0 that means we dont see it
            {
                gameObject.SetActive(false);//turn this object off
            }
        }
        if(move==true)//if we need to move
        {
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].transform.position = Vector2.MoveTowards(BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].transform.position, moveTo.position, 2.5f * Time.deltaTime);
            //keep moving
        }
    }

    public void EnemyFade()//a method to start the fade out
    {
        shouldFade = true;
    }
    public void AttackEffectOn()//a method to start the effect
    {
        effect1 = true;
    }
    public void AttackDamageNumbers()//a method to show the damageNumbers
    {
        damageNumbers = true;
    }
    public void ReturnToIdle()//a method to start the return to idle
    {
        Idle= true;
    }
    public void GoToWinAim()//a method to start the win anim
    {
        goToWin = true;
    }
    public void moveToPostion(Transform currentPos, Transform targetPos)//a method to moves to position 
    {
        if (currentPos.transform.position != targetPos.position)//if we not at the target position then
        {
            moveTo = targetPos;//the moveTo is the targetPos
            move = true;//we still need to move
        }
        else//if we arrived then
        {
            move = false;//stop moving
        }
    }
}
