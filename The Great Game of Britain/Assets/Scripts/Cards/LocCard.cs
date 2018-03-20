using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocCard  {

    private string cardName;
    private string cardDesc;
    private bool complete;

    public LocCard(string name, string desc)
    {
        cardName = name;
        cardDesc = desc;
        complete = false;
    }

    public string getName()
    {
        return cardName;
    }

    public string getDesc()
    {
        return cardDesc;
    }
    
    public bool getComplete()
    {
        return complete;
    }

    public void completed()
    {
        cardName = "COMPLETED!";
        cardDesc = "COMPLETED";
        complete = true;
    }
}
