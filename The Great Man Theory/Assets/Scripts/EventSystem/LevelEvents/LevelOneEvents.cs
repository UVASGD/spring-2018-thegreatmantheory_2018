using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEvents : EventManager {

    public Squad boundingEnemies;
    public Transform playerTransform;

	public IEnumerator BoundingEnemiesTarget() {
        Debug.Log("We gonna getchya");

        List<Node> getimChildren = new List<Node> {
            new IntervalGate(100f),
            new MoveTargetCommand(boundingEnemies, 0, 100f, playerTransform)
        };

        Node getim = new Sequencer("Attack Dude", getimChildren);
        Command attackTarget = new Command(getim, 1f);

        Debug.Log("boundingEnemies: " + boundingEnemies.ToString());
        Debug.Log("command: " + attackTarget.ToString());
        Debug.Log("tree:" + boundingEnemies.maintree.ToString());

        boundingEnemies.maintree.insertAtPriority(attackTarget, 0);

        yield return null;
    }
}
