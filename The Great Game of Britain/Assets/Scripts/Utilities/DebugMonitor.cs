using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugMonitor : MonoBehaviour {

    [SerializeField] private GameObject debugCanvas;
    [SerializeField] private Text ConsoleText;

    private StringBuilder sb = new StringBuilder();

    public InputField commandInput;

    private void Start()
    {
        commandInput.onEndEdit.AddListener(delegate { processCommand(commandInput.text); } );
    }

    // Update is called once per frame
    void Update () {

#if UNITY_EDITOR

        if (Input.GetButtonDown("Debug"))
        {
            debugCanvas.GetComponent<Canvas>().enabled = !debugCanvas.GetComponent<Canvas>().enabled;
        }

#endif

        if (!debugCanvas.GetComponent<Canvas>().enabled)
        {
            return;
        }

        ConsoleText.text = sb.ToString();
	}

    private void PrintGameStats()
    {
        sb.AppendLine("Quality Level: " + QualitySettings.GetQualityLevel());
        sb.AppendLine("Master Volume: " + (PlayerPrefs.GetFloat("masterVolume") * 100));

        string temp = "";

        if (PlayerPrefs.GetInt("mouseScroll") == 1)
        {
            temp = "True";
        }
        else
        {
            temp = "False";
        }
        sb.AppendLine("Mouse Scroll Enabled: " + temp);
    }

    private void processCommand(string command)
    {
        commandInput.text = "";

        if (command.Equals("help"))
        {
            sb.AppendLine("Current commands are:");
            sb.AppendLine("help - this menu");
            sb.AppendLine("stats - prints game settings");
            sb.AppendLine("end game - ends game with player 1 winning");
            sb.AppendLine("roll 6 - rolls a 6 on the dice");
            return;
        }

        if (command.Equals("stats"))
        {
            PrintGameStats();
            return;
        }

        if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
        {
            if (command.Equals("end game"))
            {
                sb.AppendLine("Cannot use this command in this scene");
                return;
            }
            else if (command.Equals("roll 6"))
            {
                sb.AppendLine("Cannot use this command in this scene");
                return;
            }
        }
        else
        {
            if (command.Equals("end game"))
            {
                sb.AppendLine("Ending Game");
                GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().endGame(1);
                return;
            }
            else if (command.Equals("roll 6"))
            {
                sb.AppendLine("Rolling a 6");
                GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().rollCheatDice();
                return;
            }
        }

        sb.AppendLine("Command not found");
        return;
    }
}
