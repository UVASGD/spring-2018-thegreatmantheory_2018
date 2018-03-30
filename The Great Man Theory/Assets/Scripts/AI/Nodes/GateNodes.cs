using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Node {
    NodeDel del;
    public string name;

    public Gate(NodeDel _del, string _gate = "gate") {
        del = _del;
        name = _gate;
    }

    public override NodeState GetState() {
        Debug.Log(name);
        return del();
    }
}

public class IntervalGate : Node {
    float interval;
    float time;
    bool fail;

    public IntervalGate(float _interval, bool _fail = false) {
        interval = _interval;
        time = interval;
        fail = _fail;
    }

    public override NodeState GetState() {
        time -= Time.deltaTime;
        if (time <= 0) {
            time = interval;
            return (fail) ? NodeState.Failure : NodeState.Success;
        }
        return (fail) ? NodeState.Success : NodeState.Failure;
    }
}

public class OneShotGate : Node {
    bool fail;
    bool done = false;

    public OneShotGate(bool _fail = false) {
        fail = _fail;
    }

    public override NodeState GetState() {
        if (!done) {
            done = true;
            return (fail) ? NodeState.Failure : NodeState.Success;
        }
        return (fail) ? NodeState.Success : NodeState.Failure;
    }
}
