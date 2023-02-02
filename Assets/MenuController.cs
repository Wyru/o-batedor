using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    
    public Animator animator;
    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        animator.SetBool("credits", true);
        animator.SetBool("howToPlay", false);
    }

    public void HowToPlay()
    {
        animator.SetBool("credits", false);
        animator.SetBool("howToPlay", true);
    }

    public void Exit()
    {
        Application.Quit();
    }


}
