using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stop : MonoBehaviour {

	#region Fields
	// Pick colour for stop
	public enum colourName { Red, Blue, Yellow, Black }
	public colourName currentColour;


	// Serializing private fields allows them to be edited in the Inspector whilst retaining there visibility

	[SerializeField] private Color stopColour;                                  // Colour of Stop        
	[SerializeField] private Stop[] connectingStops;                            // Stops this stop connects to
	[SerializeField] private GameObject[] playersOnStop;                        // Players on this stop
	public bool isStart;
	public string stopName;                                                     // Name of the stop
	public GameObject stopGO;                                                   // GameObject spawned when stop is created
	private Dictionary<Stop, bool> LinesDrawn = new Dictionary<Stop, bool>();   //List of lines drawn between this stop and its connecting ones
	private bool ishighlighted;
	public Renderer rend;



	#endregion Contains fields for this class
	protected List<GameObject> idlePlayers;

	// Used in Editor to update colour in edit mode when a value changes in the inspector, can ignore
	void OnValidate()
	{
		stopColour = setColour();

	}

	/// <summary>
	/// Set  stopColour to a colour depending on the Enum
	/// @see colourName
	/// </summary>
	/// <returns>Color of stop</returns>
	private Color setColour()
	{

		switch (currentColour)
		{
		case colourName.Red:
			return Color.red;

		case colourName.Blue:
			return Color.blue;

		case colourName.Yellow:
			return Color.yellow;

		case colourName.Black:
			return Color.black;

		default:
			return Color.black;
		}

	}

	/// <summary>
	/// Used as a virtual start for inherited classes, ensures this code is ran for all stops
	/// </summary>
	protected virtual void Start()
	{


		//create stop gameobject with position and rotation equal to marker and set marker as parent
		var stop = (GameObject)Instantiate(stopGO, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);


		// Log creation
		Debug.Log("Making station: " + stopName);
		rend = stop.GetComponent<Renderer> ();

		//Set material of this clone to the stops colour
		rend.material.color = setColour();
		//sets the highlight property
		rend.material.SetFloat("_OutlineWeight",1.0f);
		setIshiglighted(false);





		// Add each stop to the linesDrawn map and set there value to false
		foreach (Stop s in connectingStops)
		{
			LinesDrawn.Add(s, false);
		}


		Debug.Log("currently ishiglighted: "+ getIsHighlighted());

	}

	/// <summary>
	/// checks if the current stop is highlighted
	/// </summary>
	/// <returns>true or false</returns>
	public bool getIsHighlighted()
	{
		return ishighlighted;
	}

	/// <summary>
	/// Set  ishighlighted to true or false while changing the stop's outline weight accordingly
	/// </summary>
	public void setIshiglighted(bool b)
	{

		if (b == true) {
			rend.material.SetFloat("_OutlineWeight",1.8f);
			ishighlighted = true;

		} else {
			rend.material.SetFloat("_OutlineWeight",1.0f);
			ishighlighted = false;
		}
	}

	/// <summary>
	/// Checks wheter a stop is within it's connected stops
	/// <returns>
	/// true or false
	/// </returns>
	/// </summary>
	public bool isContains(GameObject stop)
	{
		foreach (Stop s in connectingStops)
		{
			if(s.name == stop.name)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Compares a stop colour to another stop colour
	/// <returns>
	/// true or false
	/// </returns>
	/// </summary>
	public bool isSameColour(GameObject astop)
	{
		if(currentColour == astop.GetComponent<SmallStation>().currentColour|| astop.GetComponent<SmallStation>().currentColour == colourName.Black)
		{
			return true;
		}
		return false;
	}


	#region Getter Methods

	public Stop[] getConnectedStations()
	{
		return connectingStops;
	}

	public Dictionary<Stop, bool> getLinesDrawn()
	{
		return LinesDrawn;
	}

	public Color getStopColour()
	{
		return stopColour;
	}

	public bool checkIsStart()
	{
		return isStart;
	}
	#endregion

	#region Setter Methods
	public void setLinesDrawn(Stop stopToSet, bool state)
	{
		LinesDrawn[stopToSet] = state;
	}
	#endregion

	#region Debug Methods

	public void testMethod()
	{
		Debug.Log("Button done something", gameObject);
	}
	#endregion
}
