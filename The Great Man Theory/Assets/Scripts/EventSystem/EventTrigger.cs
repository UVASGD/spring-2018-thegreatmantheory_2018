using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {

    [System.Serializable]
    public class StrEvent : UnityEvent<string> { }

    public StrEvent strEvent;
}
