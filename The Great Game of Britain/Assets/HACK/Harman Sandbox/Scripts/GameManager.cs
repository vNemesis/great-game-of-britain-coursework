using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameObject[] players;
    private GameObject start;
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at

    public Text playerDisplay;
    public Text turnDisplay;

    private float countdown = 1.0f;
    private int playerNumberTurn = 1;
    private int turnCounter = 1;

    private bool opsCompleted;

    // Use this for initialization
    void Start ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("Found " + players.Length + " Player(s)");

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Station"))
        {
            if (g.GetComponent<Stop>().checkIsStart())
            {
                start = g.gameObject;
            }
        }

        players[0].GetComponent<PlayerController>().controllerEnabled = true;

        playerDisplay.text = ("Player " + playerNumberTurn + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // buffer to allow stations to spawn
        countdown -= 1.0f * Time.deltaTime;

        if (countdown <= 0.0f && !opsCompleted)
        {
            //call method to draw connections
            // Set first player as the player whose turn it is
            players[0].GetComponent<PlayerController>().controllerEnabled = true;
            Debug.Log("Assigning Roles");

            playersToStart();

            // set true if all operations are complete
            opsCompleted = true;
        }
        else if (countdown <= 0.0f && opsCompleted)
        {
            // end countdown
            countdown = 0.0f;
        }
    }

    private void playersToStart()
    {
        // find all stations
        foreach (GameObject p in players)
        {
            if (start.transform.childCount > 1)
            {
                // change player parent to new station and update position
                p.transform.parent = start.transform;
                p.transform.localPosition = idlePos + (new Vector3(1.5f, 0.0f, 0.0f) * (start.transform.childCount - 2));
            }
            else
            {
                // change player parent to new station and update position
                p.transform.parent = start.transform;
                p.transform.localPosition = idlePos;
            }
        }
    }

    public void nextPlayerTurn()
    {
        if (playerNumberTurn == players.Length)
        {
            Debug.Log("Player number before " + playerNumberTurn);

            players[playerNumberTurn-1].GetComponent<PlayerController>().controllerEnabled = false;
            playerNumberTurn = 1;
            players[playerNumberTurn].GetComponent<PlayerController>().controllerEnabled = true;
            turnCounter++;

            Debug.Log("Player number after " + playerNumberTurn);
            Debug.Log(players.Length);
        }
        else if (playerNumberTurn == players.Length - 1)
        {
            Debug.Log("Player number before " + playerNumberTurn);

            players[playerNumberTurn].GetComponent<PlayerController>().controllerEnabled = false;
            playerNumberTurn++;
            players[playerNumberTurn-1].GetComponent<PlayerController>().controllerEnabled = true;

            Debug.Log("Player number after " + playerNumberTurn);
            Debug.Log(players.Length);
        }
        else
        {
            Debug.Log("Player number before " + playerNumberTurn);

            players[playerNumberTurn].GetComponent<PlayerController>().controllerEnabled = false;
            playerNumberTurn++;
            players[playerNumberTurn].GetComponent<PlayerController>().controllerEnabled = true;

            Debug.Log("Player number after " + playerNumberTurn);
            Debug.Log(players.Length);
        }

        playerDisplay.text = ("Player " + playerNumberTurn + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);
    }

    public void moveCurrentPlayer()
    {
        players[playerNumberTurn-1].GetComponent<PlayerController>().moveToSelectedStation();
    }
}
