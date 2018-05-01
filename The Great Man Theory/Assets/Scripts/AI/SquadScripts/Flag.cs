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
        zone.radius = Mathf.Clamp(zone.radius, 20, 100);
    }

    public void Update() {
        if (carrier)
            transform.position = carrier.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !squad.enemies.Contains(collider.gameObject)) {
            Body colliderBody = collider.GetComponent<Body>();
            if (colliderBody != null && colliderBody.team != carrier.body.team && colliderBody.untargetable == false) {
                if (!squad.enemies.Contains(collider.gameObject)) {
                    squad.enemies.Add(collider.gameObject);
                    if (collider.GetComponent<BasicBot>()) {
                        if (collider.GetComponent<BasicBot>().squad) {
                            squad.AttackSquad(collider.GetComponent<BasicBot>().squad);
                        }
                    }
                    else {
                        squad.AttackIntruder(collider.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        squad.enemies.Remove(collider.gameObject);
        if (collider.CompareTag("Body")) {
            Body colliderBody = collider.GetComponent<Body>();
            if (colliderBody.team == squad.team && colliderBody.untargetable == false)
                if (collider.GetComponent<BasicBot>() != null) {
                    BasicBot bot = collider.GetComponent<BasicBot>();
                    squad.Command = delegate () { squad.TargetCommand(squad.flag.gameObject, bot); };
                }
        }
    }
}
