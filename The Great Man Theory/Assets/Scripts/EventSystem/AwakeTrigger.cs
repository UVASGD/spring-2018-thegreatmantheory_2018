using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTrigger : EventTrigger {
    
    void OnAwake() {
        strEvent.Invoke("");
    }
}
