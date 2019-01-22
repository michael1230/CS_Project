using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;//the name of the move we want to activate
    public int activeBattlerTarget;//the target we selected 
    public Text targetName;//the name of the target
   // public Button theButton;
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void Press()
    {
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
