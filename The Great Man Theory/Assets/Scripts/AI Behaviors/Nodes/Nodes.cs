using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  NodeState {
	Failure,
	Success,
	Running
}

public delegate NodeState NodeDel();


public abstract class Node {
    protected string name = "node";

	public bool expired = false;

    public Node(string _name = "Default Node") {
        name = _name;
}

    public abstract NodeState GetState();
}