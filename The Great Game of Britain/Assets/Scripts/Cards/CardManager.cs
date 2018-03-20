using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager  {

    private ArrayList locationCardDeck;

	public CardManager () {
        //initialise arraylist
        locationCardDeck = new ArrayList();
        //load the data
        TextAsset cityData = Resources.Load<TextAsset>("CityList");
        //split line by line
        string[] data = cityData.text.Split(new char[] { '\n' });

        for (int i = 0; i < data.Length; i++)
        {
            //split data by comma
            string[] cardData =  data[i].Split(',');
            //create a card and add it do the deck.
            LocCard locationCard = new LocCard(cardData[0], cardData[1]);
            locationCardDeck.Add(locationCard);

        }
    }
	
    public LocCard[] dealCards(int numCards)
    {
        //create a temp array of type LocCard to return, with numCards as capacity.
        LocCard[] cards = new LocCard[numCards];

        //loop for numCards
        for (int i = 0; i < numCards; i++)
        {
            //get a random int with range 1 and the deck size
            var randomInt = Random.Range(1, locationCardDeck.Count);
            Debug.Log("random int: " + randomInt);
            Debug.Log("deck size" + locationCardDeck.Count);
            LocCard card = (LocCard)locationCardDeck[randomInt];
            locationCardDeck.Remove(card);
            cards[i] = card;
            //add the card to the array that we are returning.
        }

        //return the array of cards;
        return cards;
    }



}
