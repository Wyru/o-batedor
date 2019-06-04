using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public AudioSource audioSource;
    public AudioClip typeSound;

    [SerializeField, Range(0,2)] float maxPitch = 1;
    [SerializeField, Range(0,2)] float minPitch = 1;

    [SerializeField, Range(0f, 1f)] float timeBtwLetters = 0.1f;
    bool done;

    public bool Done{
        get{
            return done;
        }
    }

    [SerializeField, TextArea] string textToType = "Alô amor, tô te ligando de um orelhão";

    string textTyped = "";

    float timer = 0;

    int index = 0;

    string lastWordBuffer;

    bool canStart;

    bool jumpToEnd;

    Animator m_Animator;

    private void Start() {
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (!done && canStart)
        {
            if (index < textToType.Length)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (text.isTextOverflowing)
                    {
                        textTyped = lastWordBuffer;
                    }

                    if (textToType[index] == ' ')
                    {
                        lastWordBuffer = "";
                    }
                    else
                    {
                        lastWordBuffer += textToType[index];
                        
                        if (audioSource != null && typeSound != null && !jumpToEnd)
                        {
                            audioSource.pitch = Random.Range(minPitch, maxPitch);
                            audioSource.PlayOneShot(typeSound);
                        }
                    }

                    textTyped += textToType[index];
                    text.SetText(textTyped);
                    if(!jumpToEnd)
                        timer = timeBtwLetters;
                    index++;
                }
            }
            else
            {
                done = true;
                m_Animator.SetBool("Done", done);
            }
        }
    }

    public void Setup(string textToType, float timeBtwLetters, float maxPitch, float minPitch, AudioClip voice){
        this.textToType = textToType;
        this.timeBtwLetters = timeBtwLetters;
        this.maxPitch = maxPitch;
        this.minPitch = minPitch;
        this.typeSound = voice;
        canStart = true;
    }


    public void JumpToEnd(){
        this.timeBtwLetters = 0;
        jumpToEnd = true;
    }
}
