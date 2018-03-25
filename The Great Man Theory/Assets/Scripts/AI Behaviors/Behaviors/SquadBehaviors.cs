using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadHoldTree : DefaultTree {
    Vector2 pos;
    float orderTime = Random.Range(10, 15);

    public SquadHoldTree(ArmySquad squad) {
        pos = squad.flag.transform.position;

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Hold", new List<Node>() {
                    new IntervalGate(orderTime),
                    //new CommandNode(squad, 
                        //new MoveCommand(pos, orderTime), 3)
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

public class SquadAdvanceTree : DefaultTree {

    public SquadAdvanceTree(ArmySquad squad) {
        float moveInterval = Random.Range(20, 30);

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Advance", new List<Node>() {
                    new IntervalGate(moveInterval),
                    //new CommandNode(squad, 
                        //new MoveCommand(new Vector2(0, squad.direction*1000), moveInterval), 3)
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}