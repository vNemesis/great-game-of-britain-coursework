using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private GameObject[] players;
    private GameObject start;
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at

    public Text playerDisplay;
    public Text turnDisplay;
    public Text travelsDisplay;

    private float countdown = 1.0f;
    private int currentplayer = 0;
    private int turnCounter = 1;
    private int numTravels = 3; 

    private bool opsCompleted;


    // Use this for initialization
    void Start()
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

        playerDisplay.text = ("Player " + (currentplayer + 1)  + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);
        
        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true);
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = true;
        players[currentplayer].GetComponent<PlayerController>().setTravels(numTravels);
    }

    // Update is called once per frame
    void Update()
    {
        // buffer to allow stations to spawn
        countdown -= 1.0f * Time.deltaTime;
        if (countdown <= 0.0f && !opsCompleted)
        {
<<<<<<< HEAD
            //call method to draw connections
            // Set first player as the player whose turn it is
            players[0].GetComponent<PlayerController>().controllerEnabled = true;
=======
>>>>>>> e7c1bba07c5fb9abf2bfcaf3f7802e7a9d6299d2
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

        playerDisplay.text = ("Player " + (currentplayer + 1) + "'s turn");
        turnDisplay.text = ("Turn: " + turnCounter);
        travelsDisplay.text = ("Travels Remaining: " + numTravels); 
    }

    /// <summary>
    /// moves all the players to the start position.
    /// </summary>
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


    public void moveCurrentPlayer()
    {
        players[currentplayer].GetComponent<PlayerController>().moveToSelectedStation();
        numTravels = players[currentplayer].GetComponent<PlayerController>().getTravels();     
    }

    /// <summary>
    /// updates which player is in control, enables the next player and disables current player when called.
    /// increments the turnCounter and sets the number of travels for that player.
    /// </summary>
    private void nextPlayerTurn()
    {
        
        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(false); //disables current player
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = false;
        currentplayer++;
        turnCounter++;
        if (currentplayer>=players.Length)
        {
            currentplayer = 0;
           
        }

        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true);
        players[currentplayer].GetComponent<PlayerController>().controllerEnabled = true;    //enables the next player in turn 
        numTravels = 3;                                                                     //Dice method should be placed here
        players[currentplayer].GetComponent<PlayerController>().setTravels(numTravels);
    }
}




<<<<<<< HEAD
    public void moveCurrentPlayer()
    {
        players[playerNumberTurn-1].GetComponent<PlayerController>().moveToSelectedStation();
    }
}
=======



>>>>>>> e7c1bba07c5fb9abf2bfcaf3f7802e7a9d6299d2
