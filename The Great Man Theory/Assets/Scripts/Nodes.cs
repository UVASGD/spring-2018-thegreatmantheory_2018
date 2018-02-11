using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  NodeState {
	Failure,
	Success,
	Running
}

public abstract class Node {
    protected string name = "node";

    public Node(string _name = "Default Node") {
        name = _name;
    }

    public abstract NodeState GetState();
}

public delegate NodeState LeafDel();

public class Leaf : Node {
    LeafDel leafDel;

    public Leaf(string _name = "Selector", LeafDel _leafDel = null) : base(_name) {
        leafDel = _leafDel;
    }

    public override NodeState GetState() {
        return leafDel();
    }
}

public class Selector : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Selector(string _name = "Selector", List<Node> _children = null) : base(_name) {
		children =  (_children == null) ? new List<Node>() : _children;
		currentNodeIndex = 0;
	}

	public override NodeState GetState() {
		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
				return NodeState.Running;
			} else if (childState == NodeState.Success) {
				currentNodeIndex = 0;
				return NodeState.Success;
			}
		}
		currentNodeIndex = 0;
		return NodeState.Failure;
	}
}

public class RandomSelector : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public RandomSelector(string _name = "Random Selector", List<Node> _children = null) : base(_name) {
        children = (_children == null) ? new List<Node>() : _children;
        currentNodeIndex = UnityEngine.Random.Range(0, children.Count);
	}

	public override NodeState GetState() {
		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
				return NodeState.Running;
			} else if (childState == NodeState.Success) {
				currentNodeIndex = UnityEngine.Random.Range(0, children.Count);
				return NodeState.Success;
			}
		}
		currentNodeIndex = UnityEngine.Random.Range (0, children.Count);
		return NodeState.Failure;
	}
}

public class Sequencer : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Sequencer(string _name = "Sequencer", List<Node> _children = null) : base(_name) {
        children = (_children == null) ? new List<Node>() : _children;
        currentNodeIndex = 0;
	}

	public override NodeState GetState() {
		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
				return NodeState.Running;
			} else if (childState == NodeState.Failure) {
				currentNodeIndex = 0;
				return NodeState.Failure;
			}
		}
		currentNodeIndex = 0;
		return NodeState.Success;
	}
}