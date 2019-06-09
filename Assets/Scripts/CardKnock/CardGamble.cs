using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace CardKnock
{


    public class CardGamble : MonoBehaviour
    {

        public TextMeshProUGUI cost;
        public Database.Card card;

        bool isInHand = true;

        bool isOpponent;

        public void Setup(Database.Card card, bool isOpponent)
        {
            this.card = card;
            this.isOpponent = isOpponent;


            if (isOpponent)
            {
                GetComponent<Image>().sprite = card.cover;
            }
            else
            {
                cost.SetText(((int)card.rarity + 1).ToString());
                GetComponent<Image>().sprite = card.front;
            }
        }

        public void OnClick()
        {
            if(!GambleController.Instance.finished){
                if (isInHand)
                    GambleController.Instance.PlayerBetCard(card);
                else
                    GambleController.Instance.PlayerUnBetCard(card);

                isInHand = !isInHand;
            }
        }

        public void OnMouseEnter()
        {
            if(!isOpponent || isOpponent && !isInHand){
                GambleController.Instance.ShowCardInfo(card);
            }
        }

        public void OnMouseExit()
        {
            if(!isOpponent || isOpponent && !isInHand){
                GambleController.Instance.HideCardInfo();
            }
        }

        public void Show(){
            GetComponent<Image>().sprite = card.front;
            isInHand = false;
        }

    }
}
