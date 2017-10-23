using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stop : MonoBehaviour {

    public enum colourName { Red, Blue, Yellow, Black } 
    public colourName currentColour;

    [SerializeField] private Color stopColour;
    [SerializeField] private Stop[] connectingStops;
    [SerializeField] private GameObject[] playersOnStop;
    public bool isStart;
    public string stopName;
    public GameObject stopGO;

    private Dictionary<Stop, bool> LinesDrawn = new Dictionary<Stop, bool>();

    void OnValidate()
    {
        stopColour = setColour();
        
    }

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

    protected virtual void Start()
    {
        var stop = (GameObject)Instantiate(stopGO, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
        Debug.Log("Making a station");
        stop.GetComponent<Renderer>().material.color = setColour();
        //stop.tag = "Station";

        foreach (Stop s in connectingStops)
        {
            LinesDrawn.Add(s, false);
        }

    }

    public Stop[] getConnectedStations()
    {
        return connectingStops;
    }

    public Dictionary<Stop, bool> getLinesDrawn()
    {
        return LinesDrawn;
    }

    public void setLinesDrawn(Stop stopToSet, bool state)
    {
        LinesDrawn[stopToSet] = state;
    }

    public Color getStopColour()
    {
        return stopColour;
    }

    public void testMethod()
    {
        Debug.Log("Button done something", gameObject);
    }

    public bool checkIsStart()
    {
        return isStart;
    }


    //private void addStationToArray()
    //{
    //    foreach (Stop s in connectingStops)
    //    {
    //        s.getConnectedStations
    //    }
    //}

}
