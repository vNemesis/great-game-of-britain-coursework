using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StationSyncScript))]
public class CustomInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StationSyncScript myScript = (StationSyncScript)target;

        if (GUILayout.Button("Sync Stations"))
        {
            myScript.SyncStations();
        }
    }

}
