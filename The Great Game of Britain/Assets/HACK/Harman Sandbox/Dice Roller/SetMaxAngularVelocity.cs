using UnityEngine;
using System.Collections;

public class SetAngularVelocityOnMax : MonoBehaviour
{
	public float maxAngularVelocity = 7.0f;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
	}
}
