using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace CardKnock
{
    public class JokenpoController : MonoBehaviour
    {
        public enum Choice
        {
            rock,
            paper,
            scissor
        }

        public Choice playerChoice;
        public Choice opponentChoice;
        public Sprite rockSprite;
        public Sprite paperSprite;
        public Sprite scissorSprite;
        public Image opponentChoiceGUI;
        public Image playerChoiceGUI;
        public Animator jokenpoContainer;
        bool playerHasChoose;
        bool opponentHasChoose;
        bool canNext;
        public bool playerHasWin;
        public bool finished;
        public bool running;

        void Update()
        {
            if (canNext)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    canNext = false;
                    jokenpoContainer.SetTrigger("end_jokenpo");
                    finished = true;
                    running = false;
                }
            }
            else if (playerHasChoose)
            {
                if (!opponentHasChoose)
                {
                    SelectOponnentChoice();
                    SetGraphics();
                    jokenpoContainer.SetTrigger("resolve");
                    HandleWinner();
                }
            }
        }

        void SelectOponnentChoice()
        {
            int choice = UnityEngine.Random.Range(0, 3);
            opponentChoice = (Choice)choice;
            opponentHasChoose = true;
        }

        public void SelectOption(string choice)
        {
            playerChoice = (Choice)Enum.Parse(typeof(Choice), choice);
            playerHasChoose = true;
        }

        void SetGraphics()
        {
            opponentChoiceGUI.sprite = GetArt(opponentChoice);
            playerChoiceGUI.sprite = GetArt(playerChoice);
        }

        void HandleWinner()
        {

            if (playerChoice == opponentChoice)
            {
                Draw();
            }
            else
            {
                if (playerChoice == Choice.paper)
                {
                    if (opponentChoice == Choice.rock)
                    {
                        PlayerWin();
                    }
                    else
                    {
                        PlayerLose();
                    }
                }

                if (playerChoice == Choice.rock)
                {
                    if (opponentChoice == Choice.scissor)
                    {
                        PlayerWin();
                    }
                    else
                    {
                        PlayerLose();
                    }
                }

                if (playerChoice == Choice.scissor)
                {
                    if (opponentChoice == Choice.paper)
                    {
                        PlayerWin();
                    }
                    else
                    {
                        PlayerLose();
                    }
                }

            }
        }

        void PlayerWin()
        {
            jokenpoContainer.SetTrigger("player_win");
            canNext = true;
            playerHasWin = true;
        }

        void PlayerLose()
        {
            jokenpoContainer.SetTrigger("player_lose");
            canNext = true;
            playerHasWin = false;
        }

        void Draw()
        {
            playerHasWin = false;
            playerHasChoose = opponentHasChoose = false;
        }

        Sprite GetArt(Choice choice)
        {
            switch (choice)
            {
                case Choice.rock:
                    return rockSprite;
                case Choice.paper:
                    return paperSprite;
                case Choice.scissor:
                    return scissorSprite;
                default:
                    return rockSprite;
            }
        }



        [ContextMenu("Start Jokenpo")]
        public void StartJokenpo()
        {
            jokenpoContainer.SetTrigger("start_jokenpo");
            playerHasWin = playerHasChoose = opponentHasChoose = canNext = finished = false;
            running = true;

        }
    }

}
