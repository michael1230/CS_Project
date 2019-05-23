using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleTrigger : MonoBehaviour
{
    public string[] enemyToSpawn;
    public bool canflee;
    public KightsAttack kight;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(BattleOn());
        }
    }

    public IEnumerator BattleOn()
    {
        if (kight!=null)
        {
            kight.triggerBox.gameObject.SetActive(false);
        }
        StartCoroutine(BattleManager.instance.PrepareforBattleStart(enemyToSpawn));        
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }


}



