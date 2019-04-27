using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Canvas theMenuCanves;
    public GameObject theMenu;
    public static GameMenu instance;

    //public List<GameObject> Elements;
    //public List<Image> partyImages;

    public GameObject[] Elements;
    //public Image[] partyImages;
    public GameObject[] partyImages;


    // Use this for initialization
    void Start ()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        theMenuCanves.worldCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (theMenu.activeInHierarchy)
            {
                theMenu.SetActive(false);
                GameManager.instance.gameMenuOpen = false;
                //CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
            AudioManager.instance.PlaySFX(5);
        }
    }

    public void UpdateMainStats()
    {
        int numberOfElement = GameManager.instance.numberOfElement;
        int activePartyMemberIndex = GameManager.instance.activePartyMemberIndex;

        for (int i = 0; i < numberOfElement; i++)
        {
            if (Elements[i].gameObject.activeInHierarchy==false)
            {
                Elements[i].SetActive(true);
                partyImages[i].SetActive(true);
            }
        }
        /*for (int i = 0; i < partyImages.Length; i++)
        {
            if (partyImages[i].gameObject.activeInHierarchy == false)
            {
                partyImages[i].SetActive(true);
            }
        }*/
    }
}
