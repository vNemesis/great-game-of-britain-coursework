using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ------------------------------------------------------------------------------------
/// 
/// Authors : Harman Uppal
/// 
/// ------------------------------------------------------------------------------------
/// </summary>

public class DrawConnections : MonoBehaviour {

    #region Fields
    private GameObject[] stations;      // Array of all the stations in the scene
    public GameObject connectionGO;     // Gameobject which will be spawned as the line marker
    private bool drawn = false;         // Have all lines been drawn

    public float countdown = 1.0f;      // Time to wait in seconds

    #endregion Contains fields for this class

    // Use this for initialization
    void Start ()
    {
        stations = GameObject.FindGameObjectsWithTag("Station");
    }

    /// <summary>
    /// will call draw connections code
    /// </summary>
    private void Update()
    {
        // buffer to allow stations to spawn
        countdown -= 1.0f * Time.deltaTime;

        if (countdown <= 0.0f && !drawn)
        {
            //call method to draw connections
            drawStationConnections();
            Debug.Log("Drawning Connections");

            // set has drawn all lines to true
            drawn = true;
        }
        else if (countdown <= 0.0f && drawn)
        {
            // end countdown
            countdown = 0.0f;
        }
    }

    /// <summary>
    /// Will go through every gameobject tagged "Station" and draw a line between its connection stops
    /// </summary>
    private void drawStationConnections()
    {
        // Iterate through each station
        foreach (GameObject g in stations)
        {
            // Get this station's stops it connects to
            Stop[] stops = g.GetComponent<Stop>().getConnectedStations();

            // Get which stops this stations already has lines drawn to
            Dictionary<Stop, bool> linesDrawn = g.GetComponent<Stop>().getLinesDrawn();

            // store this stations location
            Vector3 stationLocation = g.transform.position;

            // Iterate through each stop in this stations conected stops
            foreach (Stop s in stops)
            {
                // if there is no line between this station and this stop
                if (linesDrawn[s] == false)
                {
                    // Find Midpoint betwwen station and stop
                    Vector3 midpoint = g.transform.position + (s.transform.position - g.transform.position) / 2;

                    //Create connection MArker
                    var connMarker = (GameObject)Instantiate(connectionGO, midpoint, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

                    //Create an array to hold positions of line
                    Vector3[] positions = new Vector3[] { g.transform.position, s.transform.position };

                    //set the positions of the line and colour
                    LineRenderer line = connMarker.GetComponent<LineRenderer>();
                    line.SetPositions(positions);
                    //line.startColor = g.GetComponent<Stop>().getStopColour();
                    //line.endColor = g.GetComponent<Stop>().getStopColour();
                    line.material.color = g.GetComponent<Stop>().getStopColour();

                    // set this stops line drawn value to true
                    linesDrawn[s] = true;

                    // set the connected stops value for this stations line drawn value in its map to be true
                    s.setLinesDrawn(g.GetComponent<Stop>(), true);
                }

            }

        }


    }
	
}
