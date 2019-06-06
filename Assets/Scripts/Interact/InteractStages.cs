using UnityEngine;
using UnityEngine.Events;
public class InteractStages : MonoBehaviour
{
    [SerializeField]
    int currentIndex = 0;
    public Stage[] stages;

    public void PlayEvents()
    {
        stages[currentIndex].events.Invoke();

        switch (stages[currentIndex].action)
        {
            case Stage.Action.nextStage:
                currentIndex = Mathf.Min(currentIndex + 1, stages.Length);
                break;
            case Stage.Action.previusStage:
                currentIndex = Mathf.Max(currentIndex - 1, 0);
                break;
            case Stage.Action.customStage:
                if (stages[currentIndex].customStage >= 0 && stages[currentIndex].customStage < stages.Length)
                    currentIndex = stages[currentIndex].customStage;
                break;
        }
    }

    public void SetIndex(int i)
    {
        if (i >= 0 && i < stages.Length)
        {
            currentIndex = i;
        }
    }

}


[System.Serializable]

public class Stage
{
    public UnityEvent events;


    public enum Action
    {
        nothing,
        nextStage,
        previusStage,
        customStage
    }

    public Action action;

    public int customStage;
}
