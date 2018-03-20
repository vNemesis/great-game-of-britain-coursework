using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float scrollSpeed = 1.0f;
    [SerializeField] private float zoomSpeed = 50.0f;

    public bool followPlayer = false;

    private bool disabled;
    private bool keyOnly;

    [SerializeField] private Text disabledText;

    // Use this for initialization
    void Start()
    {

        Cursor.lockState = CursorLockMode.Confined;

        keyOnly = !(SettingsManager.isSettingTrue("mouseScroll"));

    }

    // Update is called once per frame
    void Update()
    {

        if (!followPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                disabled = !disabled;
                if (disabled)
                {
                    Debug.LogWarning("Input disabled");
                }
                else
                {
                    Debug.LogWarning("Input enabled");
                }
                disabledText.enabled = disabled;
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                keyOnly = !keyOnly;
                Debug.LogWarning("Only Keyboard = " + keyOnly);
            }

            if (disabled)
            {
                return;
            }

            // Update position with input

            if (!disabled && !followPlayer)
            {
                gameObject.transform.Translate(Vector3.forward * Input.GetAxis("Vertical"));
                gameObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal"));
                gameObject.transform.Translate(Vector3.up * Input.GetAxis("Zoom"));
            }

            float directionY = 0;
            float directionX = 0;

            if (!disabled && !keyOnly && !followPlayer && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.mousePosition.y > (Screen.height * 0.9))
                {
                    directionY = scrollSpeed;
                }
                else if (Input.mousePosition.y < (Screen.height * 0.1))
                {
                    directionY = -scrollSpeed;
                }

                if (Input.mousePosition.x > (Screen.width * 0.9))
                {
                    directionX = scrollSpeed;
                }
                else if (Input.mousePosition.x < (Screen.width * 0.1))
                {
                    directionX = -scrollSpeed;
                }
            }

            gameObject.transform.Translate(Vector3.forward * directionY);
            gameObject.transform.Translate(Vector3.right * directionX);
            gameObject.transform.Translate(Vector3.up * (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed));

            // Get current position

            Vector3 pos = gameObject.transform.position;
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;

            // Boundary transformations

            if (y < 40.0f)
            {
                y = 40.0f;
            }
            else if (y > 100.0f)
            {
                y = 100.0f;
            }

            if (x < 60.0f)
            {
                x = 60.0f;
            }
            else if (x > 940.0f)
            {
                x = 940.0f;
            }

            if (z < 60.0f)
            {
                z = 60.0f;
            }
            else if (z > 940.0f)
            {
                z = 940.0f;
            }

            gameObject.transform.position = new Vector3(x, y, z);
        }
        else
        {

            GameObject player = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().getCurrentPlayer();

            gameObject.transform.position = new Vector3(player.transform.position.x, 70, player.transform.position.z - 50);

            // Get current position

            Vector3 pos = gameObject.transform.position;
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;

            // Boundary transformations

            if (y < 40.0f)
            {
                y = 40.0f;
            }
            else if (y > 100.0f)
            {
                y = 100.0f;
            }

            if (x < 60.0f)
            {
                x = 60.0f;
            }
            else if (x > 940.0f)
            {
                x = 940.0f;
            }

            if (z < 60.0f)
            {
                z = 60.0f;
            }
            else if (z > 940.0f)
            {
                z = 940.0f;
            }

            gameObject.transform.position = new Vector3(x, y, z);
        }

    }

    public void toggleFollowMode()
    {
        followPlayer = !followPlayer;

        Debug.LogWarning("Following player: " + GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>().getCurrentPlayer().GetComponent<PlayerController>().getPlayerNumber());
    }
}
