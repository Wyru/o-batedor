using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public AudioClip submitSound;
    public UnityEvent OnPressed;
    public void Action()
    {
        if(OnPressed != null){
            OnPressed.Invoke();
        }
    }
}
