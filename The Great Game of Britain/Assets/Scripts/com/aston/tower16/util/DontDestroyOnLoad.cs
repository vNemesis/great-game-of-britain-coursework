using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    public bool isAudioScript;

	// Use this for initialization
	void Awake ()
    {
        if (isAudioScript)
        {

        }

        DontDestroyOnLoad(transform.gameObject);
        Debug.Log("Will not destroy");
    }
}
