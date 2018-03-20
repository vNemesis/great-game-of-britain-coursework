using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGetSetting : MonoBehaviour {

    private float volumeLevel;

    private void Start()
    {
        volumeLevel = this.GetComponent<AudioSource>().volume;
    }

    private void Update()
    {
        this.GetComponent<AudioSource>().volume = (volumeLevel * PlayerPrefs.GetFloat("masterVolume"));
    }

}
