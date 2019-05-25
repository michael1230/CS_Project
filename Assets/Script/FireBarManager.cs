using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBarManager : MonoBehaviour {

    public Slider fireSlider;//for the slider
    public MapInventory playerInventory;

	// Use this for initialization
	void Start ()
    {
        fireSlider.maxValue = playerInventory.maxMagic;
        fireSlider.value = playerInventory.maxMagic;
        playerInventory.currentMagic = playerInventory.maxMagic;
	}
	
    public void AddMagic()
    {
        fireSlider.value += 3;
        playerInventory.currentMagic += 3;//how much magic Fire player gets
        /*
        if (fireSlider.value > fireSlider.maxValue)
        {
            fireSlider.value = fireSlider.maxValue;
            playerInventory.currentMagic = playerInventory.maxMagic;
        }
        */
        if (playerInventory.currentMagic > playerInventory.maxMagic)
        {
            fireSlider.value = playerInventory.maxMagic;
            playerInventory.currentMagic = playerInventory.maxMagic;
        }
    }

    public void DecreaseMagic()
    {
        fireSlider.value -= 1;
        playerInventory.currentMagic -= 1;
        if (fireSlider.value < 0)
        {
            fireSlider.value = 0;
            playerInventory.currentMagic = 0;
        }
    }

}

