﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    public Sentence[] sentences;


    public UnityEvent OnStartDialogue;
    public UnityEvent OnEndDialogue;
    
    public void Play(){
        DialogueController.Instance.StartDialogue(this);
    }
    
}

[System.Serializable]
public class Sentence{

    [TextArea]
    public string text;

    public CharacterVoice characterVoice;

    public float speed = -1;

    public Transform origin;

    public float Speed{
        get{
            if(speed < 0)
                return 1-characterVoice.speed;
            return 1-speed;
        }
    }
    public float MaxPitch{
        get{
            return characterVoice.maxPitch;
        }
    }

    public float MinPitch{
        get{
            return characterVoice.minPitch;
        }
    }

    public AudioClip Voice{
        get{
            return characterVoice.voice;
        }
    }
    
}
