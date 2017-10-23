using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private GameObject selected;
    private Canvas mainCanvas;
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);

    // Use this for initialization
    void Start ()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Station"))
        {
            if (g.GetComponent<Stop>().checkIsStart())
            {
                gameObject.transform.SetParent(g.transform);
                gameObject.transform.localPosition = idlePos;
            }
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.parent.gameObject.tag == "Station")
                {
                    selected = hitInfo.transform.parent.gameObject;
                    Debug.Log("Selected Station" + selected.GetComponent<Stop>().stopName);
                    mainCanvas.transform.Find("Text_Selection").GetComponent<Text>().text = ("Selected: " + selected.GetComponent<Stop>().stopName);
                }
                else
                {
                    Debug.Log("Didn't select a valid object");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }

	}

    public void moveToSelectedStation()
    {
        if (selected != null)
        {
            gameObject.transform.parent = selected.transform;
            gameObject.transform.localPosition = idlePos;
        }
    }
}
