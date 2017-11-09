using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    #region Fields

    private GameObject selected;                                // Currently selected object
    private Canvas mainCanvas;                                  // Main canvas in scene for UI
    private Vector3 idlePos = new Vector3(0.0f, 0.5f, 0.0f);    // On a stop what local position will the player be located at
    public bool controllerEnabled { get; set; }

    #endregion Contains fields for this class
    private float speed;
    private bool moveable;
    private Vector3 targetPosition;
    private int travels;
    private GameObject currentStation;

    public Text movePermisson;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponentInChildren<Animator>().enabled = false;
        controllerEnabled = false;
        speed = 10;
        moveable = false;
        // Fetch main canvas from scene
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();


    }

    // Update is called once per frame
    void Update()
    {
        if (controllerEnabled)
        {
            currentStation = transform.GetComponentInParent<SmallStation>().gameObject;
            Debug.Log(currentStation.ToString());
            // If left-mouse is pressed and currently not hovering over a UI element
            if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
            {
                Debug.Log("Mouse is down");

                // Send a raycast
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

                // if it hit something
                if (hit)
                {
                    // if it hit a station
                    if (hitInfo.transform.parent.gameObject.tag == "Station")
                    {
                        // set selected to equal that station
                        selected = hitInfo.transform.parent.gameObject;
                        Stop[] connectedStations = selected.transform.GetComponentInParent<SmallStation>().getConnectedStations();

                        if (currentStation.GetComponent<SmallStation>().isContains(selected))
                        {
                            movePermisson.text = ("Accessible: Yes");
                        }
                        else
                        {
                            movePermisson.text = ("Accessible: No");
                        }

                        Debug.Log("Selected Station" + selected.GetComponent<Stop>().stopName);
                        Debug.Log("num player: " + selected.transform.childCount);

                        //update UI text on main canvas to display selected stops name
                        mainCanvas.transform.Find("Text_Selection").GetComponent<Text>().text = ("Selected: " + selected.GetComponent<Stop>().stopName);
                    }
                    // else if it's not a station
                    else
                    {
                        Debug.Log("Didn't select a valid object");
                    }
                }
                // else if it didn't hit any coliider
                else
                {
                    Debug.Log("No hit");
                }
            }
        }

        movePlayer();
    }

    /// <summary>
    /// Moves player to selected station, checks station children before entering.
    /// </summary>
    public void moveToSelectedStation()
    {
        // if selected is not empty
        if (selected != null && travels != 0 && controllerEnabled)
        {
            if (currentStation.GetComponent<SmallStation>().isContains(selected))
            {
                if (selected.transform.childCount > 1)
                {
                    // change player parent to new station and update position
                    gameObject.transform.parent = selected.transform;

                    // will adjust depending on how many children are attached to the station
                    float position = 0.0f;
                    for (int i = 0; i < 10; i++)
                    {
                        if (!Physics.CheckSphere(selected.transform.position + new Vector3(position, 0.5f, 0.0f), 0.5f)) //checks if the area is empty
                        {
                            targetPosition = selected.transform.position + new Vector3(position, 0.5f, 0.0f);
                            decrementTravels();
                            break;
                        }
                        position = position + 1.0f;
                    }
                }
                else
                {
                    // change player parent to new station and update position
                    gameObject.transform.parent = selected.transform;
                    targetPosition = selected.transform.position + idlePos;
                    decrementTravels();
                }
            }
        }
    }

    /// <summary>
    /// Moves the player by setting movable to true and applying targetPosiiton
    /// </summary>
    public void movePlayer()
    {
        if (moveable)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);//moves towards position of station
            if (targetPosition == transform.position)//once player has reached station moveable is turned false to stop movement
            {
                moveable = false;
            }
        }
    }

    /// <summary>
    /// enables/disables the animation of the player
    /// </summary>
    /// <param name="value"></param>
    public void setEnableAnimation(bool value)
    {
        gameObject.GetComponentInChildren<Animator>().enabled = value;
    }

    /// <summary>
    /// sets the travels for the player
    /// </summary>
    /// <param name="travels"></param>
    public void setTravels(int travels)
    {
        this.travels = travels;
    }

    /// <summary>
    /// returns travels in int
    /// </summary>
    /// <returns></returns>
    public int getTravels()
    {
        return travels;
    }

    /// <summary>
    /// decreases the number of travels by 1. ensures that travels does not go below 0.
    /// </summary>
    public void decrementTravels()
    {
        moveable = true;
        if (travels != 0)
        {
            travels--;
        }
    }


}