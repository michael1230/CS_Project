using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour {

    public float effectLength;//how much it will last
    public int soundEffect;//the sfx for the attack

    // Use this for initialization
    void Start()
    {
        AudioManager.instance.PlaySFX(soundEffect);//play that sound effect
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, effectLength);//Destroy this object after effectLength
    }
    void PlayThisEffectSound()//play again for some attacks
    {
        AudioManager.instance.PlaySFX(soundEffect);
    }

}
