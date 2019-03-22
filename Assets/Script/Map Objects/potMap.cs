using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potMap : MonoBehaviour {

    private Animator anim;// pot animation

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Smash()//break the pot
    {
        anim.SetBool("smash", true);
        StartCoroutine(breakCo());
    }
    
    IEnumerator breakCo()//makes the pot disaper from map but not destroyed
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);

    }
}
