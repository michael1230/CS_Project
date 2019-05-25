using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChange : MonoBehaviour
{

    public AreaExit toMaps;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.numberOfElement==0)
        {
            toMaps.areaToLoad = "";
            toMaps.areaTransitionName = "";
        }
        else if (GameManager.instance.numberOfElement == 1)
        {

        }
        else if (GameManager.instance.numberOfElement == 2)
        {

        }
        else if (GameManager.instance.numberOfElement == 3)
        {

        }
    }
}
