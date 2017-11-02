using UnityEngine;
using System.Collections;

public class ThrowDice : MonoBehaviour
{
	public string buttonName = "MoveDice";
	public float forceAmount = 10.0f;
	public float spinnAmount = 10.0f;
	public ForceMode forceMode;

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown(buttonName))
		{
			GetComponent<Rigidbody>().AddForce(Random.onUnitSphere*forceAmount,forceMode);
			GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere*spinnAmount,forceMode);
		}
	}
}
