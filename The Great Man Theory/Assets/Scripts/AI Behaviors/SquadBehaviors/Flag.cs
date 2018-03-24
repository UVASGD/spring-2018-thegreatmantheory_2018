using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
    public BasicBot carrier;
    public ArmySquad squad;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !squad.enemies.Contains(collider.transform)) {
            if (collider.GetComponent<Body>().team != carrier.body.team) {
                squad.enemies.Add(collider.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        squad.enemies.Remove(collider.transform);
        //If it's a friendly, tell em to regroup
    }
}
