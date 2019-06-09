using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    
    Rigidbody m_Rigidbody;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public SpriteRenderer front;
    public SpriteRenderer cover;
    public LayerMask whatIsGround;
    public bool faceDown;

    public Database.Card card;

    public Transform[] poits;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward*-0.1f);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction,Color.red,.01f);
        faceDown = Physics.Raycast(ray, out hit, 0.1f,whatIsGround);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("HandPlayer")){
            Debug.Log(card.name+" Hit by player");
            HandBehaviour.InstancePlayer.HitCard(this.gameObject);
        }   
        if(other.CompareTag("HandOpponent")){
            Debug.Log(card.name+" Hit by opponent");
            HandBehaviour.InstanceOpponent.HitCard(this.gameObject);
        } 
    }

    public void Setup(Database.Card card){
        this.card = card;
        front.sprite = card.front;
        cover.sprite = card.cover;
    }
}
