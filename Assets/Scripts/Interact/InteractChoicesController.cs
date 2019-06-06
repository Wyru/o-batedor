using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractChoicesController : MonoBehaviour
{
    public GameObject canvas;
    public GameObject choiceContainerPrefab;
    public GameObject choicePrefab;

    GameObject openedChoice;

    Animator choicesAnimator;



    public void ShowChoices(InteractChoices.Choice[] choices){
        SystemsController.RunningChoices(true);
        Transform t = Instantiate(choiceContainerPrefab, canvas.transform).transform;
        choicesAnimator = t.GetComponent<Animator>();
        openedChoice = t.gameObject;
        VerticalLayoutGroup verticalGroup = t.GetComponentInChildren<VerticalLayoutGroup>();
        Debug.Log(verticalGroup.gameObject.name);
        foreach (InteractChoices.Choice choice in choices)
        {
            Button button = Instantiate(choicePrefab, verticalGroup.transform).GetComponent<Button>();
            button.onClick.AddListener(()=> SelectChoice());
            button.onClick.AddListener(()=> HideChoices());
            button.onClick.AddListener(()=> choice.Select());

            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText(choice.text);
        }
    }

    public void SelectChoice(){
        // toca som, possíveis efeitos
    }

    public void HideChoices(){
        choicesAnimator.SetTrigger("Out");
        Destroy(openedChoice,2f);
        SystemsController.RunningChoices(false);
    }

    
    
}
