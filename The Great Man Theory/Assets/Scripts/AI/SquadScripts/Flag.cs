using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
    public BasicBot carrier;
    public Squad squad;
    CircleCollider2D zone;

    public void Setup() {
        zone = GetComponent<CircleCollider2D>();
        zone.radius = squad.SquadRadius;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !squad.enemies.Contains(collider.transform)) {
            if (collider.GetComponent<Body>().team != carrier.body.team) {
                squad.enemies.Add(collider.transform);
                squad.Command = delegate () { squad.AttackIntruder(collider.transform); };
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        squad.enemies.Remove(collider.transform);
        if (collider.CompareTag("Body"))
            if (collider.GetComponent<Body>().team == squad.team)
                if (collider.GetComponent<BasicBot>())
                    squad.Command = delegate () { squad.TargetCommand(squad.flag.transform, collider.GetComponent<BasicBot>()); };
    }
}
