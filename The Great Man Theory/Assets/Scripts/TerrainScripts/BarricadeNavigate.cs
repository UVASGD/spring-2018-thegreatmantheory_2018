using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeNavigate : MonoBehaviour {

    Collider2D coll;

    private void Start() {
        coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        if (collider.CompareTag("Player")) {
            Physics2D.IgnoreCollision(coll, collider);
        }

        if (collider.CompareTag("Body"))
            if (collider.GetComponent<BasicBot>()) {
                BasicBot bot = collider.GetComponent<BasicBot>();
                int direction = (bot.squad) ? bot.squad.direction : 1;
                bot.Command(new Command(new Sequencer("Move Sequence", new List<Node>() {
                    new MoveLeaf(bot, transform.position + new Vector3(10 * Random.Range(-1, 1), 5*(-direction), 0)) }), 
                3), 
                2);
            }
            
    }
}
