using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;//the name of the scene to load

    public string areaTransitionName;//the name of: this map- exit number. like: forest-1//need to be the same name in the other Scene that connect to this one

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player")&&(areaToLoad!=""))
        {
            StartCoroutine(SceneSwitch());
        }
    }

    public IEnumerator SceneSwitch()
    {
        FadeManager.instance.ScenenTransition("ScenensFade");
        GameManager.instance.fadingBetweenAreas = true;
        // yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => FadeManager.instance.midTransition == true);
        SceneManager.LoadScene(areaToLoad);       
        GameManager.instance.fadingBetweenAreas = false;
        PlayerController.instance.areaTransitionName = areaTransitionName;
    }



    private void OnDrawGizmos()
    {
        if(shoulShowGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(1f,2f,0f));
        }

    }
}
