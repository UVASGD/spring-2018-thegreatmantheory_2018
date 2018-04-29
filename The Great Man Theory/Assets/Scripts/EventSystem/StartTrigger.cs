using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : EventTrigger {

    protected override void Update() {
        base.Update();
    }

    void Start() {
        triggered = true;
        // strEvent.Invoke("");
    }
}
