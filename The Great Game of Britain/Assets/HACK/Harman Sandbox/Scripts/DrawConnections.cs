using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawConnections : MonoBehaviour {

    private GameObject[] stations;
    public GameObject connectionGO;
    private bool drawn = false;

    public float countdown = 1.0f;

	// Use this for initialization
	void Start ()
    {
        stations = GameObject.FindGameObjectsWithTag("Station");
    }

    private void Update()
    {
        countdown -= 1.0f * Time.deltaTime;

        if (countdown <= 0.0f && !drawn)
        {
            drawStationConnections();
            Debug.Log("Drawning Connections");
            drawn = true;
        }
        else if (countdown <= 0.0f && drawn)
        {
            countdown = 0.0f;
        }
    }

    private void drawStationConnections()
    {
        foreach (GameObject g in stations)
        {
            Stop[] stops = g.GetComponent<Stop>().getConnectedStations();
            Dictionary<Stop, bool> linesDrawn = g.GetComponent<Stop>().getLinesDrawn();

            Vector3 stationLocation = g.transform.position;

            foreach (Stop s in stops)
            {
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


                    linesDrawn[s] = true;
                    s.setLinesDrawn(g.GetComponent<Stop>(), true);
                }

            }

        }


    }
	
}
