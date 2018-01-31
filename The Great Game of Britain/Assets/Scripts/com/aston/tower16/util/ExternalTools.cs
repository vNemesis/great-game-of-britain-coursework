using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalTools : MonoBehaviour {

    public void launchWebpage(string webpage)
    {
        System.Diagnostics.Process.Start(webpage);
    }
}
