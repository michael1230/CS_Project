using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour {

    public Image[] hearts; //array of all the player hearts
   
    //reference to 3 different types of heart sprite
    public Sprite fullHeart; 
    public Sprite halfFullHeart;
    public Sprite emptyHeart;

    public FloatValue heartContainers; //for how many heart containers



    // Use this for initialization
    void Start () {
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

}
