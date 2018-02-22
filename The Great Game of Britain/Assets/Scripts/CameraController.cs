using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		// Update position with input

		gameObject.transform.Translate(Vector3.forward * Input.GetAxis("Vertical"));

		gameObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal"));

		gameObject.transform.Translate(Vector3.up * Input.GetAxis("Zoom"));

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
