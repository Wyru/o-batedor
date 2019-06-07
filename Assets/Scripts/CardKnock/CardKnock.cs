using UnityEngine;
using UnityEngine.SceneManagement;
using Database;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
public class CardKnock: MonoBehaviour {

    static CardKnock _instance;

    public static CardKnock Instance{
        get{
            return _instance;
        }
    }

    [Header("Settings")]
    public Camera duelCam;
    public PlayerCards player;
    public Animator transition;
    [Range(0f,2f)]
    public float transitionTime;
    [Range(0f,1f)]
    public float changeCamTime;


    bool camChanged;
    
    [ HideInInspector] public List<Card> playerCards;
    [ HideInInspector] public List<Card> opponentCards;

    [ HideInInspector] public OpponentCharacter.Level difficult;

    public enum States
    {
        Jokenpo,
        Gambling,
        PlayerTurn,
        OpponentTurn,
        GameEnd
    }
    

    private void Start() {
        _instance = this;
    }

    public void StartMatch(Card[] opponentCards, OpponentCharacter.Level difficult){
        this.playerCards = player.List;
        this.opponentCards = new List<Card>(opponentCards);
        this.difficult = difficult;
        Debug.Log("Start Match");
        StartCoroutine("TransitionIn");
    }


    IEnumerator TransitionIn(){
        transition.SetTrigger("play");
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if(timer >= transitionTime){
                break;
            }
            else{
                if ((timer/transitionTime) > changeCamTime && !camChanged)
                {
                    ChangeCurrentCam();
                }
            }

            yield return null;
        }
    }

    public void ChangeCurrentCam(){
        Debug.Log("Change cam");
        camChanged = !camChanged;
        duelCam.gameObject.SetActive(camChanged);
    }
}