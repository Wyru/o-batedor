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

    public LayerMask whatIsGround;
    public bool faceDown;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward*-0.1f);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction,Color.red,.01f);
        faceDown = Physics.Raycast(ray, out hit, 0.1f,whatIsGround);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Hand")){
            HandBehaviour.Instance.HitCard(this.gameObject);
        }   
    }
}
