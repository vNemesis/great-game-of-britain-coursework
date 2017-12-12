using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameSetUp : MonoBehaviour {

    [SerializeField] private int numOfPlayers;
    [SerializeField] private Slider numOfPlayersInput;
    [SerializeField] private Text SliderText;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(transform.gameObject);
    //}

    void Update()
    {
        numOfPlayers = Mathf.RoundToInt(numOfPlayersInput.value);
        SliderText.text = ("Number of Players: " + numOfPlayers);
    }

    public void startGame(int sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public int getNumOfPlayers()
    {
        return numOfPlayers;
    }

}
