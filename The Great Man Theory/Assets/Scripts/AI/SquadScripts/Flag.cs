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

    public void Update() {
        transform.position = carrier.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !squad.enemies.Contains(collider.transform)) {
            Body colliderBody = collider.GetComponent<Body>();
            Debug.Log("collider body" + colliderBody);
            if (colliderBody != null && colliderBody.team != carrier.body.team) {
                squad.enemies.Add(collider.transform);
                squad.Command = delegate () { squad.AttackIntruder(collider.transform); };
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        squad.enemies.Remove(collider.transform);
        if (collider.CompareTag("Body"))
            if (collider.GetComponent<Body>().team == squad.team)
                if (collider.GetComponent<BasicBot>() != null) {
                    BasicBot bot = collider.GetComponent<BasicBot>();
                    squad.Command = delegate () { squad.TargetCommand(squad.flag.transform, bot); };
                }
    }
}
