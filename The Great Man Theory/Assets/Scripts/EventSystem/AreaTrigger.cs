using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : EventTrigger {

    public Collider2D[] triggers;

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Is Happening");
        foreach (Collider2D collider in triggers) {
            if (other == collider && !(onceOnly && hasHappened)) {
                strEvent.Invoke("");
                hasHappened = true;
                break;
            }
        }

    }
}
