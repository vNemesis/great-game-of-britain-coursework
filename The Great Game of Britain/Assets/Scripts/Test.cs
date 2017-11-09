using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public float lightRange = 10.0f;

	// Use this for initialization
	void Start () {

        this.gameObject.GetComponent<Light>().range = lightRange;
		
	Debug.Log("testing git");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
