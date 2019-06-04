using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    public Camera cam;
    Animator m_Animator;

    PlayerMovement playerMovement;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        BillboadEffect();
        UpdateAnimation();
    }



    void BillboadEffect()
    {
        transform.LookAt(cam.transform);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }


    void UpdateAnimation()
    {
        if (playerMovement.State == PlayerMovement.PlayerState.Idle)
        {
            SetWalkBools(false, false, false, false);
            m_Animator.SetBool("Idle", true);

        }
        else
        {
            m_Animator.SetBool("Idle", false);
            if (playerMovement.State == PlayerMovement.PlayerState.MovingLeft)
                SetWalkBools(true, false, false, false);
            else if (playerMovement.State == PlayerMovement.PlayerState.MovingRight)
                SetWalkBools(false, true, false, false);
            else if (playerMovement.State == PlayerMovement.PlayerState.MovingUp)
                SetWalkBools(false, false, true, false);
            else if (playerMovement.State == PlayerMovement.PlayerState.MovingDown)
                SetWalkBools(false, false, false, true);
        }
    }


    void SetWalkBools(bool left, bool right, bool up, bool down)
    {
        m_Animator.SetBool("MovingLeft", left);
        m_Animator.SetBool("MovingRight", right);
        m_Animator.SetBool("MovingUp", up);
        m_Animator.SetBool("MovingDown", down);
    }
}
