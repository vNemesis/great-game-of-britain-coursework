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

	private GameObject selected;
	// Currently selected object
	private Canvas mainCanvas;
	// Main canvas in scene for UI
	private Vector3 idlePos = new Vector3 (0.0f, 0.5f, 0.0f);
	// On a stop what local position will the player be located at
	public bool controllerEnabled { get; set; }

	#endregion Contains fields for this class
	public bool rolledDice;
	private float speed;
	private bool moveable;
	private Vector3 targetPosition;
	private int travels;
	private GameObject startingStation;
	private GameObject currentStation;
	private bool isfirststation = true;
	public Text movePermisson;
	private Stack<GameObject> selectedStations;
	public Stack<GameObject> commitedMoves;
	public int count=0;

    private AudioSource selectSound;            // Source used for selection
    private AudioSource moveSound;              // Source used for movement

	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponentInChildren<Animator> ().enabled = false;
		controllerEnabled = false;
		speed = 50;
		moveable = false;
		rolledDice = false;
		// Fetch main canvas from scene
		mainCanvas = GameObject.FindGameObjectWithTag ("Main Canvas").GetComponent<Canvas> ();

        movePermisson = GameObject.Find("Text_MovePermission").GetComponent<Text>();

        selectSound = GameObject.Find("UIAudio").GetComponent<AudioSource>();

        moveSound = GameObject.Find("UIAudioMovement").GetComponent<AudioSource>();


	}

	// Update is called once per frame
	void Update ()
	{
	



		if (controllerEnabled) {
			//The player can only move to stations that are the same colour, therefore we need to store the station it started at.
			//We also want to keep reference of the current station the player is at
			if (isfirststation) {
				startingStation = transform.GetComponentInParent<SmallStation> ().gameObject;
				currentStation = transform.GetComponentInParent<SmallStation> ().gameObject;
				Debug.Log ("current station is: "+currentStation.name);
				isfirststation = false;
			}



			// If left-mouse is pressed and currently not hovering over a UI element
			if (Input.GetMouseButtonDown (0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false) {

				// Send a raycast
				RaycastHit hitInfo = new RaycastHit ();
				bool hit = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo);

				// if it hit something
				if (hit) {

                    Debug.Log("HIT!");

					// if it hit a station and the player has rolled a dice
					if (hitInfo.transform.parent.gameObject.tag == "Station" && rolledDice) {
						// set selected to equal that station
						selected = hitInfo.transform.parent.gameObject;
						//get the connected stations of the selected station
						Stop[] connectedStations = selected.transform.GetComponentInParent<SmallStation> ().getConnectedStations ();
						//if highlighted, unhighlight
						if (selected.GetComponent<SmallStation> ().getIsHighlighted () == true) {
							removeFronSelectedStations (selected);
							setcurrentStation ();
						} 
						//the station is accessible under these conditions:
						//if the selected station is adjacent to the current highlighted station & the same colour as the starting station
						//if the selected station is adjacent to the current highlighted station & is black
						else if ((currentStation.GetComponent<SmallStation> ().isContains (selected) && startingStation.GetComponent<SmallStation> ().isSameColour (selected)) 
							|| (currentStation.GetComponent<SmallStation> ().isContains (selected) && startingStation.GetComponent<SmallStation> ().currentColour == Stop.colourName.Black))
						{
							movePermisson.text = ("Accessible: Yes");

							//Highlihts the selected station
							if (selected.GetComponent<SmallStation> ().getIsHighlighted () == false && travels!=0) {
								addToSelectedStations (selected);
								currentStation = selected;
                                selectSound.Play();
								Debug.Log ("current station is: "+currentStation.name);
							} else {
								removeFronSelectedStations(selected);
								setcurrentStation ();
							}

						} else {
							movePermisson.text = ("Accessible: No");
						}

						//	Debug.Log ("Selected Station" + selected.GetComponent<Stop> ().stopName);
						//	Debug.Log ("num player: " + selected.transform.childCount);

						//update UI text on main canvas to display selected stops name
								mainCanvas.transform.Find ("Text_Selection").GetComponent<Text> ().text = ("Selected: " + selected.GetComponent<Stop> ().stopName);
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

	/// <summary>
	/// Moves player to committed stations, checks station children before entering.
	/// </summary>
	public void moveToSelectedStations ()
	{
		// if selected is not empty

			GameObject station = commitedMoves.Pop ();
			if (station != null && controllerEnabled) {

				if (station.transform.childCount > 1) {
					// change player parent to new station and update position
					gameObject.transform.parent = station.transform;

					// will adjust depending on how many children are attached to the station
					float position = 0.0f;
					for (int i = 0; i < 10; i++) {
						if (!Physics.CheckSphere (station.transform.position + new Vector3 (position, 0.5f, 0.0f), 0.5f)) { //checks if the area is empty
							targetPosition = station.transform.position + new Vector3 (position, 0.5f, 0.0f);
							//decrementTravels ();
							moveable=true;
							movePlayer();
							break;
						}
						position = position + 5.0f;
					}
				} else {
					// change player parent to new station and update position
					gameObject.transform.parent = station.transform;
					targetPosition = station.transform.position + idlePos;
					//decrementTravels ();
					moveable=true;
					movePlayer();

			}
		}
	}


	/// <summary>
	/// Moves the player by setting movable to true and applying targetPosiiton
	/// </summary>
	public void movePlayer ()
	{
		if (moveable) {
			gameObject.transform.position = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);//moves towards position of station

			if (targetPosition == transform.position) {//once player has reached station moveable is turned false to stop movement
				moveable = false;

				if (commitedMoves.Count > 0) {
					GameObject station = commitedMoves.Pop ();
					gameObject.transform.parent = station.transform;
					targetPosition = station.transform.position + idlePos;

					moveable=true;
					
				}
				//no more moves is you have moved
				else if(commitedMoves.Count ==0)
				{
					clearSelectedStations ();
				
				}
	
			}
		}

	}



	public void setisfirststation (bool b)
	{
		isfirststation = b;
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
		this.travels = travels;
	}

	/// <summary>
	/// returns travels in int
	/// </summary>
	/// <returns></returns>
	public int getTravels ()
	{
		return travels;
	}

	/// <summary>
	/// decreases the number of travels by 1. ensures that travels does not go below 0.
	/// </summary>
	public void decrementTravels ()
	{
		if (travels != 0) {
			travels--;
		}
	}

	public void incrementTravels ()
	{
		travels++;
	}


	public void setSelectedStationsSize (int numOfTravels)
	{
		selectedStations = new Stack<GameObject> (numOfTravels);
	}

	public void addToSelectedStations(GameObject ss)
	{
		if (selectedStations.Count <= travels) {
			ss.GetComponent<SmallStation> ().setIshiglighted (true);
			selectedStations.Push (ss);
			decrementTravels ();

			Debug.Log ("Added " + ss.GetComponent<SmallStation> ().name + " to stack. The current stack size is: " + selectedStations.Count);
		} else {
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
			Debug.Log ("Removed "+ p.GetComponent<SmallStation>().name +" from stack. The current stack size is: " + selectedStations.Count);	
			if(p.name == ss.name)
			{
				found = true;
			}
			incrementTravels ();
		}	
	}

	public void clearSelectedStations()
	{
		while (selectedStations.Count !=0)
		{
			GameObject p = selectedStations.Pop ();
			p.GetComponent<SmallStation> ().setIshiglighted (false);
			Debug.Log ("Removed "+ p.GetComponent<SmallStation>().name +" from stack. The current stack size is: " + selectedStations.Count);	
		}	
	}

	public void setcurrentStation()
	{
		if (selectedStations.Count == 0) {
			currentStation = startingStation;
		} else {
			currentStation = selectedStations.Peek ();
		}
		Debug.Log ("current station is: "+currentStation.name);

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
		return moveable;
	}


}