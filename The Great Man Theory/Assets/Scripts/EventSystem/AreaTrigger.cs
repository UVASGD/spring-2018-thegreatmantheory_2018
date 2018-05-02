using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : EventTrigger {

    public Collider2D[] triggers;

    public bool acceptAll = false;
    public bool justPlayer = false;

    protected override void Update() {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Is Happening");

        if (justPlayer && other.gameObject.transform.parent && other.gameObject.transform.parent.gameObject.CompareTag("Player")) {
            triggered = true;
        }

        if (acceptAll && !(onceOnly && triggered)) {
            // strEvent.Invoke("");
            triggered = true;
            return;
        }

        foreach (Collider2D collider in triggers) {
            if (other == collider && !(onceOnly && triggered)) {
                // strEvent.Invoke("");
                triggered = true;
                // hasHappened = true;
                break;
            }
        }

    }
}
