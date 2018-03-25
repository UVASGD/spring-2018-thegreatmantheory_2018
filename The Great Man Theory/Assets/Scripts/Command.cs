using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {

    public Node subtree;
    public float timeLeft;

    public Command(float _timeLeft) {
        timeLeft = _timeLeft;
    }
}

public class OpenCommand : Command {
    public OpenCommand(Node _subtree, float _timeLeft) : base(_timeLeft) {
        subtree = _subtree;
    }
}

public class MoveCommand : Command {

    public MoveCommand(Vector2 target, float _timeLeft) : base(_timeLeft) {
        subtree = new Sequencer("Move", new List<Node>() {

        });
    }
}

public class MoveTargetCommand {

}