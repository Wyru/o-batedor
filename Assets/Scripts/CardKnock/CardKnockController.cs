using UnityEngine;
using UnityEngine.SceneManagement;
using Database;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;


namespace CardKnock
{
    public class CardKnockController : MonoBehaviour
    {

        static CardKnockController _instance;

        public static CardKnockController Instance
        {
            get
            {
                return _instance;
            }
        }

        [Header("Settings")]
        public Camera duelCam;
        public PlayerCards player;
        public Animator transition;
        [Range(0f, 2f)]
        public float transitionTime;
        [Range(0f, 1f)]
        public float changeCamTime;

        public JokenpoController jokenpoController;
        public GambleController gambleController;

        bool camChanged;

        [HideInInspector] public List<Card> playerCards;
        [HideInInspector] public List<Card> opponentCards;

        [HideInInspector] public OpponentCharacter.Level difficult;

        public List<CardBehavior> cardsInBet;

        public GameObject cardContainer;

        public GameObject CardBehaviorPrefab;

        public HandBehaviour playerHandBehavior;
        public HandBehaviour opponentHandBehavior;

        public GameObject arena;

        public enum States
        {
            NotRunnig,
            StartGame,
            Jokenpo,
            Gambling,
            PlayerTurn,
            OpponentTurn,
            GameEnd
        }

        public States state;

        public GameObject Menu;

        private void Start()
        {
            _instance = this;
        }

        public OpponentCharacter opponent;
        public void StartMatch(Card[] opponentCards, OpponentCharacter opponent)
        {
            this.playerCards = player.List;
            this.opponentCards = new List<Card>(opponentCards);
            this.difficult = opponent.difficult;
            this.opponent = opponent;
            StartCoroutine("TransitionIn");
        }

        IEnumerator TransitionIn()
        {
            transition.SetTrigger("play");
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;

                if (timer >= transitionTime)
                {
                    break;
                }
                else
                {
                    if ((timer / transitionTime) > changeCamTime && !camChanged)
                    {
                        ChangeCurrentCam();
                        Menu.SetActive(false);
                        state = States.StartGame;
                    }
                }

                yield return null;
            }
        }


        IEnumerator TransitionOut()
        {
            transition.SetTrigger("play");
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime;

                if (timer >= transitionTime)
                {
                    break;
                }
                else
                {
                    if ((timer / transitionTime) > changeCamTime && camChanged)
                    {
                        playerHandBehavior.gameObject.SetActive(false);
                        opponentHandBehavior.gameObject.SetActive(false);
                        Cursor.visible = true;
                        arena.SetActive(false);
                        SystemsController.RunningCardKnock(false);
                        Menu.SetActive(true);
                        ChangeCurrentCam();
                    }
                }

                yield return null;
            }
        }

        public void ChangeCurrentCam()
        {
            camChanged = !camChanged;
            duelCam.gameObject.SetActive(camChanged);
        }

        private void Update()
        {
            switch (state)
            {
                case States.StartGame:
                    SystemsController.RunningCardKnock(true);
                    jokenpoController.StartJokenpo();
                    state = States.Jokenpo;
                    break;

                case States.Jokenpo:
                    UpdateJokenpo();
                    break;

                case States.Gambling:
                    UpdateGambling();
                    break;

                case States.PlayerTurn:
                    UpdatePlayerTurn();
                    break;

                case States.OpponentTurn:
                    UpdateOpponentTurn();
                    break;

                case States.GameEnd:

                    break;

                default:
                    return;
            }
        }

        void UpdateJokenpo()
        {
            if (jokenpoController.finished)
            {
                GambleController.Instance.StartGamble(jokenpoController.playerHasWin);
                state = States.Gambling;
            }
        }


        void UpdateGambling()
        {
            if (gambleController.finished)
            {
                cardsInBet = new List<CardBehavior>();

                foreach (CardGamble card in GambleController.Instance.opponentCardsInBetGUI)
                {
                    CardBehavior cb = Instantiate(CardBehaviorPrefab, cardContainer.transform).GetComponent<CardBehavior>();
                    cb.Setup(card.card);
                    cardsInBet.Add(cb);
                }

                foreach (CardGamble card in GambleController.Instance.playerCardsInBetGUI)
                {
                    CardBehavior cb = Instantiate(CardBehaviorPrefab, cardContainer.transform).GetComponent<CardBehavior>();
                    cb.Setup(card.card);
                    cardsInBet.Add(cb);
                }

                arena.SetActive(true);

                GambleController.Instance.Desactive();

                if(jokenpoController.playerHasWin){
                    playerHandBehavior.gameObject.SetActive(true);
                    playerHandBehavior.StartTurn();
                    state = States.PlayerTurn;
                }
                else{
                    opponentHandBehavior.gameObject.SetActive(true);
                    opponentHandBehavior.StartTurn();
                    state = States.OpponentTurn;
                }
            }
        }


        void UpdatePlayerTurn(){
            if(playerHandBehavior.turnFinished){
                if(cardsInBet.Count > 0){
                    List<CardBehavior> cardsToRemove = new List<CardBehavior>();
                    foreach (CardBehavior card in cardsInBet)
                    {
                        if(card.faceDown){
                            GambleController.Instance.playerCardsInHand.Add(card.card);
                            cardsToRemove.Add(card);
                        }
                    }

                    foreach (CardBehavior card in cardsToRemove)
                    {
                        if(card.faceDown){
                            cardsInBet.Remove(card);
                            Destroy(card.gameObject);
                        }
                    }

                    if(cardsInBet.Count == 0){
                        GameEnd();
                    }

                    playerHandBehavior.cards = new List<GameObject>();
                    playerHandBehavior.gameObject.SetActive(false);
                    opponentHandBehavior.gameObject.SetActive(true);
                    opponentHandBehavior.StartTurn();
                    state = States.OpponentTurn;
                }
                else
                {
                    GameEnd();
                }
            }
        }

        void UpdateOpponentTurn(){
            if(opponentHandBehavior.turnFinished){

                if(cardsInBet.Count > 0){
                    List<CardBehavior> cardsToRemove = new List<CardBehavior>();
                    foreach (CardBehavior card in cardsInBet)
                    {
                        if(card.faceDown){
                            GambleController.Instance.opponentCardsInHand.Add(card.card);
                            cardsToRemove.Add(card);
                        }
                    }

                    foreach (CardBehavior card in cardsToRemove)
                    {
                        if(card.faceDown){
                            cardsInBet.Remove(card);
                            Destroy(card.gameObject);
                        }
                    }

                    if(cardsInBet.Count == 0){
                        GameEnd();
                    }

                    opponentHandBehavior.cards = new List<GameObject>();
                    opponentHandBehavior.gameObject.SetActive(false);
                    playerHandBehavior.gameObject.SetActive(true);
                    playerHandBehavior.StartTurn();
                    state = States.PlayerTurn;
                }
                else{
                    GameEnd();
                }
            }
        }

        void GameEnd(){
            player.list = GambleController.Instance.playerCardsInHand;
            opponent.cards = GambleController.Instance.opponentCardsInHand.ToArray();
            StartCoroutine("TransitionOut");

            if (opponent.cards.Length == 0)
            {
                opponent.PlayerWin();
            }
        }

    }
}
