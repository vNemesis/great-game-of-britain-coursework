using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSyncScript : MonoBehaviour
{

    public void SyncStations()
    {
        gameObject.GetComponent<Stop>().testMethod();
    }


}
