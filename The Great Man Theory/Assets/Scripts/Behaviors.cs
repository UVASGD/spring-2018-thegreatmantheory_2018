using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree {

    protected Body body;
    protected Mover mover;
    //squad

    public BehaviorTree(Body _body, Mover _mover) {
        body = _body;
        mover = _mover;
    }

    protected Node rootNode;

    public void Traverse() {
        rootNode.GetState();
    }
}

public class MeleeBehavior : BehaviorTree {
    public MeleeBehavior(Body _body, Mover _mover, int fleeDir, float mainTimer, int mainDist, int mainLeeway, float charTimer, float wigTimer, float maxWig, float wigDist) 
        : base(_body, _mover) {

        List<Node> WoundedList = new List<Node>() {
            new MedicLeaf(mover, body), //timer, medic target from squad -- medic target == null
            new FleeLeaf(mover, fleeDir) //end zone from squad -- n/a
        };

        List<Node> PrepareList = new List<Node>() {
            new MaintainLeaf(mover, mainTimer, mainDist, mainLeeway), //timer, distance from go, target from go -- n/a
            new ChargeLeaf(mover, charTimer) //target from go -- n/a
        };

        List<Node> FightList = new List<Node>() {
            new WiggleLeaf(mover, wigTimer, maxWig, wigDist), //target from go -- n/a
            new Sequencer("Prepare", _children:PrepareList) // n/a -- n/a
        };

        List<Node> RootList = new List<Node>() {
            new RandomSelector("Wounded", 
                delegate() { return (body.Wounded()) ? NodeState.Running : NodeState.Failure; },
                WoundedList), // n/a -- health < (maxHealth * 0.1)
            new RandomSelector("Fight", 
                delegate() { return (mover.targetObj) ? NodeState.Running : NodeState.Failure; }, 
                FightList) // n/a -- (target) && squad.Fight
        };

        rootNode = new Selector("RootNode", _children:RootList);
    }
}
