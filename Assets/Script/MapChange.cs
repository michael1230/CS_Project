using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChange : MonoBehaviour
{

    public AreaExit toForest;
    public AreaExit toDeserts;
    public AreaExit toIce;
    public AreaExit toLast;
    public Sign dialog;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.gameBeat==true)
        {
            toForest.gameObject.SetActive(false);
            toDeserts.gameObject.SetActive(false);
            toIce.gameObject.SetActive(false);
            toLast.gameObject.SetActive(false);
            dialog.dialogText2.text = "Im happy to see all of you survived";
            dialog.dialogText3.text = "now our world can be at peace";
        }
            if (GameManager.instance.numberOfElement==0)
        {
            toForest.gameObject.SetActive(true);
            toDeserts.gameObject.SetActive(false);
            toIce.gameObject.SetActive(false);
            toLast.gameObject.SetActive(false);
            dialog.dialogText2.text = "you got the fire elemet!";
            dialog.dialogText3.text = "go save the other element users.. go first to the forest";


        }
        else if (GameManager.instance.numberOfElement == 1)
        {
            toForest.gameObject.SetActive(false);
            toDeserts.gameObject.SetActive(true);
            toIce.gameObject.SetActive(false);
            toLast.gameObject.SetActive(false);
            dialog.dialogText2.text = "you saved Roselia";
            dialog.dialogText3.text = "now you can together to the desert and save Aiden";
        }
        else if (GameManager.instance.numberOfElement == 2)
        {
            toForest.gameObject.SetActive(false);
            toDeserts.gameObject.SetActive(false);
            toIce.gameObject.SetActive(true);
            toLast.gameObject.SetActive(false);
            dialog.dialogText2.text = "you saved Aiden";
            dialog.dialogText3.text = "now you can together to the ice land and save Sakura";
        }
        else if (GameManager.instance.numberOfElement == 3)
        {
            toForest.gameObject.SetActive(false);
            toDeserts.gameObject.SetActive(false);
            toIce.gameObject.SetActive(false);
            toLast.gameObject.SetActive(true);
            dialog.dialogText2.text = "you saved all of them!";
            dialog.dialogText3.text = "you are all ready to face Garland in his Dark land";
        }
    }
}
