using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


namespace CardKnock
{
    public class GambleController : MonoBehaviour
    {
        static GambleController _instance;

        public static GambleController Instance
        {
            get
            {
                return _instance;
            }
        }

        CardKnockController cardKnock;

        List<Database.Card> playerCardsInHand;
        List<Database.Card> playerCardsInBet;

        List<Database.Card> opponentCardsInHand;
        List<Database.Card> opponentCardsInBet;


        public GameObject playerBet;
        public GameObject playerHand;


        public GameObject opponentBet;
        public GameObject opponentHand;

        public List<CardGamble> playerCardsInHandGUI;
        public List<CardGamble> playerCardsInBetGUI;

        public List<CardGamble> opponentCardsInHandGUI;
        public List<CardGamble> opponentCardsInBetGUI;


        public GameObject cardGamblePrefab;
        public GameObject opponentCardGamblePrefab;


        int playerBetValue;
        int opponentBetValue;


        bool playerStarted;


        public TextMeshProUGUI playerBetValueGUI;
        public TextMeshProUGUI opponentBetValueGUI;



        public Button endGambling;

        enum States
        {
            Nothing,
            Start,
            PlayerGambling,
            OpponentGambling,
            EndGambling
        }


        [SerializeField] States state;


        public Image cardInfoImage;
        public TextMeshProUGUI cardInfoName;
        public TextMeshProUGUI cardInfoDescription;
        public TextMeshProUGUI cardInfoRarity;

        public Animator gambleAnimator;

        public bool playerGambled;
        public bool opponentGambled;


        public bool finished;

        public bool gameEnded;

        private void Start()
        {
            _instance = this;
            cardKnock = FindObjectOfType<CardKnockController>();
        }


        public void StartGamble(bool playerStart)
        {
            gambleAnimator.gameObject.SetActive(true);
            playerStarted = playerStart;
            playerCardsInBet = new List<Database.Card>();
            opponentCardsInBet = new List<Database.Card>();
            playerCardsInBetGUI = new List<CardGamble>();
            opponentCardsInBetGUI = new List<CardGamble>();
            

            playerCardsInHand = new List<Database.Card>(cardKnock.playerCards);
            opponentCardsInHand = new List<Database.Card>(cardKnock.opponentCards);

            playerCardsInHand.Sort((card_a, card_b) => card_a.name.CompareTo(card_b.name));
            opponentCardsInHand.Sort((card_a, card_b) => card_a.name.CompareTo(card_b.name));

            playerCardsInHandGUI = new List<CardGamble>();
            opponentCardsInHandGUI = new List<CardGamble>();


            foreach (Database.Card card in playerCardsInHand)
            {
                CardGamble cardGamble = Instantiate(cardGamblePrefab, playerHand.transform).GetComponent<CardGamble>();
                cardGamble.Setup(card, false);
                playerCardsInHandGUI.Add(cardGamble);
            }

            foreach (Database.Card card in opponentCardsInHand)
            {
                CardGamble cardGamble = Instantiate(opponentCardGamblePrefab, opponentHand.transform).GetComponent<CardGamble>();
                cardGamble.Setup(card, true);
                opponentCardsInHandGUI.Add(cardGamble);
            }

            if (playerStarted)
            {
                state = States.PlayerGambling;
            }
            else
            {
                state = States.OpponentGambling;
            }
            
            gameEnded = false;
            finished = false;


        }



        public void ShowCardInfo(Database.Card card)
        {
            gambleAnimator.SetBool("cardInfo", true);
            cardInfoImage.sprite = card.front;
            cardInfoName.SetText(card.name);
            cardInfoDescription.SetText(card.description);
            cardInfoRarity.SetText(card.rarity.ToString());

        }

        public void HideCardInfo()
        {
            gambleAnimator.SetBool("cardInfo", false);
        }

        public void PlayerBetCard(Database.Card card)
        {
            CardGamble cardGamble;
            playerBetValue += (int)card.rarity + 1;
            playerBetValueGUI.SetText(playerBetValue.ToString());
            for (int i = 0; i < playerCardsInHandGUI.Count; i++)
            {
                if (card == playerCardsInHandGUI[i].card)
                {
                    cardGamble = playerCardsInHandGUI[i];
                    playerCardsInHandGUI.Remove(cardGamble);
                    cardGamble.transform.SetParent(playerBet.transform);
                    playerCardsInBetGUI.Add(cardGamble);
                    break;
                }
            }
        }

