using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;//the name of the scene to load
    public string areaTransitionName;//the name of: this map- exit number. like: forest-1//need to be the same name in the other Scene that connect to this one
    public AreaEntrance theEntrance;//the AreaEntrance object (son of this object in Unity)
    public bool shoulShowGizmo;
    // Use this for initialization
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;

    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)//if Collider2D enter this object
    {
        if ((other.tag == "Player")&&(areaToLoad!=""))//if the Collider2D is the player and areaToLoad is not empty(allow to be empty for tests)
        {
            StartCoroutine(SceneSwitch());//start the Coroutine
        }
    }
    public IEnumerator SceneSwitch()//the Coroutine for scene switching
    {
        FadeManager.instance.ScenenTransition("ScenensFade");//call FadeManager ScenenTransition method with ScenensFade effect
        GameManager.instance.fadingBetweenAreas = true;//tell GameManager that we are fading Between Areas
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);//with until the screen is black 
        SceneManager.LoadScene(areaToLoad);//load the scene(its name is the areaToLoad)     
        GameManager.instance.fadingBetweenAreas = false;//tell GameManager that we are finished fading Between Areas
        PlayerController.instance.areaTransitionName = areaTransitionName;//update the areaTransitionName of the player
    }
    private void OnDrawGizmos()//a method for showing gizmo
    {
        if(shoulShowGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(1f,2f,0f));
        }

    }
}
