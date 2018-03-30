using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMAND NODES
/*
public class CommandNode : Node {
    protected Squad squad;
    protected int priority;
    protected float timeLeft;

    public CommandNode(Squad _squad, int _priority, float _timeLeft) {
        squad = _squad;
        priority = _priority;
        timeLeft = _timeLeft;
    }

    public override NodeState GetState() {
        return NodeState.Failure;
    }
}

public class MoveCommand : CommandNode {

    Vector2 target;

    public MoveCommand(Squad _squad, int _priority, float _timeLeft, Vector2 _target) : base(_squad, _priority, _timeLeft) {
        target = _target;
    }

    public override NodeState GetState() {
        foreach (BasicBot b in squad.minions) {
            squad.Command(b, new Command(
                    new Sequencer("Move", new List<Node>() {
                        new Gate(delegate () {
                            if (Vector2.Distance(target, b.transform.position) > b.squad.SquadRadius) {
                                return NodeState.Success;
                            }
                            return NodeState.Failure;
                        }),
                        new MoveLeaf(b, target)
                        }),
                    timeLeft
                    ), priority);
        }
        return NodeState.Success;
    }
}

public class MoveTargetCommand : CommandNode {

    Transform target;

    public MoveTargetCommand(Squad _squad, int _priority, float _timeLeft, Transform _target) : base(_squad, _priority, _timeLeft) {
        target = _target;
    }

    public override NodeState GetState() {
        foreach (BasicBot b in squad.minions) {
            squad.Command(b, new Command(
                    new Sequencer("Move", new List<Node>() {
                        new Gate(delegate () {
                            if (Vector2.Distance(target.position, b.transform.position) > b.squad.SquadRadius) {
                                return NodeState.Success;
                            }
                            return NodeState.Failure;
                        }),
                        new MoveTargetLeaf(b, target)
                        }),
                    timeLeft
                    ), priority);
        }
        return NodeState.Success;
    }
}
*/