        public void PlayerUnBetCard(Database.Card card)
        {
            CardGamble cardGamble;
            playerBetValue += ((int)card.rarity + 1) * -1;
            playerBetValueGUI.SetText(playerBetValue.ToString());
            for (int i = 0; i < playerCardsInBetGUI.Count; i++)
            {
                if (card == playerCardsInBetGUI[i].card)
                {
                    cardGamble = playerCardsInBetGUI[i];
                    playerCardsInBetGUI.Remove(cardGamble);
                    cardGamble.transform.SetParent(playerHand.transform);
                    playerCardsInHandGUI.Add(cardGamble);
                    break;
                }
            }
        }


        private void Update()
        {
            if (finished)
            {
                return;
            }

            switch (state)
            {
                case States.Start:

                    break;

                case States.PlayerGambling:
                    PlayerGambling();
                    break;

                case States.OpponentGambling:
                    OpponentGambling();

                    break;

                case States.EndGambling:

            
                    break;
            }
        }


        public void EndGambling()
        {
            if (state == States.PlayerGambling)
            {

                playerGambled = true;

                if (!opponentGambled)
                {
                    state = States.OpponentGambling;
                }
                else
                {
                    state = States.EndGambling;
                    gambleAnimator.SetTrigger("Out");
                }
            }
            else if (state == States.OpponentGambling)
            {
                opponentGambled = true;
                if (!playerGambled)
                {
                    state = States.PlayerGambling;
                }
                else
                {
                    state = States.EndGambling;
                    gambleAnimator.SetTrigger("Out");
                }
            }
        }

        void PlayerGambling()
        {
            if (playerStarted)
            {
                endGambling.interactable = playerBetValue > 0;
            }
            else
            {
                endGambling.interactable = playerBetValue >= opponentBetValue;
            }
        }


        void OpponentGambling()
        {
            if (playerStarted)
            {
                endGambling.interactable = playerBetValue > 0;
            }
            else
            {
                endGambling.interactable = playerBetValue >= opponentBetValue;
            }

            List<Database.Card> cards = new List<Database.Card>();

            if (playerStarted)
            {
                if (CardKnock.CardKnockController.Instance.difficult == OpponentCharacter.Level.Easy)
                {
                    for (int i = 0; i < opponentCardsInHandGUI.Count; i++)
                    {
                        cards.Add(opponentCardsInHandGUI[i].card);

                        opponentBetValue += ((int)opponentCardsInHandGUI[i].card.rarity + 1);

                        if (opponentBetValue >= playerBetValue)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (CardKnock.CardKnockController.Instance.difficult == OpponentCharacter.Level.Easy)
                {

                    for (int i = 0; i < 2 && i < opponentCardsInHandGUI.Count; i++)
                    {
                        cards.Add(opponentCardsInHandGUI[i].card);
                        opponentBetValue += ((int)opponentCardsInHandGUI[i].card.rarity + 1);
                    }
                }
            }

            foreach (Database.Card card in cards)
            {
                OpponentBetCard(card);
            }

            opponentBetValueGUI.SetText(opponentBetValue.ToString());

            EndGambling();

        }


        public void OpponentBetCard(Database.Card card)
        {
            CardGamble cardGamble;

            for (int i = 0; i < opponentCardsInHandGUI.Count; i++)
            {
                if (card == opponentCardsInHandGUI[i].card)
                {
                    cardGamble = opponentCardsInHandGUI[i];
                    opponentCardsInHandGUI.Remove(cardGamble);
                    cardGamble.transform.SetParent(opponentBet.transform);
                    opponentCardsInBetGUI.Add(cardGamble);
                    cardGamble.Show();
                    break;
                }
            }
        }
        public void Desactive(){
            gambleAnimator.gameObject.SetActive(false);
        }

        public void OutAnimationEnd(){
            finished = true;
            state = States.Nothing;
        }
    }
}
