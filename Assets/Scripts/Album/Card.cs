using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Album
{
    public class Card : MonoBehaviour
    {
        public Database.Card cardData;

        public void Setup(Database.Card cardData){
            this.cardData = cardData;
            GetComponent<Image>().sprite = cardData.front;
        }

        public void SetImage(bool front){
            if(front){
                GetComponent<Image>().sprite = cardData.front;
            }
            else
            {
                GetComponent<Image>().sprite = cardData.cover;
            }
        }

    }
}

