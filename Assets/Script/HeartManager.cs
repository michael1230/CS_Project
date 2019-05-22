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
        /*for (int i = 0; i < heartContainers.initialValue; i++) //go throughout all the heart array
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }*/
        for (int i = 0; i < 5; i++) //go throughout all the heart array
        {
            if (i< heartContainers.initialValue)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHearts() //update the number of hearts after damage
    {
        float tempHealth = playerCurrentHealth.RuntimeValue / 2; //divide by 2 because there is a half heart
        for (int i = 0; i < heartContainers.initialValue; i++) //go throughout all the heart array
        {
            if (i <= tempHealth - 1)
            {
                //Full Heart
                hearts[i].sprite = fullHeart;
            }
            else if (i >= tempHealth)
            {
                //empty heart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //half full heart
                hearts[i].sprite = halfFullHeart;
            }
        }

    }

}

