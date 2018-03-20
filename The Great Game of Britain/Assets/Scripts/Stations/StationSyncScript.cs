using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationSyncScript : MonoBehaviour
{

    public void SyncStations()
    {
        gameObject.GetComponent<Stop>().testMethod();
    }

    public void Start()
    {
        this.GetComponentInChildren<Text>().text = this.GetComponent<Stop>().getStopName();

        if (!this.GetComponent<Stop>().checkIsMainStop())
        {
            this.GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    void OnValidate()
    {
        this.GetComponentInChildren<Text>().text = this.GetComponent<Stop>().getStopName();

    }


}
