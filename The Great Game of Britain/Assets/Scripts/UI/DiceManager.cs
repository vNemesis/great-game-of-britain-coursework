using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines methods for fancy dice animation
/// 
/// Authors: Matt
/// </summary>
public class DiceManager : MonoBehaviour {
	/* component reference */
	private Image image;
    private TravelLightsDisplay hud;

	/* path variables */
	private string path = "dice_faces_hd";
	private string prefix = "";
	private string suffix = "_dot";

    /* animation variables */
    [SerializeField] private int dCons = 15;
    [SerializeField] private int dExp = 5;
    private int dTotal;
    [SerializeField] private float tBase = 0.05f;
	private float tNext;
	private int count = 1;
    [SerializeField] private float mod = 5f;
	private float tick;
	private bool anim = false;
	private int diceNum = 0;
	private int currDiceNum;
    
	void Start () {
		image = this.GetComponent<Image> ();
        hud = GameObject.Find("HUDLeftCorner").GetComponent<TravelLightsDisplay>();

        dTotal = dCons + dExp;
		tick = 0f;
		tNext = tBase;
		currDiceNum = diceNum;
	}
	
	void Update () {
		if (anim) {
            tick += Time.deltaTime;
            if (tick > tNext)
            {
                if (!(count > dTotal))
                {
                    if (count > dCons)
                    {
                        tNext = tBase * Mathf.Pow(mod, Mathf.Log(count - dCons));
                        hud.changeLightHUD(0);
                    }

                    int i = genNum();
                    if (count == dTotal)
                    {
                        while (i == currDiceNum || i == diceNum)
                        {
                            i = genNum();
                        }
                        changeFace(i);
                    }
                    else
                    {
                        while (i == currDiceNum)
                        {
                            i = genNum();
                        }
                        changeFace(i);
                    }
                
                    hud.changeLightHUD(count > dCons + (dExp / 2) ? 0 : i);

                    resetTick();
                    count++;

                }
                else
                {
                    changeFace(diceNum);
                    hud.changeLightHUD(diceNum);
                    tNext = tBase;
                    resetTick();
                    count = 0;
                    anim = false;
                }
            }
		}
	}

	string getPath(int diceNum) {
		return (path != "" ? path + "/" : "") + prefix + diceNum.ToString () + suffix;
	}

	public void rolled(int diceNum) {
		this.diceNum = diceNum;
		anim = true;
	}

	void resetTick() {
		tick = 0f;
	}

	int genNum() {
		return Random.Range(1, 7);
	}

	void changeFace(int diceNum) {
		image.overrideSprite = Resources.Load<Sprite> (getPath (diceNum));
        currDiceNum = diceNum;
	}

	public void reset() {
		image.overrideSprite = null;
	}
}
