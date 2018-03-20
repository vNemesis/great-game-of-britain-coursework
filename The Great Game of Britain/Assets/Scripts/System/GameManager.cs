using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Class for the game, consolidates external functions and runs operations
/// 
/// Authors: Harman, Tariq, Mosope, Matt
/// </summary>
public class GameManager : MonoBehaviour
{

	#region Fields 
	private GameObject[] players;
	private GameObject start;
	private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at
	private Canvas mainCanvas;
	private CardDisplay cardDisplay;
	public GameObject playerPrefab;
    

	public Text playerDisplay;
	public Text turnDisplay;
	public Text travelsDisplay;	

	private float countdown = 1.0f;
	private int currentplayer = 0;
	private int turnCounter = 1;
	private int numTravels = 0;
	private bool rolled = false;
	private int numOfPlayers = 2;
	private int numCards = 3;
	private bool opsCompleted;
	private Tooltip tooltip;
    private bool isTempHideBlockStation=false;
    private ArrayList blockedStations = new ArrayList();

    //Loading Stuff
    public Canvas loadingImage;
    public Image loadingSymbol;
    public Text loadingText;

	// Managers
	private CardManager cardManager;
	public  TravelLightsDisplay travelLightsDisplay;
	public  DiceManager diceManager;
	#endregion

	void Start()
	{
		/* access game component references */

		// UI components
		mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();
		travelLightsDisplay = GameObject.Find("HUDLeftCorner").GetComponent<TravelLightsDisplay>();
        diceManager = GameObject.Find("Dice").GetComponent<DiceManager>();
		tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

        // number displays
        playerDisplay = GameObject.Find ("TextPlayerNo").GetComponent<Text> ();
		turnDisplay = GameObject.Find ("TextTurnNo").GetComponent<Text> ();
        //travelsDisplay = GameObject.Find ("TextTurnNo").GetComponent<Text> ();

		// card system
		cardDisplay = GameObject.FindGameObjectWithTag("Card Display").GetComponent<CardDisplay>();
		cardManager = new CardManager();

        /* generate players */

        // if running scene from main menu, find out many players were selected
        if (GameObject.Find("Game Setup Manager") != null)
		{
			numOfPlayers = GameSetUp.getNumOfPlayers();
		}

		// instantiate players and mutate appropiately
		for (int i = 1; i <= numOfPlayers; i++)
		{
			GameObject player = Instantiate(playerPrefab);

			PlayerController playerController = player.GetComponent<PlayerController>();

            #region blocker logic
            /*blocker logic set to false to bypass it and carry on with the game*/
            playerController.setBlockerMode(true);
            #endregion

            playerController.setPlayerNumber(i);
		}
		players = GameObject.FindGameObjectsWithTag("Player");

		// find and store reference to first start station
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Station"))
		{
			if (g.GetComponent<Stop>().checkIsStart())
			{
				start = g.gameObject;
				break;
			}
		}

		// update display
		updateDisplayNo();

		/* card management */
            //deal the cards to each player
            for (int i = 0; i < numOfPlayers; i++)
		{
			// retreive the cards from the cardmanager
			LocCard[] cards = cardManager.dealCards(numCards);
			// give the cards to the player
			players[i].GetComponent<PlayerController>().addCards(cards);

		}

		// display the cards
		LocCard[] displayCards = players[currentplayer].GetComponent<PlayerController>().getCards();
		cardDisplay.setCard(displayCards);
		cardDisplay.updateCard();

        /* moves all the players to the start position */
        foreach (GameObject p in players)
        {
            p.transform.parent = start.transform;

            // change player parent to new station and update position
            // shift position if multiple players are on a station
            if (start.transform.childCount > 1)
            {
                p.transform.localPosition = idlePos + (new Vector3(1.5f, 0.0f, 0.0f) * (start.transform.childCount - 2));
            }
            else
            {
                p.transform.localPosition = idlePos;
            }
        }
        players[currentplayer].GetComponent<PlayerController>().isControllerEnabled = true;



    }

	void Update()
	{

        players[currentplayer].GetComponent<PlayerController>().isControllerEnabled = true;
        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true);

        //players [currentplayer].GetComponent<PlayerController> ().setTravels(numTravels);

