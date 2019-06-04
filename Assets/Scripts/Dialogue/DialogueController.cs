using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    static DialogueController _instance;
    public static DialogueController Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<DialogueController>();
            }
            return _instance;
        }
    }

    public GameObject typewriterPrefab;

    public Transform canvas;

    public  Camera cam;

    public Sentence[] sentences;

    bool running;

    int currentSentence;

    Typewriter currentTypewriter;


    bool next;

    Sentence sentence;

    void Update()
    {
        if(running){
            if(currentTypewriter == null ){
                if(currentSentence < sentences.Length){
                    currentTypewriter = Instantiate(typewriterPrefab, canvas).GetComponent<Typewriter>();
                    sentence = sentences[currentSentence++];
                    
                    currentTypewriter.Setup(sentence.text, sentence.Speed, sentence.MaxPitch, sentence.MinPitch, sentence.Voice);
                }
                else{
                    running = false;
                    SystemsController.RunningDialogue(running);
                }
            }
            else{

                if( sentence.origin != null){
                        currentTypewriter.transform.position = cam.WorldToScreenPoint(sentence.origin.position);
                }
                else{
                    // system message
                }
                if(currentTypewriter.Done){
                    if(Input.GetMouseButtonDown(0)){
                        Destroy(currentTypewriter.gameObject);
                        currentTypewriter = null;
                    }
                }
                else{
                    if(Input.GetMouseButtonDown(0)){
                        currentTypewriter.JumpToEnd();
                    }
                }
            }
        }
    }

    public void StartDialogue(Sentence[] sentences){
        this.sentences = sentences;
        currentSentence = 0;
        running = true;
        SystemsController.RunningDialogue(running);
    }
}
