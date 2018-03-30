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
    protected bool hasHappened = false;
}
