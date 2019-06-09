using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandBehaviour : MonoBehaviour
{
    public Camera cam;

    public enum States
    {
        Nothing,
        Start,
        ChoosingPosition,
        ChargingPower,
        Hiting,
        WaitingEnd

    }

    public States state;

    public Slider powerGauge;
    public Image sliderImage;

    public float powerValue;

    public AnimationCurve powerIncrementRate;
    public Gradient powerGradient;

    public float powerIncrement;

    Animator m_Animator;


    bool growing;

    public float speedUp;


    public List<GameObject> cards;

    static HandBehaviour _instancePlayer;

    public static HandBehaviour InstancePlayer
    {
        get
        {
            return _instancePlayer;
        }
    }

    static HandBehaviour _instanceOpponent;

    public static HandBehaviour InstanceOpponent
    {
        get
        {
            return _instanceOpponent;
        }
    }

    public float forceBase;

    public AnimationCurve forceModifierCurve;
    public AnimationCurve hitModifierCurve;
    public float forceModifier;
    public float hitModifier;

    public bool turnFinished;

    public bool isOpponent;


    // Start is called before the first frame update
    void Start()
    {
        if(this.isOpponent)
            _instanceOpponent = this;
        else _instancePlayer = this;
        
        turnFinished = false;
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.Start:
                Cursor.visible = false;
                state = States.ChoosingPosition;
                break;
            case States.ChoosingPosition:
                UpdatePosition();
                break;
            case States.ChargingPower:
                UpdateChargingPower();
                break;
            case States.Hiting:
                UpdateHiting();
                break;

            case States.WaitingEnd:
                if(AllCardsIdle()){
                    NextTurn();
                }

            break;
        }

    }

    Vector3 opponentTarget;
    void UpdatePosition()
    {
        if (isOpponent)
        {
            int i = Random.Range(0, CardKnock.CardKnockController.Instance.cardsInBet.Count);
            int j = Random.Range(0, 6);
            opponentTarget = CardKnock.CardKnockController.Instance.cardsInBet[i].poits[j].position;
            opponentMousePressed = true;
            transform.position = opponentTarget;
            state = States.ChargingPower;
            powerGauge.gameObject.SetActive(true);
            state = States.ChargingPower;
            powerValue = 0;

        }
        else
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                powerGauge.gameObject.SetActive(true);
                state = States.ChargingPower;
                powerValue = 0;
            }
        }

    }
    bool opponentMousePressed;

    void UpdateChargingPower()
    {


        if (Input.GetMouseButton(0) || opponentMousePressed)
        {

            if (growing)
            {
                powerValue += powerIncrement * powerIncrementRate.Evaluate(powerGauge.value) * Time.deltaTime;
                if (powerGauge.value >= 1)
                {
                    growing = !growing;
                }
            }
            else
            {
                powerValue -= powerIncrement * powerIncrementRate.Evaluate(powerGauge.value) * Time.deltaTime;
                if (powerGauge.value == 0)
                {
                    growing = !growing;
                }
            }

            powerGauge.value = powerValue;
            sliderImage.color = powerGradient.Evaluate(powerGauge.value);

            if (opponentMousePressed && isOpponent)
            {

                switch (CardKnock.CardKnockController.Instance.opponent.difficult)
                {
                    case OpponentCharacter.Level.Easy:
                        if (forceModifierCurve.Evaluate(powerGauge.value) > .4f && forceModifierCurve.Evaluate(powerGauge.value) < 1f)
                        {
                            opponentMousePressed = !(Random.Range(0, 10) > 5);
                        }
                        break;

                    case OpponentCharacter.Level.Medium:
                        if (forceModifierCurve.Evaluate(powerGauge.value) > .6f && forceModifierCurve.Evaluate(powerGauge.value) < .9f)
                        {
                            opponentMousePressed = !(Random.Range(0, 10) > 4);
                        }
                        break;

                    case OpponentCharacter.Level.Hard:
                        if (forceModifierCurve.Evaluate(powerGauge.value) > .6f && forceModifierCurve.Evaluate(powerGauge.value) < .8f)
                        {
                            opponentMousePressed = !(Random.Range(0, 10) > 5);
                        }
                        break;

                    case OpponentCharacter.Level.Expert:
                        if (forceModifierCurve.Evaluate(powerGauge.value) > .7f && forceModifierCurve.Evaluate(powerGauge.value) < .8f)
                        {
                            opponentMousePressed = !(Random.Range(0, 10) > 2);
                        }
                        break;

                }


            }

        }
        else if (Input.GetMouseButtonUp(0) || (!opponentMousePressed && isOpponent))
        {
            state = States.Hiting;
            forceModifier = forceModifierCurve.Evaluate(powerGauge.value);
            m_Animator.SetFloat("SpeedMultiplier", 1 + speedUp * powerGauge.value);
            m_Animator.SetTrigger("Hit");
        }

    }

    void UpdateHiting()
    {
        float progress = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (hitImpact)
        {
            if (isOpponent)
            {
                if (!opponentMousePressed)
                {

                    switch (CardKnock.CardKnockController.Instance.opponent.difficult)
                    {
                        case OpponentCharacter.Level.Easy:
                            if (hitModifierCurve.Evaluate(progress) > .1f && hitModifierCurve.Evaluate(progress) < 1f)
                            {
                                opponentMousePressed = !(Random.Range(0, 10) > 5);
                            }
                            break;

                        case OpponentCharacter.Level.Medium:
                            if (hitModifierCurve.Evaluate(progress) > .2f && hitModifierCurve.Evaluate(progress) < .8f)
                            {
                                opponentMousePressed = !(Random.Range(0, 10) > 4);
                            }
                            break;

                        case OpponentCharacter.Level.Hard:
                            if (hitModifierCurve.Evaluate(progress) > .3f && hitModifierCurve.Evaluate(progress) < .7f)
                            {
                                opponentMousePressed = !(Random.Range(0, 10) > 4);
                            }
                            break;

                        case OpponentCharacter.Level.Expert:
                            if (hitModifierCurve.Evaluate(progress) > .40f && hitModifierCurve.Evaluate(progress) < .50f)
                            {
                                opponentMousePressed = !(Random.Range(0, 10) > 2);
                            }
                            break;
                    }
                }
                else
                {
                    hitModifier = hitModifierCurve.Evaluate(progress);
                    m_Animator.SetTrigger("Click");
                    TryFlipCards();
                }
            }
            else
            {
                if (progress >= .9)
                {
                    hitModifier = hitModifierCurve.Evaluate(progress);
                    TryFlipCards();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    hitModifier = hitModifierCurve.Evaluate(progress);
                    m_Animator.SetTrigger("Click");
                    TryFlipCards();
                }
            }
        }



    }

    private void TryFlipCards()
    {
        foreach (GameObject card in cards)
        {
            Vector3 force = card.transform.forward * -1;
            force = force * (0.5f * hitModifier * forceBase) + force * (0.4f * forceModifier * forceBase) + force * (0.4f * forceBase * Random.Range(0.01f, 0.1f));
            Debug.Log(force);

            card.GetComponent<Rigidbody>().AddForceAtPosition(force, transform.position, ForceMode.Impulse);
        }
        EndTurn();

    }

    public void HitCard(GameObject card)
    {
        cards.Add(card);
    }


    public void StartTurn()
    {
        Debug.Log("Turn Start " + gameObject.name);
        turnFinished = false;
        hitImpact = false;
        state = States.Start;
    }

    public void EndTurn()
    {
        state = States.WaitingEnd;
        powerGauge.gameObject.SetActive(false);
    }


    public void NextTurn()
    {
        turnFinished = true;
    }

    bool hitImpact;
    public void StartHitImpact()
    {
        hitImpact = true;
    }


    public bool AllCardsIdle(){
        foreach (GameObject card in cards)
        {
            if(Mathf.Abs(card.GetComponent<Rigidbody>().velocity.magnitude) > 0.05 ){
                return false;
            }
        }

        return true;
    }

    


}
