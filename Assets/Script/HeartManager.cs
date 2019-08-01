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
    public FloatValue playerCurrentHealth; //for player current health


    // Use this for initialization
    void Start () {
        InitHearts();
    }

    public void InitHearts() //initiate all the hearts as full hearts
    {
        for (int i = 0; i < 5; i++) //go throughout all the heart array
        {
            if (i< heartContainers.initialValue) //for each map update differnt amount of hearts
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeart; //all starts with full heart
            }
            else
            {
                hearts[i].gameObject.SetActive(false); //if not needed in the than the gameObject set to not active
            }
        }
    }

    public void UpdateHearts() //update the number of hearts after damage
    {
        float tempHealth = playerCurrentHealth.RuntimeValue / 2; //divide by 2 because there is a half heart
        for (int i = 0; i < heartContainers.initialValue; i++) //go throughout all the hearts we have in the map array
        {//example if  tempHealth = playerCurrentHealth.RuntimeValue/2   1.5 = 3/2
            if (i <= tempHealth - 1)//i=0 1.5-1 = 0.5  full heart
            {//i=1 not smaller than 0.5
                //Full Heart
                hearts[i].sprite = fullHeart;
            }//i=1 not bigger than 1.5
            else if (i >= tempHealth) //1.5
            {
                //empty heart
                hearts[i].sprite = emptyHeart;
            }
            else //so we get 1 full heart and half heart
            {
                //half full heart
                hearts[i].sprite = halfFullHeart;
            }
        }

    }

}

