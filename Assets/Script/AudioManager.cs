using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx;//the sound effects array
    public AudioSource[] bgm;//the game music array
    public static AudioManager instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void PlaySFX(int soundToPlay)//a method to play the sound effects
    {
        if (soundToPlay < sfx.Length)//if we in the array bounds then
        {
            sfx[soundToPlay].Play();//play the effects
        }
    }
    public void PlayBGM(int musicToPlay)//a method to play the game music
    {
        if (!bgm[musicToPlay].isPlaying)//if we are not playing the bgm[musicToPlay] right now then
        {
            StopMusic();//stop playing
            if (musicToPlay < bgm.Length)//if we in the array bounds then
            {
                bgm[musicToPlay].Play();//play the music
            }
        }
    }
    public void StopMusic()//a method for stop playing all the music
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
