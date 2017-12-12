using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.Translate(Vector3.forward * Input.GetAxis("Vertical"));

        gameObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal"));

        gameObject.transform.Translate(Vector3.up * Input.GetAxis("Zoom"));

        if (gameObject.transform.position.y < 40.0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 40.0f, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.y > 100.0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 100.0f, gameObject.transform.position.z);
        }

        if (gameObject.transform.position.x < 60.0f)
        {
            gameObject.transform.position = new Vector3(60.0f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x > 940.0f)
        {
            gameObject.transform.position = new Vector3(940.0f, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (gameObject.transform.position.z < 60.0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 60.0f);
        }
        else if (gameObject.transform.position.z > 940.0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 940.0f);
        }

    }
}
