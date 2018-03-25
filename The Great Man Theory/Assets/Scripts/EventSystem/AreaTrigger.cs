using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : EventTrigger {

    public Collider2D[] triggers;

    void OnTriggerEnter2D(Collider2D other) {
        foreach (Collider2D collider in triggers) {
            if (other == collider) {
                strEvent.Invoke("");
                break;
            }
        }
    }
}
