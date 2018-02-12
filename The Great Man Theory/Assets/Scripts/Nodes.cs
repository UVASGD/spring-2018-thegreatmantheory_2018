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

	protected List<int> cumulativeFrequencies;
	protected int frequencySum;

	/* Some details on the parameter @frequencies
	 * *
	 * * Should be same length as @children, or else a default frequency list will be used
	 * 
	 * *
	 * * Represents the relative frequency of each node. The probability of node [i] being chosen is _frequencies[i] / sum(frequencies)
	 * 
	 * * Example, for a node with 4 children, the list [1, 3, 4, 2] would have a 1/10 chance for node 0, 3/10 for node 1, 4/10 for node 2, and 2/10 for node 3
	 * 
	 */
	public RandomSelector(string _name = "Random Selector", List<Node> _children = null, List<int> _frequencies = null) : base(_name) {
        children = (_children == null) ? new List<Node>() : _children;
		generateCumulativeFrequencies (_frequencies);
		currentNodeIndex = GetRandomIndex ();
	}

	private void generateCumulativeFrequencies(List<int> _frequencies) {
		cumulativeFrequencies = new List<int>();
		int sum = 0;

		//If param is witheld or invalid, generate a uniform distribution
		if (_frequencies == null || children.Count != _frequencies.Count) {
			for (int i = 1; i <= children.Count; i++) {
				cumulativeFrequencies.Add (i);
			}
			frequencySum = children.Count;
			return;
		}

		for (int i = 0; i < _frequencies.Count; i++) {
			sum += _frequencies [i];
			cumulativeFrequencies.Add (sum);
		}
		frequencySum = sum;
	}

	private int GetRandomIndex() {
		float point = UnityEngine.Random.value * frequencySum; //uniform between 0, frequencysum
		for (int i = 0; i < cumulativeFrequencies.Count; i++) { //Calculate P^-1 [point]
			if (point <= cumulativeFrequencies [i]) {
				return i;
			}
		}
		//Should never get here, but if point > all cumulative frequency values, it should be the last index.
		return cumulativeFrequencies.Count - 1;
	}

	public override NodeState GetState() {
		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
				return NodeState.Running;
			} else if (childState == NodeState.Success) {
				currentNodeIndex = GetRandomIndex();
				return NodeState.Success;
			}
		}
		currentNodeIndex = GetRandomIndex();
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