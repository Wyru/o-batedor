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
    public UnityEvent OnNoCards;

    public void Chase(){
        CardKnock.CardKnockController.Instance.StartMatch(cards, this);
    }


    public void PlayerWin(){
        if(OnNoCards != null){
            OnNoCards.Invoke();
        }
    }

}
