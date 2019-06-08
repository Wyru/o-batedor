using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using UnityEngine.Events;
public class OpponentCharacter : MonoBehaviour
{
    public Card[] cards;
    
    public enum Level
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
    public Level difficult;
    public UnityEvent OnPlayerWin;
    public UnityEvent OnPlayerLost;

    public void Chase(){
        CardKnock.CardKnockController.Instance.StartMatch(cards, difficult);
    }

}
