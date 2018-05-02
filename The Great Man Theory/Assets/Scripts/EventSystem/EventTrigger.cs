using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StrEvent : UnityEvent<string> { }

[System.Serializable]
public class EventTrigger : MonoBehaviour {

    public StrEvent strEvent;

    public bool onceOnly = false;

    protected bool triggered = false;
    bool done = false;

    public float delay = 0f;
    float delayCounter = 0f;

    protected virtual void Update() {
        if (triggered && delayCounter < delay) {
            delayCounter += Time.deltaTime;
        }
        else if (!done && triggered && delayCounter >= delay && GameManager.state != GameState.SceneTransition) {
            strEvent.Invoke("");
            done = true;
        }
    }
}
