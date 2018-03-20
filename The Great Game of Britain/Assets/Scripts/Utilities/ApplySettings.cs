using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplySettings : MonoBehaviour {

    private Button saveButton;

	// Use this for initialization
	void Start () {

        saveButton = this.GetComponent<Button>();

        saveButton.onClick.AddListener(delegate { SaveSettings(saveButton); });


    }

    private void SaveSettings(Button clickedButton)
    {
        GameObject.FindGameObjectWithTag("Settings Manager").GetComponent<SettingsManager>().saveSettings();
    }
}
