using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

    public void changeToScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
