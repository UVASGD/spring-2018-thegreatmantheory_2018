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
    Bot bot;
    Transform pos;
    Vector2 target;
    int prefDist;
    int leeway;

    public MaintainLeaf(Bot _bot, float _timerMax, int _prefDist, int _leeway, int _priority = 1) : base(priority: _priority) {
        key = LeafKey.Maintain;
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
        pos = bot.transform;
        target = bot.Target;
        prefDist = _prefDist;
        leeway = _leeway;
    }

    public override NodeState GetState() {
        if (!started) {
            bot.Hold();
            started = true;
        }
        timer -= Time.deltaTime;
        target = bot.Target;
        target = ((Vector2)pos.position - target).normalized * prefDist;
        bot.SetTarget(target);
        if (timer <= 0) {
            timer = timerMax;
            started = false;
            bot.Hold(false);
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
    Bot bot;
    Vector2 target;
    float randoDist;


    public WiggleLeaf(Bot _bot, float _timerMax, float _maxWig, float _randoDist, int _priority = 2) : base(priority: _priority) {
        key = LeafKey.Wiggle;
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
        target = bot.Target;
        randoDist = _randoDist;
        maxWig = _maxWig;
        swingMax = UnityEngine.Random.Range(0.2f, _maxWig);
        swingTimer = swingMax;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            target = bot.Target + (UnityEngine.Random.insideUnitCircle * randoDist);
            bot.SetTarget(target);
            swingMax = UnityEngine.Random.Range(0.2f, maxWig);
            swingTimer = swingMax;
        }
        swingTimer -= Time.deltaTime;
        if (swingTimer <= 0) {
            swingTimer = swingMax;
            target = bot.Target + (UnityEngine.Random.insideUnitCircle * randoDist);
            bot.SetTarget(target);
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
    Bot bot;

    public ChargeLeaf(Bot _bot, float _timerMax, int _priority = 3) : base(priority: _priority) {
        key = LeafKey.Charge;
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            bot.SetTarget(bot.Target);
        }
        timer -= Time.deltaTime;
        bot.Dash();
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
    Bot bot;
    Body body;
    float timerMax;
    float timer;

    public FocusLeaf(Bot _bot, Body _body, float _timerMax, int _priority = 2) : base(priority: _priority) {
        key = LeafKey.Focus;
        timerMax = _timerMax;
        timer = UnityEngine.Random.Range(0.4f, timerMax);
        bot = _bot;
        body = _body;
    }

    public override NodeState GetState() {
        if (!started) {
            Debug.Log("OOF");
            started = true;
            Vector2 target = (bot.Target - body.weapon.pointer.ForcePoint).normalized;
            target = bot.transform.InverseTransformDirection(target);
            target = new Vector2(target.x + UnityEngine.Random.Range(-0.1f, 0.1f), target.y).normalized;
            bot.SetTarget(bot.transform.TransformDirection(target));
        }
        timer -= Time.deltaTime;
        bot.Brace();
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

    public ShootLeaf(Body _body, float _timerMax, int _priority = 5) : base(priority: _priority) {
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
    bool started = false;
    Bot bot;
	Transform target;
    Transform pos;


	public MedicLeaf(Bot _bot, int _priority = 5) : base(priority: _priority) {
        key = LeafKey.Medic;
        bot = _bot;
        target = null;
        pos = bot.transform;
	}

    public override NodeState GetState() {
        if (!started) {
            started = true;
            target = bot.GetCommander().FindMedic();
        }
		if (target == null) {
			return NodeState.Failure;
		}
        if (Vector2.Distance(target.position, pos.position) > 0.5f) {
			bot.SetTarget (target.position);
			return NodeState.Running;
		} else {
            started = false;
			return NodeState.Success;
		}
    }
}

public class MoveLeaf : Leaf {
    bool started = false;
	Bot bot;
	Transform target;
    Transform pos;

	public MoveLeaf(Bot _bot, int _priority = 4) : base(priority: _priority) {
		key = LeafKey.Move;
		bot = _bot;
        target = null;
        pos = bot.transform;
	}

	public override NodeState GetState() {
        if (!started) {
            started = true;
            target = bot.GetCommander().FindTarget();
        }
		if (target == null) {
			return NodeState.Failure;
		}
        if (Vector2.Distance(target.position, pos.position) > bot.GetCommander().SquadRadius()) {
			bot.SetTarget (target.position);
			return NodeState.Running;
		} else {
            started = false;
			return NodeState.Success;
		}
	}
}

public class RegroupLeaf : Leaf {
    bool started = false;
	Bot bot;
	Transform target;
    Transform pos;

	public RegroupLeaf(Bot _bot, int _priority = 4) : base(priority: _priority) {
		key = LeafKey.Regroup;
		bot = _bot;
		target = null;
        pos = bot.transform;
	}

	public override NodeState GetState() {
        if (!started) {
            started = true;
            target = bot.GetCommander().FindOfficer();
        }
		if (target == null) {
			return NodeState.Failure;
		}
        if (Vector2.Distance(target.position, pos.position) > bot.GetCommander().SquadRadius()) {
			bot.SetTarget (target.position);
			return NodeState.Running;
		} else {
            started = false;
			return NodeState.Success;
		}
	}
}

public class FleeLeaf : Leaf {
    bool started = false;
    Bot bot;
    int fleeDir;

    public FleeLeaf(Bot _bot, int _fleeDir, int _priority = 5) : base(priority: _priority) {
        key = LeafKey.Flee;
        bot = _bot;
        fleeDir = _fleeDir;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            bot.SetTarget(new Vector2(bot.transform.position.x, fleeDir));
        }
        bot.Dash();
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
        Debug.Log("AAAA");
        for (int i = currentNodeOffset; i < children.Count; i++) {
            NodeState childState = children [(i + currentStartIndex) % children.Count].GetState ();
			if (childState == NodeState.Running) {
				currentNodeOffset = i;
                currPriority = children[i].currPriority;
                Debug.Log(children[i]);
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