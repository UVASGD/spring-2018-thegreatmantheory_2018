using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class strEvent : UnityEvent<string> {
}

public class OldFollowPointer : MonoBehaviour {

    public strEvent e;

}
