using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  NodeState {
	Failure,
	Success,
	Running
}

public class BehaviorTree {

	
}

public abstract class Node {
	protected string name = "node";

	public Node(string _name = "Default Node") {
		name = _name;
	}

	public abstract NodeState GetState();
}

/*
public abstract class Leaf : Node {
	//Not really enough shared behaviors for a class??
	public abstract void Dewit ();
}
*/

public class Selector : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Selector(List<Node> _children, string _name = "Selector") : base(_name) {
		children = _children;
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

	public RandomSelector(List<Node> _children, string _name = "Random Selector") : base(_name) {
		children = _children;
		currentNodeIndex = Random.Range(0, children.Count);
	}

	public override NodeState GetState() {
		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
				return NodeState.Running;
			} else if (childState == NodeState.Success) {
				currentNodeIndex = Random.Range(0, children.Count);
				return NodeState.Success;
			}
		}
		currentNodeIndex = Random.Range (0, children.Count);
		return NodeState.Failure;
	}
}

public class Sequencer : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Sequencer(List<Node> _children, string _name = "Selector") : base(_name) {
		children = _children;
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