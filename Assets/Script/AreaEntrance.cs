using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{

    public string transitionName;

    public bool shoulShowGizmo;

    // Use this for initialization
    void Start()
    {
        if (transitionName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
        }

       // UIFade.instance.FadeFromBlack();
        //GameManager.instance.fadingBetweenAreas = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if (shoulShowGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }

    }
}
