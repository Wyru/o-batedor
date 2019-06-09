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

    static HandBehaviour _instance;

    public static HandBehaviour Instance{
        get{
            return _instance;
        }
    }

    public float forceBase;


    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
        state = States.Start;
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

        }

    }


    void UpdatePosition()
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


    void UpdateChargingPower()
    {

        if (Input.GetMouseButton(0))
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

        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = States.Hiting;
            m_Animator.SetFloat("SpeedMultiplier", 1 + speedUp * powerGauge.value);
            m_Animator.SetTrigger("Hit");
        }
    }

    void UpdateHiting()
    {
        float progress = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (progress >= .9)
        {
            state = States.Start;
            Debug.Log("Muiiiiito lendo");
            TryFlipCards();
            cards = new List<GameObject>();
        }
        else if (Input.GetMouseButtonDown(0))
        {

            if (progress > 0.66)
            {
                m_Animator.SetTrigger("Click");
            }
            else if (progress > 0.33)
            {
                m_Animator.SetTrigger("Click");
            }
            else
            {
                m_Animator.SetTrigger("Click");
            }
            TryFlipCards();
            state = States.Start;
            cards = new List<GameObject>();
        }
    }

    private void TryFlipCards(){
        foreach (GameObject card in cards)
        {
            card.GetComponent<Rigidbody>().AddForceAtPosition(card.transform.forward*-1*forceBase*powerValue,transform.position, ForceMode.Impulse);
        }
    }

    public void HitCard(GameObject card){
        cards.Add(card);
    }

    
}
