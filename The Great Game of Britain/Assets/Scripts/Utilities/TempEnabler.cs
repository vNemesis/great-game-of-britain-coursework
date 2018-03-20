using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to stop error spamming the console. breigly enables then disbaled the controls
/// </summary>
public class TempEnabler : MonoBehaviour
{
    [SerializeField] private float i;
    private bool complete = false;

    private void Update()
    {
        if (i >= 0)
        {
            i -= 2.0f * Time.deltaTime;
            return;
        }
        else if (!complete)
        {
            complete = true;
            this.GetComponent<Toggle>().isOn = false;
        }



        
    }
}
