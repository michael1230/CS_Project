using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChange : MonoBehaviour
{

    public AreaExit toForest;//the AreaExit to Forest map
    public AreaExit toDeserts;//the AreaExit to Deserts map
    public AreaExit toIce;//the AreaExit to Ice map
    public AreaExit toLast;//the AreaExit to Last map
    public Sign dialog;//a dialog box

    // Use this for initialization
    void Start ()
    {
		
	}	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.gameBeat==true)//if we have beaten the last boss
        {
            toForest.gameObject.SetActive(false);//cant go to Forest
            toDeserts.gameObject.SetActive(false);//cant go to Desert
            toIce.gameObject.SetActive(false);//cant go to Ice
            toLast.gameObject.SetActive(false);//cant go to Last
            //the dialog the old man gives
            dialog.dialogText2.text = "Im happy to see all of you survived";
            dialog.dialogText3.text = "now our world can be at peace";
        }
            if (GameManager.instance.numberOfElement==0)//if we only start the game
        {
            toForest.gameObject.SetActive(true);//can go to Forest only
            toDeserts.gameObject.SetActive(false);//cant go to Desert
            toIce.gameObject.SetActive(false);//cant go to ice
            toLast.gameObject.SetActive(false);//cant go to Last
            //the dialog the old man gives
            dialog.dialogText2.text = "you got the fire elemet!";
            dialog.dialogText3.text = "go save the other element users.. go first to the forest";
        }
        else if (GameManager.instance.numberOfElement == 1)//if we got the second element
        {
            toForest.gameObject.SetActive(false);//cant go to Forest
            toDeserts.gameObject.SetActive(true);//can go to Desert only
            toIce.gameObject.SetActive(false);//cant go to ice
            toLast.gameObject.SetActive(false);//cant go to Last
            //the dialog the old man gives
            dialog.dialogText2.text = "you saved Roselia";
            dialog.dialogText3.text = "now you can together to the desert and save Aiden";
        }
        else if (GameManager.instance.numberOfElement == 2)//if we got the third element
        {
            toForest.gameObject.SetActive(false);//cant go to Forest
            toDeserts.gameObject.SetActive(false);//cant go to Desert
            toIce.gameObject.SetActive(true);//can go to Ice only
            toLast.gameObject.SetActive(false);//cant go to Last
            //the dialog the old man gives
            dialog.dialogText2.text = "you saved Aiden";
            dialog.dialogText3.text = "now you can together to the ice land and save Sakura";
        }
        else if (GameManager.instance.numberOfElement == 3)//if we got the forth element
        {
            toForest.gameObject.SetActive(false);//cant go to Forest
            toDeserts.gameObject.SetActive(false);//cant go to Desert
            toIce.gameObject.SetActive(false);//cant go to ice
            //the dialog the old man gives
            toLast.gameObject.SetActive(true);//can go to Lat only
            dialog.dialogText2.text = "you saved all of them!";
            dialog.dialogText3.text = "you are all ready to face Garland in his Dark land";
        }
    }
}
