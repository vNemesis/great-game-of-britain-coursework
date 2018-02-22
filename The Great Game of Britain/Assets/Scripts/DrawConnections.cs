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
    [SerializeField] private GameObject connectionGO;     // Gameobject which will be spawned as the line marker
                     private bool drawn = false;         // Have all lines been drawn
                     public float countdown = 1.0f;      // Time to wait in seconds
    [SerializeField] private stopConnection[] connections;

    //               private static stopConnection[] buffer;

    #endregion Contains fields for this class

    // Use this for initialization
    void Start ()
    {
        purge();
        stations = GameObject.FindGameObjectsWithTag("Station");
        //connections = buffer;
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
            //drawStationConnections();
            drawStationConnectionsTwo();
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

    private void drawStationConnectionsTwo()
    {
        foreach (stopConnection c in connections)
        {
            // Legacy Support
            c.stop1.addConnectingStop(c.stop2);
            c.stop2.addConnectingStop(c.stop1);

            Stop stop1 = c.stop1;
            Stop stop2 = c.stop2;

            // Find Midpoint betwwen station and stop
            Vector3 midpoint = stop1.gameObject.transform.position + (stop2.gameObject.transform.position - stop1.gameObject.transform.position) / 2;

            //Create connection MArker
            var connMarker = (GameObject)Instantiate(connectionGO, midpoint, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

            //Create an array to hold positions of line
            Vector3[] positions = new Vector3[] { stop1.gameObject.transform.position, stop2.gameObject.transform.position };

            //set the positions of the line and colour
            LineRenderer line = connMarker.GetComponent<LineRenderer>();
            line.SetPositions(positions);
            if (!(stop1.getStopColour() == Color.black))
            {
                line.material.color = stop1.getStopColour();
            }
            else
            {
                line.material.color = stop2.getStopColour();
            }

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
                    if (!(g.GetComponent<Stop>().getStopColour() == Color.black))
                    {
                        line.material.color = g.GetComponent<Stop>().getStopColour();
                    }
                    else
                    {
                        line.material.color = s.gameObject.GetComponent<Stop>().getStopColour();
                    }

                    // set this stops line drawn value to true
                    linesDrawn[s] = true;

                    // set the connected stops value for this stations line drawn value in its map to be true
                    s.setLinesDrawn(g.GetComponent<Stop>(), true);
                }

            }

        }


    }

    #region Helper Methods

    private static void purge()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag("Lines");

        foreach (GameObject g in clones)
        {
            DestroyImmediate(g);
        }
    }

    #endregion

    #region Testing

    //public static void EditorSeeLines(stopConnection[] ListOfConnections, GameObject connectionGO)
    //{
    //    purge();

    //    buffer = ListOfConnections;

    //    foreach (stopConnection c in ListOfConnections)
    //    {
    //        Stop stop1 = c.stop1;
    //        Stop stop2 = c.stop2;

    //        // Find Midpoint betwwen station and stop
    //        Vector3 midpoint = stop1.gameObject.transform.position + (stop2.gameObject.transform.position - stop1.gameObject.transform.position) / 2;

    //        //Create connection MArker
    //        var connMarker = (GameObject)Instantiate(connectionGO, midpoint, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

    //        //Create an array to hold positions of line
    //        Vector3[] positions = new Vector3[] { stop1.gameObject.transform.position, stop2.gameObject.transform.position };

    //        //set the positions of the line and colour
    //        LineRenderer line = connMarker.GetComponent<LineRenderer>();
    //        line.SetPositions(positions);
    //        if (!(stop1.getStopColour() == Color.black))
    //        {
    //            line.material.color = stop1.getStopColour();
    //        }
    //        else
    //        {
    //            line.material.color = stop2.getStopColour();
    //        }
    //    }
    //}

    #endregion
}
