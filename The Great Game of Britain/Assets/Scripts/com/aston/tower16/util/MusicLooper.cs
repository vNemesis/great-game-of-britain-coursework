using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    //array of sound clips
    public AudioClip[] soundClips = new AudioClip[3];
    private int musicNumber;
    private bool playNextMusic;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        musicNumber = 0;
        playNextMusic = true;
        audio = GameObject.Find("SceneMusic").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playNextMusic)
        {
            StartCoroutine(PlayNextTune());
        }
    }

    public IEnumerator PlayNextTune()
    {
        Debug.Log("PlayNextTune()");
        playNextMusic = false;
        audio.clip = soundClips[musicNumber];
        Debug.Log("Playing music");
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        playNextMusic = true;
        musicNumber++;
        if (musicNumber > 2)
        {
            musicNumber = 0;
        }
        Debug.Log("Next Track");
      
    }

}
