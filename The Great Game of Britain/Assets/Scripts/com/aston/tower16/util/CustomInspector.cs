using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(StationSyncScript))]
public class CustomInspector : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StationSyncScript myScript = (StationSyncScript)target;

        if (GUILayout.Button("Sync Stations"))
        {
            myScript.SyncStations();
        }
    }
#endif

}
