using UnityEngine;
using UnityEngine.Events;

public class AnimationEventOtherObject : MonoBehaviour {
    public UnityEvent events;

    public void Action(){
        if(events != null){
            events.Invoke();
        }
    }
}