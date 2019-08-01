using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBarManager : MonoBehaviour {

    public Slider fireSlider; //for the slider
    public MapInventory playerInventory; //the current amount of magic fire the player has according to the slider

    // Use this for initialization
    void Start ()
    {
        fireSlider.maxValue = playerInventory.maxMagic; //set to maxValue to maxMagic
        fireSlider.value = playerInventory.maxMagic; //first value of slider is maxium 
        playerInventory.currentMagic = playerInventory.maxMagic; //first value of playerInventory is maxium 
    }
	
    public void AddMagic() //add magic if potion is picked up
    {
        fireSlider.value += 3; //every time value +3 added to current value in the slider
        playerInventory.currentMagic += 3; //every time value +3 added to current playerInventory
        if (playerInventory.currentMagic > playerInventory.maxMagic) //if the amount added is bigger than the given max it will be updated to the max that chosen from start
        {
            fireSlider.value = playerInventory.maxMagic; //update slider
            playerInventory.currentMagic = playerInventory.maxMagic; //update playerInventory
        }
    }

    public void DecreaseMagic() //decrease magic if it was used
    {
        fireSlider.value -= 1; //decrea slider by 1
        playerInventory.currentMagic -= 1; //decrea playerInventory by 1
        if (fireSlider.value < 0) //if the amount less then zero update all to zero
        {
            fireSlider.value = 0; //update slider
            playerInventory.currentMagic = 0; //update playerInventory
        }
    }

}

