using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  NodeState {
	Failure,
	Success,
	Running
}

public enum LeafKey {
    Maintain,
    Wiggle,
    Charge,
    Focus,
    Shoot,
    Repo,
    Medic,
    Flee,
	Move,
	Regroup
}

public delegate NodeState NodeDel();


public abstract class Node {
    protected string name = "node";
    protected NodeDel nodeDel;
    protected int priority;
    public int currPriority;

    public Node(string _name = "Default Node", NodeDel _nodeDel = null, int _priority = 0) {
        nodeDel = _nodeDel;
        name = _name;
        priority = _priority;
        currPriority = priority;
}

    public abstract NodeState GetState();
}



public class Leaf : Node {

    protected LeafKey key;
    public LeafKey Key { get { return key; } }

    public Leaf(string _name = "Leaf", int priority = 0) : base(_name, _priority: priority) {
    }

    public override NodeState GetState() {
        return NodeState.Running;
    }
}

public class MaintainLeaf : Leaf {
    bool started = false;
    float timerMax;
    float timer;
    Mover mover;
    Transform pos;
    Vector2 target;
    int prefDist;
    int leeway;

    public MaintainLeaf(Mover _mover, float _timerMax, int _prefDist, int _leeway, string _name = "Maintain", int _priority = 1) : base(_name, priority: _priority) {
        key = LeafKey.Maintain;
        timerMax = _timerMax;
        timer = timerMax;
        mover = _mover;
        pos = mover.transform;
        target = mover.Target;
        prefDist = _prefDist;
        leeway = _leeway;
    }

    public override NodeState GetState() {
        if (!started) {
            mover.Hold();
            started = true;
        }
        timer -= Time.deltaTime;
        target = mover.Target;
        target = ((Vector2)pos.position - target).normalized * prefDist;
        mover.SetTarget(target);
        if (timer <= 0) {
            timer = timerMax;
            started = false;
            mover.Hold(false);
            return (Mathf.Abs(prefDist - Vector2.Distance(pos.position, target)) < leeway) ? NodeState.Success : NodeState.Failure;
        }
        return NodeState.Running;
    }
}

public class WiggleLeaf : Leaf {
    bool started = false;
    float timerMax;
    float timer;
    float maxWig;
    float swingMax;
    float swingTimer;
    Mover mover;
    Vector2 target;
    float randoDist;


    public WiggleLeaf(Mover _mover, float _timerMax, float _maxWig, float _randoDist, string _name = "Wiggle", int _priority = 2) : base(_name, priority: _priority) {
        key = LeafKey.Wiggle;
        timerMax = _timerMax;
        timer = timerMax;
        mover = _mover;
        target = mover.Target;
        randoDist = _randoDist;
        maxWig = _maxWig;
        swingMax = UnityEngine.Random.Range(0.2f, _maxWig);
        swingTimer = swingMax;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            target = mover.Target + (UnityEngine.Random.insideUnitCircle * randoDist);
            mover.SetTarget(target);
            swingMax = UnityEngine.Random.Range(0.2f, maxWig);
            swingTimer = swingMax;
        }
        swingTimer -= Time.deltaTime;
        if (swingTimer <= 0) {
            swingTimer = swingMax;
            target = mover.Target + (UnityEngine.Random.insideUnitCircle * randoDist);
            mover.SetTarget(target);
        }
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = timerMax;
            started = false;
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}

public class ChargeLeaf : Leaf {
    bool started = false;
    float timerMax;
    float timer;
    Mover mover;

    public ChargeLeaf(Mover _mover, float _timerMax, string _name = "Charge", int _priority = 3) : base(_name, priority: _priority) {
        key = LeafKey.Charge;
        timerMax = _timerMax;
        timer = timerMax;
        mover = _mover;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            mover.SetTarget(mover.Target);
        }
        timer -= Time.deltaTime;
        mover.Dash();
        if (timer <= 0) {
            timer = timerMax;
            started = false;
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}


public class FocusLeaf : Leaf {
    bool started = false;
    Mover mover;
    Body body;
    float timerMax;
    float timer;

    public FocusLeaf(Mover _mover, Body _body, float _timerMax, string _name = "Focus", int _priority = 2) : base(_name, priority: _priority) {
        key = LeafKey.Focus;
        timerMax = _timerMax;
        timer = UnityEngine.Random.Range(0.4f, timerMax);
        mover = _mover;
        body = _body;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            Vector2 target = (mover.Target - body.weapon.pointer.ForcePoint).normalized;
            target = mover.transform.InverseTransformDirection(target);
            target = new Vector2(target.x + UnityEngine.Random.Range(-0.1f, 0.1f), target.y).normalized;
            mover.SetTarget(mover.transform.TransformDirection(target));
        }
        timer -= Time.deltaTime;
        mover.Brace();
        if (timer <= 0) {
            timer = UnityEngine.Random.Range(0.4f, timerMax);
            started = false;
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}

public class ShootLeaf : Leaf {
    bool fired = false;
    Body body;
    float timerMax;
    float timer;

    public ShootLeaf(Body _body, float _timerMax, string _name = "Shoot", int _priority = 5) : base(_name, priority: _priority) {
        key = LeafKey.Shoot;
        timerMax = _timerMax;
        timer = UnityEngine.Random.Range(0.4f, timerMax);
        body = _body;
    }

