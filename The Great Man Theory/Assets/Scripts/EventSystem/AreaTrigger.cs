using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : EventTrigger {

    void OnTriggerEnter2D(Collider2D other) {
        strEvent.Invoke("");
    }
}
