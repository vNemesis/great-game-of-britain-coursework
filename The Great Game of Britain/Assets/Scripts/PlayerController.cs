using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class PlayerController : MonoBehaviour
{

	#region Fields

	private GameObject selectedStation;
	// Currently selected object
	private Canvas mainCanvas;
	// Main canvas in scene for UI
	private Vector3 idlePos = new Vector3 (0.0f, 0.5f, 0.0f);
	// On a stop what local position will the player be located at
	public bool isControllerEnabled { get; set; }

    private bool blockerMode;
    public int NumOBlockers = 2;
    public bool hasRolledDice;
    private float travelSpeed;
    private bool isMoveable;
    private Vector3 targetPosition;
    private int travelsRemaining;
    private GameObject startingStation;
    private GameObject currentSelectedStation;
    private bool isStartingstation = true;
    public Text movePermisson;
    private Stack<GameObject> selectedStations = new Stack<GameObject>();
    private int playerNumber;
    private int capacity;
    public Stack<GameObject> commitedMoves = new Stack<GameObject>();
    public int count = 0;
    private AudioSource selectSound;            // Source used for selection
    private AudioSource moveSound;              // Source used for movement
    private LocCard[] playerCards;
    private Text helpSystem;

    // Visual
    public GameObject steam;

    #endregion Contains fields for this class

    ///<summary> Initialises Player Controller </summary>
    void Start ()
	{
		gameObject.GetComponentInChildren<Animator> ().enabled = false;
		isControllerEnabled = false;
		travelSpeed = 50;
		isMoveable = false;
		hasRolledDice = false;
		// Fetch main canvas from scene
		mainCanvas = GameObject.FindGameObjectWithTag ("Main Canvas").GetComponent<Canvas> ();

        // Set UI Components
        movePermisson = GameObject.Find("TextMovePermission").GetComponent<Text>();
        // selectSound = GameObject.Find("UIAudio").GetComponent<AudioSource>();
        // moveSound = GameObject.Find("UIAudioMovement").GetComponent<AudioSource>();
        helpSystem = GameObject.Find("HelpMsgSystem").GetComponent<Text>();
    }

	///<summary> Update is called once per frame to refresh the scene</summary>
	void Update ()
	{
	
       
		if (isControllerEnabled) {
            Debug.Log("Blocker mode is"+ blockerMode);

            
            if (blockerMode)
            {
                if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
                {
                    // Send a raycast
                    RaycastHit hitInfo = new RaycastHit();
                    bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

                    if (hit)
                    {
                        if (hitInfo.transform.parent.gameObject.tag == "Station")
                        {
                            selectedStation = hitInfo.transform.parent.gameObject;

                            if (!selectedStation.GetComponent<SmallStation>().isBlocked)
                            {
                                if (NumOBlockers > 0
)
                                {
                                    selectedStation.GetComponent<SmallStation>().setBlocker(true);
                                    NumOBlockers--;
                                }
                            }
                            else
                            {
                                selectedStation.GetComponent<SmallStation>().setBlocker(false);
                                NumOBlockers++;
                            }
                        }
                    }
                }
            }
            else
            { 
                //Setup refrence to the station it started at, also record what station the player is currently at
                ConfigStartingStation();
            //If there are no selected stations, reset the current station to the starting station
            ResetCurrentStationToStart();

			// If left-mouse is pressed and currently not hovering over a UI element
			if (Input.GetMouseButtonDown (0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false) {

				// Send a raycast
				RaycastHit hitInfo = new RaycastHit ();
				bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);

				// if it hit something
				if (hit) {

                    Debug.Log("HIT!");

					// if it hit a station and the player has rolled a dice
					if (hitInfo.transform.parent.gameObject.tag == "Station" && hasRolledDice ) {
						
                        // set selected to equal that station
						selectedStation = hitInfo.transform.parent.gameObject;
						//get the connected stations of the selected station
						Stop[] connectedStations = selectedStation.transform.GetComponentInParent<SmallStation> ().getConnectedStations ();

                        //if highlighted, unhighlight
                        if (selectedStation.GetComponent<SmallStation> ().getIsHighlighted () == true) {
							removeFronSelectedStations (selectedStation);
							setcurrentSelectedStation ();
						} 
						//the station is accessible under these conditions:
						//if the selected station is adjacent to the current highlighted station & the same colour as the starting station
						//if the selected station is adjacent to the current highlighted station & is black
						else if ((!selectedStation.GetComponent<SmallStation>().isBlocked && currentSelectedStation.GetComponent<SmallStation>().isContains (selectedStation) && startingStation.GetComponent<SmallStation> ().isSameColour (selectedStation)) 
							|| (!selectedStation.GetComponent<SmallStation>().isBlocked && currentSelectedStation.GetComponent<SmallStation> ().isContains (selectedStation) && startingStation.GetComponent<SmallStation> ().currentColour == Stop.colourName.Black)
                            || ((!selectedStation.GetComponent<SmallStation>().isBlocked && selectedStation.GetComponent<SmallStation>().isContains(currentSelectedStation) && startingStation.GetComponent<SmallStation>().isSameColour(selectedStation))
                            || (!selectedStation.GetComponent<SmallStation>().isBlocked && selectedStation.GetComponent<SmallStation>().isContains(currentSelectedStation) && startingStation.GetComponent<SmallStation>().currentColour == Stop.colourName.Black)))

						{
							movePermisson.text = ("Accessible: Yes");

							//Highlihts the selected station
							if (selectedStation.GetComponent<SmallStation> ().getIsHighlighted () == false && travelsRemaining > 0) {
								addToSelectedStations (selectedStation);
								currentSelectedStation = selectedStation;
                                //selectSound.Play();
								Debug.Log ("current station is: "+currentSelectedStation.name);
							} else {
								removeFronSelectedStations(selectedStation);
								setcurrentSelectedStation ();
							}

						} else {
							movePermisson.text = ("Accessible: No");
                                helpSystem.text = ("This station is not accessible");
                        }

						//update UI text on main canvas to display selected stops name
								mainCanvas.transform.Find ("Text_Selection").GetComponent<Text> ().text = ("Selected: " + selectedStation.GetComponent<Stop> ().stopName);
                            helpSystem.text = ("You have Selected "  + selectedStation.GetComponent<Stop>().stopName);
                        }
					// else if it's not a station
					else {
						Debug.Log ("Didn't select a valid object");

					}
				}
				// else if it didn't hit any coliider
				else {
					Debug.Log ("No hit");
				}
			}
		}

		movePlayer ();
        }
    }

   
    public void ConfigStartingStation()
    {
        if (isStartingstation)
        {
            startingStation = transform.GetComponentInParent<SmallStation>().gameObject;
            currentSelectedStation = transform.GetComponentInParent<SmallStation>().gameObject;
            isStartingstation = false;
        }
    }

    private void ResetCurrentStationToStart()
    {
        if (selectedStations != null && selectedStations.Count == 0)
        {
            currentSelectedStation = startingStation;
        }
    }

    /// <summary>
    /// Moves player to committed stations, checks station children before entering.
    /// </summary>
    public void moveToSelectedStations()
    {
        // if selected is not empty
        Debug.Log(commitedMoves.Count);
        GameObject station = commitedMoves.Pop();
        if (station != null && isControllerEnabled)
        {
            if (station.transform.childCount > 1)
            {
                // change player parent to new station and update position
                gameObject.transform.parent = station.transform;

                // will adjust depending on how many children are attached to the station
                float position = 0.0f;
                for (int i = 0; i < 10; i++)
                {
                    if (!Physics.CheckSphere(station.transform.position + new Vector3(position, 0.5f, 0.0f), 0.5f))
                    { //checks if the area is empty
                        targetPosition = station.transform.position + new Vector3(position, 0.5f, 0.0f);
                        //decrementTravels ();
                        isMoveable = true;
                        movePlayer();
                        break;
                    }
                    position = position + 5.0f;
                }
            }
            else
            {
                // change player parent to new station and update position
                gameObject.transform.parent = station.transform;
                targetPosition = station.transform.position + idlePos;
                //decrementTravels ();
                isMoveable = true;
                movePlayer();
            }

            for (int i = 0; i < playerCards.Length; i++)
            {
                string stopName = currentSelectedStation.GetComponent<Stop>().stopName;
                Debug.Log(stopName);
                string cardName = playerCards[i].getName();
                Debug.Log(cardName);
                if (cardName.Equals(stopName))
                {
                    playerCards[i].completed();
                }
                
            }
        }
    }



	/// <summary>
	/// Moves the player by setting movable to true and applying targetPosiiton
	/// </summary>
	public void movePlayer ()
	{
		if (isMoveable) {
			gameObject.transform.position = Vector3.MoveTowards (transform.position, targetPosition, travelSpeed * Time.deltaTime);//moves towards position of station

            // Rotates object in direction it is moving
            Vector3 newDir = Vector3.RotateTowards(transform.localPosition, targetPosition, 5.0f * Time.deltaTime, 0.0F);
            transform.rotation = Quaternion.LookRotation(-newDir);

            if (targetPosition == transform.position) {//once player has reached station moveable is turned false to stop movement
				isMoveable = false;

				if (commitedMoves.Count > 0) {
					GameObject station = commitedMoves.Pop ();
					gameObject.transform.parent = station.transform;
					targetPosition = station.transform.position + idlePos;

                    isMoveable =true;
					
				}
				//no more moves is you have moved
				else if(commitedMoves.Count ==0)
				{
					clearSelectedStations ();
                    transform.rotation = new Quaternion(0,0,0,1);

                    this.GetComponent<ParticleSystem>().Play();
                    GameObject.Find("Steam_SFX").GetComponent<AudioSource>().Play();

                }
	
			}
		}

	}



	public void setisfirststation (bool b)
	{
		isStartingstation = b;
	}

	/// <summary>
	/// enables/disables the animation of the player
	/// </summary>
	/// <param name="value"></param>
	public void setEnableAnimation (bool value)
	{
		gameObject.GetComponentInChildren<Animator> ().enabled = value;
	}

	/// <summary>
	/// sets the travels for the player
	/// </summary>
	/// <param name="travels"></param>
	public void setTravels (int travels)
	{
		this.travelsRemaining = travels;
	}

	/// <summary>
	/// returns travels in int
	/// </summary>
	/// <returns></returns>
	public int getTravels ()
	{
		return travelsRemaining;
	}

	/// <summary>
	/// decreases the number of travels by 1. ensures that travels does not go below 0.
	/// </summary>
	public void decrementTravels ()
	{
		if (travelsRemaining != 0) {
			travelsRemaining--;
		}
	}

	public void incrementTravels ()
	{
		travelsRemaining++;
	}


	public void setSelectedStationsSize (int numOfTravels)
	{
		selectedStations = new Stack<GameObject> (numOfTravels);
        capacity = numOfTravels;
	}

	public void addToSelectedStations(GameObject selectedStation)
	{
		if (selectedStations.Count != capacity) {
			selectedStation.GetComponent<SmallStation> ().setIshiglighted (true);
			selectedStations.Push (selectedStation);
			decrementTravels ();

		}else {
			Debug.Log ("Maximum number of places selected");
		}
	}

	public void removeFronSelectedStations(GameObject ss)
	{

		bool found = false;

		while (found == false)
		{
			GameObject p = selectedStations.Pop ();
			p.GetComponent<SmallStation> ().setIshiglighted (false);
			if(p.name == ss.name)
			{
				found = true;
			}
			incrementTravels ();
            if(selectedStations.Count == 0)
            {
                break;
            }
		}	
	}

	public void clearSelectedStations()
	{
		while (selectedStations != null && selectedStations.Count !=0)
		{
			GameObject p = selectedStations.Pop ();
			p.GetComponent<SmallStation> ().setIshiglighted (false);
		}
    }

	public void setcurrentSelectedStation()
	{
        //if there are no selected stations, set the current station to the the station the player started
		if (selectedStations != null && selectedStations.Count == 0) {
			currentSelectedStation = startingStation;
		} else {
            //else set the current station to the one on top of the stack
			currentSelectedStation = selectedStations.Peek ();
		}
	}

	public void commitMoves()
	{
		GameObject[] t = selectedStations.ToArray();
		//Array.Reverse (t);
		commitedMoves = new Stack<GameObject>();

		foreach(GameObject s in t)
		{
			commitedMoves.Push(s);
		}
	}

	public bool isMovable()
	{
		return isMoveable;
	}

    public void addCards(LocCard[] dealtCards)//this may be changed in the future
    {
        playerCards = dealtCards;
    }

    public LocCard[] getCards()
    {
        return playerCards;
    }

    public bool allCardsComplete()
    {
        bool isComplete = false;
        int cardCount = 0;
        for (int i = 0; i < playerCards.Length; i++ )
        {
            if (playerCards[i].getComplete())
            {
                cardCount++;
            }
        }
        if (cardCount == 3)
        {
            isComplete = true;
        }
        return isComplete;
    }

    public Stack<GameObject> GetSelectedStations()
    {
        return selectedStations;
    }

    public Stack<GameObject> GetCommitedStations()
    {
        return commitedMoves;
    }

    public void SetStartingStaion(GameObject station)
    {
        startingStation = station;
    }

    public GameObject GetCurrentSelectedStation()
    {
        return currentSelectedStation;
    }

    public void setPlayerNumber(int number)
    {
        playerNumber = number;
    }

    public int getPlayerNumber()
    {
        return playerNumber;
    }

    public void setBlockerMode(bool b)
    {
        blockerMode = b;
    }

    public bool isBlockerMode()
    {
        return blockerMode;
    }
}