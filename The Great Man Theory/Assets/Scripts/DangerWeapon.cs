using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerWeapon : MonoBehaviour {

    Collider2D trigger;
    Body thisBody;
    public GameObject holder;

    private void Start() {
        if (!thisBody) {
            thisBody = holder.GetComponentInChildren<Body>();
        }
        trigger = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Check height as well, bucko
        if (collision.transform.parent.gameObject != holder.gameObject) {
            if (collision.CompareTag("Body")) {
                return;
            }
            else if (collision.CompareTag("Weapon")) {
                Weapon weapon = collision.GetComponent<Weapon>();
                if (weapon.ThisBody.team == thisBody.team) {
                    return;
                }
            }
        }
        Physics2D.IgnoreCollision(collision, trigger);
    }
}
