using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TravelLightsDisplay : MonoBehaviour {

    public Sprite[] hudSprites;
    // public SpriteRenderer cornerHUD;
    private Image cornerHUD;

	// Use this for initialization
	void Start ()
    {
        cornerHUD = this.GetComponent<Image>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeLightHUD(int number)
    {
        cornerHUD.sprite = hudSprites[number];
    }
}
