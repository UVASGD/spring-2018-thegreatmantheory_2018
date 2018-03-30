using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
    public BasicBot carrier;
    public Squad squad;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !squad.enemies.Contains(collider.transform)) {
            if (collider.GetComponent<Body>().team != carrier.body.team) {
                squad.enemies.Add(collider.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        squad.enemies.Remove(collider.transform);
        if (collider.CompareTag("Body")) {
            if (collider.GetComponent<Body>().team == squad.team) {
                /*
                squad.Command(new Command(
                    new Sequencer("Regroup", new List<Node>() {
                        new OneShotGate(),
                        new MoveTargetCommand(squad, 3, 5, transform)
                    }), 5),
                1);
                */
            }
        }
    }
}
