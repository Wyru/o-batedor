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

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward*-1);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction,Color.red,.01f);
        if(Physics.Raycast(ray, out hit)){


        }
        
    }
}
