using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

	public GameObject TextBox;
	public int randomNumber;



	public void generateNumber () {

		randomNumber = Random.Range(1,7);
		TextBox.GetComponent<Text>().text = "" + randomNumber;

	
	}
	

		
	}
