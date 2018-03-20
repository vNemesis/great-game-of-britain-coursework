using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {

    public LocCard card1;
    public Text nameText1;
    //public Text descText1;
    public Image cardCompleteOverlay1;

    public LocCard card2;
    public Text nameText2;
    //public Text descText2;
    public Image cardCompleteOverlay2;

    public LocCard card3;
    public Text nameText3;
    //public Text descText3;
    public Image cardCompleteOverlay3;

    void Start()
    {
        cardCompleteOverlay1.enabled = false;
        cardCompleteOverlay2.enabled = false;
        cardCompleteOverlay3.enabled = false;
    }


    public void setCard(LocCard[] locationCards)
    {
        //check cards 
        if (locationCards[0] == null)
        {
            Debug.Log("NULLLLLLLLL");
        }
        else
        {
            card1 = locationCards[0];
         
            card2 = locationCards[1];
        
            card3 = locationCards[2];
       


        }
    }

    public void updateCard()
    {
        nameText1.text = card1.getName();
        // descText1.text = card1.getDesc();
    

        nameText2.text = card2.getName();
        // descText2.text = card2.getDesc();
       

        nameText3.text = card3.getName();
        // descText3.text = card3.getDesc();
      

    }
    
   

    public void completeCard(LocCard[] locationCards)
    {
        if (locationCards[0].getComplete())
        {
            cardCompleteOverlay1.enabled = true;
        }
        else
        {
            cardCompleteOverlay1.enabled = false;
        }

        if (locationCards[1].getComplete())
        {
            cardCompleteOverlay2.enabled = true;
        }
        else
        {
            cardCompleteOverlay2.enabled = false;
        }

        if (locationCards[2].getComplete())
        {
            cardCompleteOverlay3.enabled = true;
        }
        else
        {
            cardCompleteOverlay3.enabled = false;
        }
    }

}
