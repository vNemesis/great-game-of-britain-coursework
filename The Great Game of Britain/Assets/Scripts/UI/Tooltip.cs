using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tooltip behavour for multiple systems to communicate with players
/// 
/// Authors: Matt
/// </summary>
public class Tooltip : MonoBehaviour {
	/* access component */
	private Text textUI;

	/* text settings */
	private float r = 1f;
	private float g = 1f;
	private float b = 1f;

	/* animation settings */
	[SerializeField] private float fadeIn = 0.5f;
	[SerializeField] private float opaque = 2f;
	[SerializeField] private float fadeOut = 0.5f;
	private float fadeInOpaque;
	private float total;
	private bool anim = false;
	private float tick = 0f;

	void Start () {
		textUI = this.GetComponent<Text> ();

		fadeInOpaque = fadeIn + opaque;
		total = fadeInOpaque + fadeOut;
	}

	void Update () {
		if (anim) {
			tick += Time.deltaTime;
			if (tick < fadeIn) {
				changeAlpha (tick / fadeIn);
			} else if (tick > fadeIn && tick <= fadeInOpaque) {
				changeAlpha (1f);
			} else if (tick > fadeInOpaque && tick <= total) {
				changeAlpha (1 - ((tick - fadeInOpaque) / fadeOut));
			} else {
				anim = false;
				reset ();
			}
		}
	}

	public void write(string msg) {
		reset ();
		textUI.text = msg;
		anim = true;
	}

	private void changeAlpha(float a) {
		textUI.color = new Color (r, g, b, a);
	}

	private void reset() {
		tick = 0f;
		changeAlpha(0f);
	}
}
