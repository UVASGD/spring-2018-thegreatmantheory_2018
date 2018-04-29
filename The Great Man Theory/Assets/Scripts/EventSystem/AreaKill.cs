using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaKill : EventTrigger {

    // public Collider2D[] triggers;

    protected override void Update() {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Is Happening");

        GameObject otherObj = other.gameObject;
        Body otherBody = otherObj.GetComponent<Body>();

        if (otherBody && !(onceOnly && triggered)) {
            Destroy(otherObj.transform.parent.gameObject);
            // strEvent.Invoke("");
            triggered = true;
        }

    }
}
