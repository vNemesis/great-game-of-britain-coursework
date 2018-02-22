using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Creates an instance of a primitive depending on the option selected by the user.
public class EditorGUILayoutPopup : EditorWindow
{
    public string[] options = new string[] {"Station Marker"};
    public int index = 0;

    //public GameObject connectionGO;
    //public stopConnection[] connections;


    [MenuItem("Tower16/Toolbox")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(EditorGUILayoutPopup));
        window.Show();
    }

    void OnGUI()
    {

        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);

        index = EditorGUILayout.Popup(index, options);
        if (GUILayout.Button("Create Station Marker"))
            InstantiatePrimitive();

        //if (GUILayout.Button("Update Editor Lines"))
        //    UpdateLines();

        //SerializedProperty g = so.FindProperty("connectionGO");
        //EditorGUILayout.ObjectField(g);

        //SerializedProperty stringsProperty = so.FindProperty("connections");

        //EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        //so.ApplyModifiedProperties(); // Remember to apply modified properties
    }

    //void UpdateLines()
    //{
    //   DrawConnections.EditorSeeLines(connections, connectionGO);
    //}

    void InstantiatePrimitive()
    {
        switch (index)
        {
            case 0:
                GameObject stationm = GameObject.Instantiate((GameObject) Resources.Load("StationMarker"));
                stationm.transform.position = Vector3.zero;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }
}
