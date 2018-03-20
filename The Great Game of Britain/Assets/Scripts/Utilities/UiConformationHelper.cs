using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiConformationHelper : MonoBehaviour {

    public void ConfirmGameExit(bool answer)
    {
        if (true)
        {
            Application.Quit();
            Debug.LogWarning("Leaving Game");
        }
    }

    public void ConfirmEndTurn(bool answer)
    {
        if (true)
        {
            this.GetComponent<GameManager>().nextPlayerTurn();
        }
    }
}
