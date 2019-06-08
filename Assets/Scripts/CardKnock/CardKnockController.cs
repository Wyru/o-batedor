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

        bool camChanged;

        [HideInInspector] public List<Card> playerCards;
        [HideInInspector] public List<Card> opponentCards;

        [HideInInspector] public OpponentCharacter.Level difficult;

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

        private void Start()
        {
            _instance = this;
        }

        public void StartMatch(Card[] opponentCards, OpponentCharacter.Level difficult)
        {
            this.playerCards = player.List;
            this.opponentCards = new List<Card>(opponentCards);
            this.difficult = difficult;
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
                        state = States.StartGame;
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

                    break;


                case States.PlayerTurn:

                    break;

                case States.OpponentTurn:

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
                // GambleController.Instance.StartGamble(jokenpoController.playerHasWin);
                state = States.Gambling;
                
            }
        }


        void UpdateGambling()
        {

        }


    }
}
