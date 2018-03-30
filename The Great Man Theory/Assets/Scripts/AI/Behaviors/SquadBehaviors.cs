using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class SquadHoldTree : DefaultTree {
    Vector2 pos;
    float interval = Random.Range(5, 10);

    public SquadHoldTree(Squad squad) {
        pos = squad.flag.transform.position;

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Hold", new List<Node>() {
                    new IntervalGate(interval),
                    new MoveCommand(squad, 3, interval, pos)
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

public class SquadAdvanceTree : DefaultTree {

    public SquadAdvanceTree(Squad squad) {
        float interval = Random.Range(5, 10);

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Advance", new List<Node>() {
                    new IntervalGate(interval),
                    new MoveCommand(squad, 3, interval, new Vector2(0, squad.direction*1000))
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

public class SquadTargetTree : DefaultTree {

    public SquadTargetTree(Squad squad) {
        float interval = Random.Range(20, 30);

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Advance", new List<Node>() {
                    new IntervalGate(interval),
                    
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}
*/