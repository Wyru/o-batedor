using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Database;
public class PlayerCards : MonoBehaviour {
    public List<Card> list;

    public List<Card> List{
        get{
            return list;
        }
    }

    public Card[] startDeck;

    private void Start() {
        list = new List<Card>(startDeck);
    }


    public void AddCard(Card card){
        list.Add(card);
    }

    public void RemoveCard(Card card){
        list.Remove(card);
    }


}