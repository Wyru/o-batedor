using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Album
{
    public class PageBehavior : MonoBehaviour
    {

        public Transform front;
        public Transform back;

        public void ShowFront(Quaternion cardRotation)
        {
            front.SetAsLastSibling();
            foreach (Card card in front.GetComponentsInChildren<Card>())
            {
                card.SetImage(true);
                card.transform.localRotation = cardRotation;
            }
        }

        public void ShowBack(Quaternion cardRotation)
        {
            back.SetAsLastSibling();
            foreach (Card card in front.GetComponentsInChildren<Card>())
            {
                card.SetImage(false);
                card.transform.localRotation = cardRotation;
            }
        }
    }
}

