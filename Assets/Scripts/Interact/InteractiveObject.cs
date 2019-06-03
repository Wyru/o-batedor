using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InteractiveObject : MonoBehaviour
{


    public UnityEvent OnClick;
    public UnityEvent OnInteract;

    bool nextToPlayer;

    public void Interact()
    {
        if (OnInteract != null)
        {
            OnInteract.Invoke();
        }
    }

    public void Click()
    {
        if (OnClick != null)
        {
            OnClick.Invoke();
        }

        if (nextToPlayer)
        {
            Interact();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nextToPlayer = true;
            if (InteractSystem.Instance.selected == this)
            {
                Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nextToPlayer = false;
        }
    }


}
