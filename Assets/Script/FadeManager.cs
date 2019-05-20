using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* 0-Title
 * 1-GameOver
 * 2-Teleport
 * 3-ScenensFade1
 * 4-ScenensFade2
 * 5-ScenensFade3
 * 6-Battle1
 * 7-Battle2
 * 8-Battle3
 * 9-Battle4
 * n-FadeBlack
 */

public class FadeManager : MonoBehaviour {

    // Use this for initialization
    public static FadeManager instance;
    public Material transMaterial;
    public int currentTextureIndex;
    public List<Texture> textureList;
    public bool finishedTransition;
    public bool midTransition;


    private void Awake()
    {
        transMaterial = FindObjectOfType<SimpleBlit>().TransitionMaterial;
    }
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        transMaterial=FindObjectOfType<SimpleBlit>().TransitionMaterial;
        transMaterial.SetFloat("_Fade", 1f);
        transMaterial.SetFloat("_Cutoff", 0f);       
        transMaterial.SetColor("_Color",Color.black);
        midTransition = false;
        finishedTransition = false;
    }

    // Update is called once per frame
    void Update()
    {
        transMaterial = FindObjectOfType<SimpleBlit>().TransitionMaterial;
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            //cutoff = transMaterial.GetFloat("_Cutoff");
            //transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);           
            //Debug.Log(transMaterial.GetTexture("_TransitionTex").name);
            //startTransition("Title");
            //startTransition("GameOver");
            //startTransition("Teleport");
            //startTransition("ScenensFade");
            //startTransition("Battle");
            //startTransition("FadeBlack");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            startTransition("ScenensFade");
        }*/
    }

    public void ScenenTransition(string transitionEffect)
    {
        finishedTransition = false;
        midTransition = false;
        bool realTime = false;
        float duration = 1f;
        transMaterial.SetColor("_Color", Color.black);
        transMaterial.SetFloat("_Cutoff", 0f);
        transMaterial.SetFloat("_Fade", 1f);
        if (transitionEffect == "Title")
        {
            currentTextureIndex = 0;
        }
        else if (transitionEffect == "GameOver")
        {
            currentTextureIndex = 1;
            duration = 2f;
        }
        else if (transitionEffect == "Teleport")
        {
            currentTextureIndex = 2;
            transMaterial.SetColor("_Color", Color.white);//maybe a different color
        }
        else if (transitionEffect == "ScenensFade")
        {
            currentTextureIndex = Random.Range(3, 6);
        }
        else if (transitionEffect == "Load")
        {
            currentTextureIndex = 0;
            realTime = true;
        }
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
        StartCoroutine(TransitionAll(0, 1f, duration, "_Cutoff", realTime));
        //StartCoroutine(TransitionAll(0, 1f, 1f, "_Cutoff", realTime));
    }


    public void BattleTransition(string transitionEffect)
    {
        finishedTransition = false;
        midTransition = false;
        string fieldName = "_Cutoff";
        transMaterial.SetColor("_Color", Color.black);
        transMaterial.SetFloat("_Cutoff", 0f);
        transMaterial.SetFloat("_Fade", 1f);
        if (transitionEffect == "Battle")
        {
            currentTextureIndex = Random.Range(6, 10);
        }
        else if (transitionEffect == "FadeBlack")
        {
            currentTextureIndex = 0;
            transMaterial.SetFloat("_Cutoff", 1f);
            transMaterial.SetFloat("_Fade", 0f);
            fieldName = "_Fade";
        }
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
        StartCoroutine(TransitionAll(0, 1f, 1f, fieldName,false));
        }


    IEnumerator TransitionOnce(float oldValue, float newValue, float duration)
    {
        float value = 0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)//for loop to Fade in
        {
            value = Mathf.Lerp(oldValue, newValue, t / duration);
            transMaterial.SetFloat("_Cutoff", value);
            yield return null;
        }
        value = newValue;
        transMaterial.SetFloat("_Cutoff", value);
        yield return new WaitForSeconds(.3f);
        midTransition = true;
    }


    IEnumerator TransitionAll(float oldValue, float newValue, float duration, string fieldName,bool realtime)
    {
        float value = 0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)//for loop to Fade in
        {
            value = Mathf.Lerp(oldValue, newValue, t / duration);
            transMaterial.SetFloat(fieldName, value);
            yield return null;
        }
        value = newValue;
        transMaterial.SetFloat(fieldName, value);
        midTransition = true;
        if(realtime)
        {
            yield return new WaitForSecondsRealtime(2);
        }
        else
        {
            yield return new WaitForSeconds(.3f);
        }
        for (float t = 0f; t < duration; t += Time.deltaTime)//for loop to Fade out
        {
            value = Mathf.Lerp(newValue, oldValue, t / duration);
            transMaterial.SetFloat(fieldName, value);
            yield return null;
        }
        value = oldValue;
        transMaterial.SetFloat(fieldName, value);
        finishedTransition = true;
    }
}
