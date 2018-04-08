using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaKill : EventTrigger {

    // public Collider2D[] triggers;

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Is Happening");

        GameObject otherObj = other.gameObject;
        Body otherBody = otherObj.GetComponent<Body>();

        if (otherBody) {
            Destroy(otherObj);
            strEvent.Invoke("");
            hasHappened = true;
        }

    }
}
