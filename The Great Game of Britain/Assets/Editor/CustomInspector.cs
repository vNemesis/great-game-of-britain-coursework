#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExternalTools))]
public class CustomInspector : Editor
{
    //int index = 0;
    //string[] options = new string[] { "Red", "Blue", "Yellow", "Black" };

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    Rect r = EditorGUILayout.BeginHorizontal();
    //    index = EditorGUILayout.Popup("Awesome Drop down:",
    //         index, options, EditorStyles.popup);
    //    EditorGUILayout.EndHorizontal();
    //}
}
#endif

//public override void OnInspectorGUI()
//{
//    DrawDefaultInspector();

//    StationSyncScript myScript = (StationSyncScript)target;

//    if (GUILayout.Button("Sync Stations"))
//    {
//        myScript.SyncStations();
//    }
//}
