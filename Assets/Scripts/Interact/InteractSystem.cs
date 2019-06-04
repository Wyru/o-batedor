using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    public static bool running = true;
    public Camera cam;

    public Texture2D cursorNormal;
    public Texture2D cursorHand;

    public CursorMode cursorMode = CursorMode.Auto;

    public Vector2 hotSpot = Vector2.zero;

    public InteractiveObject selected;
    InteractiveObject hover;


    static InteractSystem _instance;

    public static InteractSystem Instance{
        get{
            if(_instance == null){
                _instance = FindObjectOfType<InteractSystem>();
            }

            return _instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateHover();
        if(running){
            UpdateInput();
        }
        UpdateMouseCursor();
    }

    void UpdateHover(){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            hover = hit.transform.GetComponent<InteractiveObject>();
            
        }
    }


    void UpdateInput(){
        if(Input.GetMouseButtonDown(0)){
            selected = hover;
            if(selected != null){
                selected.Click();
            }
        }
    }

    void UpdateMouseCursor(){
        if(hover != null){
            Cursor.SetCursor(cursorHand, hotSpot, cursorMode);
        }
        else{
            Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
        }
    }
}
