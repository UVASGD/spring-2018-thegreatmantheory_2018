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



public class Leaf : Node {

    public Leaf(string _name = "Leaf") : base(_name) {
    }

    public override NodeState GetState() {
        return NodeState.Running;
    }
}

public class MaintainLeaf : Leaf {
    bool started = false;
    float timerMax;
    float timer;
    BasicBot bot;
    Transform pos;
    Transform target;
    Vector2 targetPos;
    int prefDist;
    int leeway;

    public MaintainLeaf(BasicBot _bot, float _timerMax = 1.5f, int _prefDist = 2, int _leeway = 8) : base() {
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
        pos = bot.transform;
        target = bot.attackTarget;
        prefDist = _prefDist;
        leeway = _leeway;
    }

    public override NodeState GetState() {
        if (!started) {
            bot.Hold();
            started = true;
        }
        timer -= Time.deltaTime;
        target = bot.attackTarget;
        targetPos = (pos.position - target.position).normalized * prefDist;
        bot.Move(targetPos);
        if (timer <= 0) {
            timer = timerMax;
            started = false;
            bot.Hold(false);
            return (Mathf.Abs(prefDist - Vector2.Distance(pos.position, targetPos)) < leeway) ? NodeState.Success : NodeState.Failure;
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
    BasicBot bot;
    Transform target;
    Vector2 targetPos;
    float randoDist;

    public WiggleLeaf(BasicBot _bot, float _timerMax = 2, float _maxWig = 0.4f, float _randoDist = 2) : base() {
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
        target = bot.attackTarget;
        randoDist = _randoDist;
        maxWig = _maxWig;
        swingMax = UnityEngine.Random.Range(0.2f, _maxWig);
        swingTimer = swingMax;
    }

    public override NodeState GetState() {
		Debug.Log ("wiggle-wiggle");
        if (!started) {
            started = true;
            targetPos = (Vector2)bot.attackTarget.position + (UnityEngine.Random.insideUnitCircle * randoDist);
 
            swingMax = UnityEngine.Random.Range(0.2f, maxWig);
            swingTimer = swingMax;
        }
        swingTimer -= Time.deltaTime;
        if (swingTimer <= 0) {
            swingTimer = swingMax;
            targetPos = (Vector2)bot.attackTarget.position + (UnityEngine.Random.insideUnitCircle * randoDist);
            
        }
		bot.Move(targetPos);
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
    BasicBot bot;

    public ChargeLeaf(BasicBot _bot, float _timerMax = 1.5f) : base() {
        timerMax = _timerMax;
        timer = timerMax;
        bot = _bot;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            
        }
        timer -= Time.deltaTime;
        bot.Dash();
		bot.Move(bot.attackTarget.position);
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
    BasicBot bot;
    Body body;
    float timerMax;
    float timer;

    public FocusLeaf(BasicBot _bot, float _timerMax) : base() {
        timerMax = _timerMax;
        timer = UnityEngine.Random.Range(0.4f, timerMax);
        bot = _bot;
        body = bot.body;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            Vector2 target = ((Vector2)bot.attackTarget.position - body.weapon.pointer.ForcePoint).normalized;
            target = bot.transform.InverseTransformDirection(target);
            target = new Vector2(target.x + UnityEngine.Random.Range(-0.1f, 0.1f), target.y).normalized;
            bot.Move(bot.transform.TransformDirection(target));
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
    RangedWeapon weapon;
    float timerMax;
    float timer;

    public ShootLeaf(BasicBot bot, float _timerMax) : base() {
        timerMax = _timerMax;
        timer = UnityEngine.Random.Range(0.4f, timerMax);
        weapon = (RangedWeapon)bot.body.weapon;
    }

    public override NodeState GetState() {
        if (!fired) {
            fired = true;
            weapon.Trigger();
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
    BasicBot bot;
	Transform target;
    Transform pos;


	public MedicLeaf(BasicBot _bot) : base() {
        bot = _bot;
        target = null;
        pos = bot.transform;
	}

    public override NodeState GetState() {
        if (!started) {
            started = true;
            //target = bot.squad.FindMedic();
        }
		if (target == null) {
			return NodeState.Failure;
		}
        if (Vector2.Distance(target.position, pos.position) > 0.5f) {
			bot.Move(target.position);
			return NodeState.Running;
		} else {
            started = false;
			return NodeState.Success;
		}
    }
}

public class MoveTargetLeaf : Leaf {
    BasicBot bot;
	Transform target;
    Transform pos;

	public MoveTargetLeaf(BasicBot _bot, Transform _target) : base() {
        bot = _bot;
        target = _target;
        pos = bot.transform;
	}

	public override NodeState GetState() {
		if (Vector2.Distance(target.position, bot.gameObject.transform.position) > bot.squad.SquadRadius) {
			bot.Move (target.position);
			return NodeState.Running;
		} else {
			return NodeState.Success;
		}
		return NodeState.Failure;
	}
}

public class MoveLeaf : Leaf {
    BasicBot bot;
    Vector2 target;
    Transform pos;

    public MoveLeaf(BasicBot _bot, Vector2 _target) : base() {
        bot = _bot;
        target = _target;
        pos = bot.transform;
    }

    public override NodeState GetState() {
		if (Vector2.Distance(target, bot.gameObject.transform.position) > bot.squad.SquadRadius) {
			bot.Move (target);
			return NodeState.Running;
		} else {
			return NodeState.Success;
		}
        return NodeState.Failure;
    }
}

//TODO Maybe just replace RegroupLeaf with MoveTarget?
public class RegroupLeaf : Leaf {
    bool started = false;
	BasicBot bot;
	Transform target;
    Transform pos;

	public RegroupLeaf(BasicBot _bot) : base() {
		bot = _bot;
		target = null;
        pos = bot.transform;
	}

	public override NodeState GetState() {
        if (!started) {
            started = true;
            //target = bot.squad.officer();
        }
		if (target == null) {
			return NodeState.Failure;
		}
        /*if (Vector2.Distance(target.position, pos.position) > bot.GetCommander().SquadRadius()) {
			bot.SetTarget (target.position);
			return NodeState.Running;
		} else {
            started = false;
			return NodeState.Success;
		}*/
		return NodeState.Failure;
	}
}

public class FleeLeaf : Leaf {
    bool started = false;
    BasicBot bot;

    public FleeLeaf(BasicBot _bot) : base() {
        bot = _bot;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            bot.Move(new Vector2(bot.transform.position.x, 100)); //TODO Get flee direction from squad or something
        }
        bot.Dash();
        return NodeState.Running;
    }
}


public class CommandNode : Node {
    ArmySquad squad;
    Command comm;
    int priority;

    public CommandNode(ArmySquad _squad, Command _comm, int _priority) {
        squad = _squad;
        comm = _comm;
        priority = _priority;
    }

    public override NodeState GetState() {
        squad.Command(comm, priority);
        return NodeState.Success;
    }
}


public class Gate : Node {
    NodeDel del;

    public Gate(NodeDel _del) {
        del = _del;
    }

    public override NodeState GetState() {
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
        return NodeState.Running;  
    }
}

public class OneShotGate : Node {
    bool fail;
    bool done = false;

    public OneShotGate(bool _fail = false) {
        fail = _fail;
    }

    public override NodeState GetState() {
        if (!done)
            done = true;
        return (fail) ? NodeState.Failure : NodeState.Success;
    }
}


public class Selector : Node {
	protected List<Node> children;
	protected int currentNodeIndex = 0;

	public Selector(string _name = "Selector", List < Node> _children = null) : base(_name) {
		children =  (_children == null) ? new List<Node>() : _children;
		currentNodeIndex = 0;
	}

	public override NodeState GetState() {

		for (int i = currentNodeIndex; i < children.Count; i++) {
			if (children [i].expired) {
				children.RemoveAt (i);
				if (i >= children.Count) {
					break;
				}
			}
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


	public void insertChild(Node n) {
		children.Insert (0, n);
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
	public RandomSelector(string _name = "Random Selector", List < Node> _children = null, List<int> _frequencies = null) 
        : base(_name) {
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
        for (int i = currentNodeOffset; i < children.Count; i++) {
			if (children [(i + currentStartIndex) % children.Count].expired) {
				children.RemoveAt ((i + currentStartIndex) % children.Count);
				if (i >= children.Count) {
					break;
				}
			}
            NodeState childState = children [(i + currentStartIndex) % children.Count].GetState ();
			if (childState == NodeState.Running) {
				currentNodeOffset = i;
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

	public Sequencer(string _name = "Sequencer", List < Node> _children = null) : base(_name) {
        children = (_children == null) ? new List<Node>() : _children;
        currentNodeIndex = 0;
	}

	public override NodeState GetState() {
        for (int i = currentNodeIndex; i < children.Count; i++) {
			if (children [i].expired) {
				children.RemoveAt (i);
				if (i >= children.Count) {
					break;
				}
			}
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