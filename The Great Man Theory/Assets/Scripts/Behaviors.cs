using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : ICommandable {

    protected Body body;
    protected Mover mover;
    protected Leaf command;
    protected int currentPriority = 0;
    //squad

    protected List<Leaf> commandList; //Maybe change this to node, but probably not


    public BehaviorTree(Body _body, Mover _mover) {
        body = _body;
        mover = _mover;
    }

    protected Node rootNode;

    public void Traverse() {
        if (command != null) {
            if (command.GetState() == NodeState.Running)
                currentPriority = command.currPriority;
            else
                command = null;
        }
        else {
            rootNode.GetState();
            currentPriority = rootNode.currPriority;
        }
    }

    public bool SetCommand(LeafKey key, int priority) {
        if (priority > currentPriority)
            foreach (Leaf l in commandList)
                if (key == l.Key) {
                    command = l;
                    return true;
                }
        return false;
    }
}

public class MeleeBehavior : BehaviorTree {
    public MeleeBehavior(Body _body, Mover _mover, int fleeDir, float mainTimer, int mainDist, int mainLeeway, float charTimer, float wigTimer, float maxWig, float wigDist) 
        : base(_body, _mover) {

        MedicLeaf medic = new MedicLeaf(mover);
        FleeLeaf flee = new FleeLeaf(mover, fleeDir);

        MaintainLeaf maintain = new MaintainLeaf(mover, mainTimer, mainDist, mainLeeway);
        ChargeLeaf charge = new ChargeLeaf(mover, charTimer);
        WiggleLeaf wiggle = new WiggleLeaf(mover, wigTimer, maxWig, wigDist);

        commandList = new List<Leaf>() { medic, flee, maintain, charge, wiggle };

        List<Node> WoundedList = new List<Node>() {
            medic, //timer, medic target from squad -- medic target == null
            flee //end zone from squad -- n/a
        };

        List<Node> PrepareList = new List<Node>() {
            maintain, //timer, distance from go, target from go -- n/a
            charge //target from go -- n/a
        };

        List<Node> FightList = new List<Node>() {
            wiggle, //target from go -- n/a
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
