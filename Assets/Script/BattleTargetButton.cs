using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour
{
    public BattleMove theMove;
    public BattleItem theItem;
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
        if(theItem==null)
            BattleManager.instance.PlayerAttack(theMove, activeBattlerTarget);
//else
    }
}
