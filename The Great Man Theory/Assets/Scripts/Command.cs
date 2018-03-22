using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command {

    public Node subtree;
    float timeLeft;

    public Command(Node _subtree, float _timeLeft) {
        subtree = _subtree;
        timeLeft = _timeLeft;
    }
}
