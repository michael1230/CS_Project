using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;//the name of the AreaEntrance object
    public bool shoulShowGizmo;//a bool for showing Gizmo
    // Use this for initialization
    void Start()
    {
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;//get the name from the player object
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()//a method for showing Gizmo 
    {
        if (shoulShowGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }

    }
}
