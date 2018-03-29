using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        swingMax = Random.Range(0.2f, _maxWig);
        swingTimer = swingMax;
    }

    public override NodeState GetState() {
        if (!started) {
            started = true;
            targetPos = (Vector2)target.position + (Random.insideUnitCircle * randoDist);

            swingMax = Random.Range(0.2f, maxWig);
            swingTimer = swingMax;
        }
        swingTimer -= Time.deltaTime;
        if (swingTimer <= 0) {
            swingTimer = swingMax;
            targetPos = (Vector2)target.position + (Random.insideUnitCircle * randoDist);

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
        }
        else {
            started = false;
            return NodeState.Success;
        }
    }
}

public class MoveTargetLeaf : Leaf {
    BasicBot bot;
    Transform target;

    public MoveTargetLeaf(BasicBot _bot, Transform _target) : base() {
        bot = _bot;
        target = _target;
    }

    public override NodeState GetState() {
        //if (Vector2.Distance(target.position, bot.gameObject.transform.position) > bot.squad.SquadRadius) {
        bot.Move(target.position);
        return NodeState.Success;
    }
}

public class MoveLeaf : Leaf {
    BasicBot bot;
    Vector2 target;

    public MoveLeaf(BasicBot _bot, Vector2 _target) : base() {
        bot = _bot;
        target = _target;
    }

    public override NodeState GetState() {
        //if (Vector2.Distance(target, bot.gameObject.transform.position) > bot.squad.SquadRadius) {
        bot.Move(target);
        return NodeState.Success;
    }
}

/*
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
		}
		return NodeState.Failure;
	}
}
*/

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