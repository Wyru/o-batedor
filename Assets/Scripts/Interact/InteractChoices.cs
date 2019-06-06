using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractChoices : MonoBehaviour
{
    [System.Serializable]
    public class Choice{

        public string text;
        public UnityEvent action;

        public void Select(){
            if(action != null){
                action.Invoke();
            }
        }
    }
    
    public Choice[] choices;

    public void ShowOptions(){
        FindObjectOfType<InteractChoicesController>().ShowChoices(choices);
    }

    public void TestA(){
        Debug.Log("Test A");
    }

    public void TestB(){
        Debug.Log("Test B");
    }

    public void TestC(){
        Debug.Log("Test C");
    }
}


