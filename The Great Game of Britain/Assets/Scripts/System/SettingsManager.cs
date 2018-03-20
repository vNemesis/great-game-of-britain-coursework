using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {


    //Audio Settings
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Text volumeSliderText;

    //Quality Setting
    [SerializeField] private Slider qualitySlider;
    [SerializeField] private Text qualitySliderText;

    // Mouse Scroll Toggle
    [SerializeField] private Toggle mouseScrollToggle;


    // Use this for initialization
    void Start ()
    {
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1);
        }

        if (!PlayerPrefs.HasKey("qualityLevel"))
        {
            PlayerPrefs.SetFloat("qualityLevel", 3);
        }

        if (!PlayerPrefs.HasKey("mouseScroll"))
        {
            PlayerPrefs.SetInt("mouseScroll", 0);
        }

        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume") * 100;

        qualitySlider.value = PlayerPrefs.GetFloat("qualityLevel");

        mouseScrollToggle.isOn = isSettingTrue("mouseScroll");
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (volumeSlider == null || volumeSliderText == null)
        {
            volumeSlider =  GameObject.Find("Volume_Slider").GetComponent<Slider>();
            volumeSliderText = GameObject.Find("Volume_Text").GetComponent<Text>();
            volumeSlider.value = PlayerPrefs.GetFloat("masterVolume") * 100;

        }

        if (qualitySlider == null || qualitySliderText == null)
        {
            qualitySlider = GameObject.Find("Quality_Slider").GetComponent<Slider>();
            qualitySliderText = GameObject.Find("Quality_Text").GetComponent<Text>();
            qualitySlider.value = PlayerPrefs.GetFloat("qualityLevel");

        }

        if (mouseScrollToggle == null)
        {
            mouseScrollToggle = GameObject.Find("MouseScroll_Toggle").GetComponent<Toggle>();

            mouseScrollToggle.isOn = isSettingTrue("mouseScroll");

        }

        volumeSliderText.text = "" +  volumeSlider.value;
        qualitySliderText.text = "" + qualitySlider.value;

    }


    public void saveSettings()
    {
        PlayerPrefs.SetFloat("masterVolume", (volumeSlider.value / 100));

        if (mouseScrollToggle.isOn)
        {
            PlayerPrefs.SetInt("mouseScroll", 1);
            Debug.LogWarning("Scroll Enabled");
        }
        else
        {
            PlayerPrefs.SetInt("mouseScroll", 0);
            Debug.LogWarning("Scroll Disabled");
        }


        PlayerPrefs.SetFloat("qualityLevel", qualitySlider.value);
        QualitySettings.SetQualityLevel(int.Parse(qualitySlider.value.ToString()));

        Debug.LogWarning("Saving Settings");
    }

    public static bool isSettingTrue(string setting)
    {
        if (PlayerPrefs.GetInt(setting) == 1)
        {
            return true;
        }

        return false;
    }


}
