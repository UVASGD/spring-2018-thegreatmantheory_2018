using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTree : DefaultTree {
    public SwordTree(BasicBot bot) {
        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {
                new Sequencer("wounded", new List<Node>() {
                    new Gate(delegate () {
                        return NodeState.Failure;
                    }),
                    new RandomSelector("Wounded Action", new List<Node>() {
                        new FleeLeaf(bot),
                        new MedicLeaf(bot)
                    })
                })
            }),
            new Selector("priority 2", new List<Node>() {
                new Gate(delegate() {
                    return (!bot.attackTarget || Vector2.Distance(bot.transform.position, bot.attackTarget.position) > 10)
                    ? NodeState.Success: NodeState.Failure;
                }),
                new RandomSelector("Fight", new List<Node>() {
                    new WiggleLeaf(bot),
                    new Sequencer("Prepare", new List<Node>() {
                        new MaintainLeaf(bot),
                        new ChargeLeaf(bot)
                    })
                }, new List<int>() {
                    1, 0
                })
            }),
            new Selector("priority 3", new List<Node>() {}),
            new Selector("priority 4", new List<Node>() {
				//Idle node here
			})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}
