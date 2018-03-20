using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameSetUp : MonoBehaviour {

    [SerializeField] private static int numOfPlayers;
    [SerializeField] private Slider numOfPlayersInput;
    [SerializeField] private Text SliderText;
    private Button startGameButton;
    public Canvas loadingImage;
    public Image loadingSymbol;
    public Text loadingText;


    void Update()
    {

        if (startGameButton == null)
        {
            startGameButton = GameObject.Find("Start_Button").GetComponent<Button>();
            startGameButton.onClick.AddListener(delegate { startGame(1); });
        }

        if (numOfPlayersInput == null || SliderText == null)
        {
            numOfPlayersInput = GameObject.Find("PlayerNum_Slider").GetComponent<Slider>();
            SliderText = GameObject.Find("Player_Num_Text").GetComponent<Text>();

        }

        numOfPlayers = Mathf.RoundToInt(numOfPlayersInput.value);
        SliderText.text = ("Number of Players: " + numOfPlayers);

    }

    private void startGame(int levelToLoad)
    {
        StartCoroutine(LoadAsync(levelToLoad));
    }

    IEnumerator LoadAsync(int sceneToLoad)
    {
        loadingImage.enabled = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            loadingText.text = Mathf.Round(progress * 100f) + "%";

            loadingSymbol.fillAmount = progress;

            yield return null;
        }
    }

    public static int getNumOfPlayers()
    {
        return numOfPlayers;
    }

}
