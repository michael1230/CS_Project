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
    public static FadeManager instance;//the FadeManager object itself
    public Material transMaterial;//a Material object
    public int currentTextureIndex;//the Texture index that we want to show
    public List<Texture> textureList;//a list of Textures
    public bool finishedTransition;//if we have finished the transition
    public bool midTransition;//if we have reached mid transition(if the screen is black)


    private void Awake()
    {
        transMaterial = FindObjectOfType<SimpleBlit>().TransitionMaterial;
    }
    void Start()//reset values
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
    }
    public void ScenenTransition(string transitionEffect)//a method to start a Transition between scenes 
    {
        finishedTransition = false;//reset
        midTransition = false;//reset
        bool realTime = false;//if we want to wait in real time
        float duration = 1f;//how fast the screen is turning black
        transMaterial.SetColor("_Color", Color.black);//to what color to turn..mosly black
        transMaterial.SetFloat("_Cutoff", 0f);//reset
        transMaterial.SetFloat("_Fade", 1f);//reset
        if (transitionEffect == "Title")//if the effect is Title
        {
            currentTextureIndex = 0;//show the first Texture
        }
        else if (transitionEffect == "GameOver")//if the effect is GameOver
        {
            currentTextureIndex = 1;//show the second Texture
            duration = 2f;//more time
        }
        else if (transitionEffect == "Teleport")//if the effect is Teleport
        {
            currentTextureIndex = 2;//show the third Texture
            transMaterial.SetColor("_Color", Color.white);//different color to Teleport(wasn't use in the final game)
        }
        else if (transitionEffect == "ScenensFade")//if the effect is ScenensFade
        {
            currentTextureIndex = Random.Range(3, 6);//randomly choose the Texture between the 4 til 6
        }
        else if (transitionEffect == "Load")//if the effect is Load
        {
            currentTextureIndex = 0;//show the first Texture
            realTime = true;//wait in real time
        }
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);//set the Material
        StartCoroutine(TransitionAll(0, 1f, duration, "_Cutoff", realTime));//start the Coroutine
    }
    public void BattleTransition(string transitionEffect)//a method to start a Transition between battles 
    {
        finishedTransition = false;//reset
        midTransition = false;//reset
        string fieldName = "_Cutoff";//what to change in the Material
        transMaterial.SetColor("_Color", Color.black);//to what color to turn..mosly black
        transMaterial.SetFloat("_Cutoff", 0f);//reset
        transMaterial.SetFloat("_Fade", 1f);//reset
        if (transitionEffect == "Battle")//if the effect is Battle
        {
            currentTextureIndex = Random.Range(6, 10);//randomly choose the Texture between the 7 til 10
        }
        else if (transitionEffect == "FadeBlack")//if the effect is FadeBlack
        {
            currentTextureIndex = 0;//show the first Texture
            transMaterial.SetFloat("_Cutoff", 1f);//start with 1 
            transMaterial.SetFloat("_Fade", 0f);//start with 0
            fieldName = "_Fade";//the parameter to change is _Fade
        }
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);//set the Material
        StartCoroutine(TransitionAll(0, 1f, 1f, fieldName,false));//start the Coroutine
    }
    IEnumerator TransitionOnce(float oldValue, float newValue, float duration)//a Transition to black and not back..for tests
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
    IEnumerator TransitionAll(float oldValue, float newValue, float duration, string fieldName,bool realtime)//Transition to black with the effect and back to normal 
    {
        float value = 0f;//reset
        for (float t = 0f; t < duration; t += Time.deltaTime)//for loop to Fade in
        {
            value = Mathf.Lerp(oldValue, newValue, t / duration);
            transMaterial.SetFloat(fieldName, value);
            yield return null;
        }
        value = newValue;
        transMaterial.SetFloat(fieldName, value);
        midTransition = true;//we have a black screen here
        if(realtime)//if realtime is true then wait real second(give the game time to load stuff)
        {
            yield return new WaitForSecondsRealtime(2);
        }
        else//if not then wait normally
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
        finishedTransition = true;//the Transition is finished 
    }
}
