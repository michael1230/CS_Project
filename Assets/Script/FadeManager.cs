using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {

    // Use this for initialization

    public static FadeManager instance;
    public Material transMaterial;

    private float cutoff;

    public AnimationCurve curve;

    public float realCutoff;

    public float speedMultiplier;

    public int currentTextureIndex;

    public List<Texture> textureList;

   

    void Start()
    {

        cutoff = transMaterial.GetFloat("_Cutoff");
        currentTextureIndex = Random.Range(0,9);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cutoff);
        if (Input.GetKeyDown(KeyCode.A))
        {
            cutoff = transMaterial.GetFloat("_Cutoff");
            transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
            StartCoroutine(Transition(cutoff,1f,2f));
            Debug.Log(transMaterial.GetTexture("_TransitionTex").name); 
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cutoff = transMaterial.GetFloat("_Cutoff");
            transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
            StartCoroutine(Transition(cutoff, 0f, 2f));
            Debug.Log(transMaterial.GetTexture("_TransitionTex").name);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentTextureIndex = Random.Range(0, 9);
        }

    }

    public void startBattleTransIn()
    {
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
        StartCoroutine(Transition(cutoff, 1f, 2f));
    }
    public void endBattleTransIn()
    {
        transMaterial.SetTexture("_TransitionTex", textureList[currentTextureIndex]);
        StartCoroutine(Transition(cutoff, 0f, 2f));
    }

    public IEnumerator Transition(float oldValue, float newValue, float duration)
    {
        float cutoff = 0f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            cutoff = Mathf.Lerp(oldValue, newValue, t / duration);
            transMaterial.SetFloat("_Cutoff", cutoff);
            yield return null;
        }
        cutoff = newValue;
        transMaterial.SetFloat("_Cutoff", cutoff);
        yield return true;
    }
}
