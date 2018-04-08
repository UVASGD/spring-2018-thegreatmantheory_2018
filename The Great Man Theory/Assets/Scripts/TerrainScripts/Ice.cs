using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {

    void OnTriggerStay2D(Collider2D collider) {

        if (collider.CompareTag("Body")) {
            Rigidbody2D body = collider.attachedRigidbody;
            body.AddForce(body.velocity.normalized * (100), ForceMode2D.Impulse);
        }
    }
}