        /**/
        //check the players travel allowance and updates
        //travelsDisplay.text = ("Travels Remaining: " + players[currentplayer].GetComponent<PlayerController>().getTravels());


    }

    public GameObject getCurrentPlayer()
    {
        return players[currentplayer];
    }

    /// <summary>
    /// 
    /// </summary>
	public void moveCurrentPlayer()
	{
        PlayerController currentPlayerController = players[currentplayer].GetComponent<PlayerController>();

        currentPlayerController.commitMoves();
        currentPlayerController.moveToSelectedStations();
		changeNumTravels(currentPlayerController.getTravels()); 
		cardDisplay.updateCard();
        cardDisplay.completeCard(currentPlayerController.getCards());
		if (currentPlayerController.allCardsComplete())
		{
			endGame(currentPlayerController.getPlayerNumber());
		}
	}

	public void nextPlayerTurn()
	{
		/* current player */

		PlayerController currentPlayerController = players [currentplayer].GetComponent<PlayerController> ();

		currentPlayerController.setEnableAnimation(false);
		currentPlayerController.isControllerEnabled = false;
		currentPlayerController.setTravels(0);
		currentPlayerController.setisfirststation(true);
		currentPlayerController.clearSelectedStations();

		/* game */
        
        currentplayer = currentplayer == numOfPlayers - 1 ? 0 : currentplayer + 1;
        turnCounter++;
        rolled = false;

		updateDisplayNo ();

		diceManager.reset ();

		/* next player */

		currentPlayerController = players [currentplayer].GetComponent<PlayerController> ();

        // animatie player
		currentPlayerController.setEnableAnimation(true);
		currentPlayerController.isControllerEnabled = true;
        
        // get next players cards
        LocCard[] currentcards = currentPlayerController.getCards();
		cardDisplay.setCard(currentcards);
		cardDisplay.updateCard();
        
		changeNumTravels(0);
        resetBlockedStations();

    }

	public void rollDice()
	{
		if (!rolled)
		{
            // generate new dice roll
			var randomInt = Random.Range(1, 7);

            // trigger dice manager's animation functionality
			diceManager.rolled(randomInt);

			// update game and player values
			changeNumTravels(randomInt);

            PlayerController currentPlayerController = players[currentplayer].GetComponent<PlayerController>();

            currentPlayerController.setTravels(numTravels);
            currentPlayerController.setSelectedStationsSize(numTravels);
            currentPlayerController.hasRolledDice = true;
            if(numTravels == 6)
            {
                tempHideBlockedStations();
            }
            rolled = true;
        }
        else
        {
			tooltip.write("You have already rolled this turn");
        }
    }

    public void rollCheatDice()
    {
        if (!rolled)
        {
            // generate new dice roll
            var setInt = 6;

            // trigger dice manager's animation functionality
            diceManager.rolled(setInt);

            // update game and player values
            changeNumTravels(setInt);

            PlayerController currentPlayerController = players[currentplayer].GetComponent<PlayerController>();

            currentPlayerController.setTravels(numTravels);
            currentPlayerController.setSelectedStationsSize(numTravels);
            currentPlayerController.hasRolledDice = true;
            if (numTravels == 6)
            {
                tempHideBlockedStations();
            }
            rolled = true;
        }
    }

    public void tempHideBlockedStations()
    {
        foreach (GameObject station in blockedStations)
        {
            station.GetComponent<SmallStation>().setBlocker(false);
        }
    }

    public void resetBlockedStations()
    {
        foreach (GameObject station in blockedStations)
        {
            station.GetComponent<SmallStation>().setBlocker(true);
        }
    }

	/// <summary>
	/// End the game
	/// </summary>
	/// <param name="playerWhoWon"></param>
	public void endGame(int playerWhoWon)
	{
        StartCoroutine(ReturnToMenu(playerWhoWon, GameObject.Find("StatusText").GetComponent<Text>()));
	}

    /// <summary>
    /// Starts count-down
    /// </summary>
    /// <param name="winner"> winning player number </param>
    /// <param name="info"> text to display information </param>
    /// <returns></returns>
    IEnumerator ReturnToMenu(int winner, Text info)
    {
        float timer = 10.0f;

        while (timer >= 0.0f)
        {
            timer -= 1.0f * Time.deltaTime;

            info.text = ("Player " + winner + " has won!\nReturning to menu in: " + Mathf.Round(timer));
            yield return null;
        }

        StartCoroutine(LoadAsync(0));
    }

    /// <summary>
    /// Loads a scene Async
    /// </summary>
    /// <param name="sceneToLoad"> Scene number to load </param>
    /// <returns></returns>
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

    public void drawHazardCard()
	{
		//cardManager.GetComponent<Card>().drawCard();
	}

	/// <summary>
	/// The ok Button should set a player's 2 blockers, once it has all been set, the OK button should hide
	/// </summary>
	public void setBlocker()
	{
        #region wut
        if (players[currentplayer].GetComponent<PlayerController>().NumOBlockers == 0) { 
        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(false); //disables current player
		players[currentplayer].GetComponent<PlayerController>().isControllerEnabled = false;
		players[currentplayer].GetComponent<PlayerController>().setBlockerMode(false);
		Debug.Log("Player " + currentplayer + " has set their blokers");
		currentplayer++;


        if (currentplayer > (numOfPlayers - 1))
		{
			GameObject blockerTextBox = GameObject.Find("BlockerTextBox");
			currentplayer = 0;
            blockerTextBox.SetActive(false);

            GameObject.Find("ButtonEndTurn").GetComponent<Toggle>().interactable = true;
            GameObject.Find("ButtonMove").GetComponent<Button>().interactable = true;
            GameObject.Find("ButtonRollDice").GetComponent<Button>().interactable = true;

            GameObject[] stations = GameObject.FindGameObjectsWithTag("Station");

            foreach(GameObject station in stations)
            {
                if(station.GetComponent<SmallStation>().isBlocked)
                {
                    blockedStations.Add(station);

                }
            }
            Debug.Log("blocked stations count "+ blockedStations.Count);
        }
        updateDisplayNo();


        players[currentplayer].GetComponent<PlayerController>().setEnableAnimation(true); //enables the next player in turn 
		players[currentplayer].GetComponent<PlayerController>().isControllerEnabled = true;
		LocCard[] currentcards = players[currentplayer].GetComponent<PlayerController>().getCards();
		cardDisplay.setCard(currentcards);
		cardDisplay.updateCard();
        }
        else
        {
			tooltip.write("You need to place 2 blockers on the board");
        }
        #endregion
    }

    public void endTurn()
    {
        GameObject endTurnConfirm = GameObject.Find("EndTurnConfirmation");
        endTurnConfirm.SetActive(false);
    }

  

    #region Helper methods

    private void updateDisplayNo() {
		playerDisplay.text = (currentplayer + 1).ToString ();
		turnDisplay.text = turnCounter.ToString ();
	}

    public void changeNumTravels(int number)
    {
        numTravels = number;
        travelLightsDisplay.changeLightHUD(number);
    }

    #endregion
}
