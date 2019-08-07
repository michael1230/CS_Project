using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleTrigger : MonoBehaviour
{
    public string[] enemyToSpawn;//the enemies to spawn
    public KightsAttack knight;//the KightsAttack object

    // Use this for initialization
    void Start ()
    {
		
	}	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public void OnTriggerEnter2D(Collider2D other)//a method to make the player unable to move and then start the turn base battle if the player touched the enemy
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.MyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;//cant move
            StartCoroutine(BattleOn());//start the battle
        }
    }

    public IEnumerator BattleOn()//a Coroutine to start the battle
    {
        if (knight != null)//if the knight is not null...that means that this object is a knight 
        {
            knight.triggerBox.gameObject.SetActive(false);//turn of the triggerBox that starts the turn base battle
        }
        StartCoroutine(BattleManager.instance.PrepareforBattleStart(enemyToSpawn));//start the Coroutine PrepareforBattleStart with this enemies enemyToSpawn
        yield return new WaitForSeconds(3f);//
        this.gameObject.SetActive(false);//after we have waited we can turn off this object
    }


}