    public override NodeState GetState() {
        if (!fired) {
            fired = true;
            ((RangedWeapon)body.weapon).Trigger();
        }
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = UnityEngine.Random.Range(0.4f, timerMax);
            fired = false;
            return NodeState.Success;
        }
        return NodeState.Running;
    }
}


public class MedicLeaf : Leaf {
    Mover mover;
	Transform target;


	public MedicLeaf(Mover _mover, string _name = "Medic", int _priority = 5) : base(_name, priority: _priority) {
        key = LeafKey.Medic;
        mover = _mover;
		target = null;
	}

    public override NodeState GetState() {
		target = mover.GetCommander().FindMedic();
		if (target == null) {
			return NodeState.Failure;
		} else if (((Vector2)mover.gameObject.transform.position - (Vector2)target.position).magnitude > 1) {
			mover.SetTarget (target.position);
			return NodeState.Running;
		} else {
			return NodeState.Success;
		}
    }
}

public class MoveLeaf : Leaf {
	Mover mover;
	Transform target;

	public MoveLeaf(Mover _mover, string _name = "MoveTo", int _priority = 4) : base(_name, priority: _priority) {
		key = LeafKey.Move;
		mover = _mover;
		target = null;
	}

	public override NodeState GetState() {
		target = mover.GetCommander().FindTarget();
		if (target == null) {
			return NodeState.Failure;
		} else if (((Vector2)mover.gameObject.transform.position - (Vector2)target.position).magnitude > 1) {
			mover.SetTarget (target.position);
			return NodeState.Running;
		} else {
			return NodeState.Success;
		}
	}
}

public class RegroupLeaf : Leaf {
	Mover mover;
	Transform target;

	public RegroupLeaf(Mover _mover, string _name = "Regroup", int _priority = 4) : base(_name, priority: _priority) {
		key = LeafKey.Regroup;
		mover = _mover;
		target = null;
	}

	public override NodeState GetState() {
		target = mover.GetCommander().FindOfficer();
		if (target == null) {
			return NodeState.Failure;
		} else if (((Vector2)mover.gameObject.transform.position - (Vector2)target.position).magnitude > mover.GetCommander().SquadRadius()) {
			mover.SetTarget (target.position);
			return NodeState.Running;
		} else {
			return NodeState.Success;
		}
	}
}

public class FleeLeaf : Leaf {
    bool started = false;
    Mover mover;
    int fleeDir;

    public FleeLeaf(Mover _mover, int _fleeDir, string _name = "Flee", int _priority = 5) : base(_name, priority: _priority) {
        key = LeafKey.Flee;
        mover = _mover;
        fleeDir = _fleeDir;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            mover.SetTarget(new Vector2(mover.transform.position.x, fleeDir));
        }
        mover.Dash();
        return NodeState.Running;
    }
}


public class Selector : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Selector(string _name = "Selector", NodeDel _nodeDel = null, List < Node> _children = null) : base(_name, _nodeDel) {
		children =  (_children == null) ? new List<Node>() : _children;
		currentNodeIndex = 0;
	}

	public override NodeState GetState() {
        currPriority = priority;
        int highestState = 0;
        if (nodeDel != null && nodeDel() == NodeState.Failure) { return NodeState.Failure; }

		for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
                highestState = (children[i].currPriority > highestState) ? children[i].currPriority : highestState;
                currPriority = highestState;
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
	protected int currentStartIndex;
	protected int currentNodeOffset = 0;

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
	public RandomSelector(string _name = "Random Selector", NodeDel _nodeDel = null, List < Node> _children = null, List<int> _frequencies = null) 
        : base(_name, _nodeDel) {
        children = (_children == null) ? new List<Node>() : _children;
		generateCumulativeFrequencies (_frequencies);
		currentStartIndex = GetRandomIndex ();
		currentNodeOffset = 0;
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
        currPriority = priority;
        if (nodeDel != null && nodeDel() == NodeState.Failure) { return NodeState.Failure; }
        for (int i = currentNodeOffset; i < children.Count; i++) {
			NodeState childState = children [(i + currentStartIndex) % children.Count].GetState ();
			if (childState == NodeState.Running) {
				currentNodeOffset = i;
                currPriority = children[i].currPriority;
				return NodeState.Running;
			} else if (childState == NodeState.Success) {
				currentStartIndex = GetRandomIndex();
				currentNodeOffset = 0;
				return NodeState.Success;
			}
		}
		currentStartIndex = GetRandomIndex();
		currentNodeOffset = 0;
		return NodeState.Failure;
	}
}

public class Sequencer : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Sequencer(string _name = "Sequencer", NodeDel _nodeDel = null, List < Node> _children = null) : base(_name, _nodeDel) {
        children = (_children == null) ? new List<Node>() : _children;
        currentNodeIndex = 0;
	}

	public override NodeState GetState() {
        currPriority = priority;
        if (nodeDel != null && nodeDel() == NodeState.Failure) { return NodeState.Failure; }
        for (int i = currentNodeIndex; i < children.Count; i++) {
			NodeState childState = children [i].GetState ();
			if (childState == NodeState.Running) {
				currentNodeIndex = i;
                currPriority = children[i].currPriority;
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