using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;

    // Use this for initialization
    void Start ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("here1");
            if (theMenu.activeInHierarchy)
            {
                theMenu.SetActive(false);
                //GameManager.instance.gameMenuOpen = false;
                Debug.Log("here2");
                //CloseMenu();
            }
            else
            {
                Debug.Log("here3");
                theMenu.SetActive(true);
               // UpdateMainStats();
                //GameManager.instance.gameMenuOpen = true;
            }

            AudioManager.instance.PlaySFX(5);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
