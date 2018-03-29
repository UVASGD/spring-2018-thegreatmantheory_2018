using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEvents : EventManager {

    public Squad boundingEnemies;
    public Transform playerTransform;

	public IEnumerator BoundingEnemiesTarget() {

        List<Node> getimChildren = new List<Node> {
            new IntervalGate(100f),
            new MoveTargetCommand(boundingEnemies, 0, 100f, playerTransform)
        };

        Node getim = new Sequencer("Attack Dude", getimChildren);
        Command attackTarget = new Command(getim, 1f);

        boundingEnemies.maintree.insertAtPriority(attackTarget, 0);

        yield return null;
    }
}
