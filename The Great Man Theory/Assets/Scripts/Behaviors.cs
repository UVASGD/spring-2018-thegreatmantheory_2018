using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree {

    protected Node rootNode;

    public void Traverse() {
        rootNode.GetState();
    }
}

public class MeleeBehavior : BehaviorTree {
    public MeleeBehavior() {
        List<Node> WoundedList = new List<Node>() {
            new Leaf("Medic", delegate() { return NodeState.Failure; }),
            new Leaf("Flee", delegate() { return NodeState.Failure; })
        };

        List<Node> PrepareList = new List<Node>() {
            new Leaf("Maintain", delegate () { return NodeState.Failure; }),
            new Leaf("Charge", delegate() { return NodeState.Failure; })
        };

        List<Node> FightList = new List<Node>() {
            new Leaf("Wiggle", delegate() { return NodeState.Failure; }),
            new Sequencer("Prepare", PrepareList)
        };

        List<Node> RootList = new List<Node>() {
            new RandomSelector("Wounded", WoundedList),
            new RandomSelector("Fight", FightList)
        };

        rootNode = new Sequencer("RootNode", RootList);
    }
}
