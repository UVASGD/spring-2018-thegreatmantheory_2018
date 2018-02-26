﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : ICommandable {

    protected Body body;
    protected Bot bot;
    protected Leaf command;
    protected int currentPriority = 0;

	protected ICommander commander;
    //squad

    protected List<Leaf> commandList; //Maybe change this to node, but probably not


    public BehaviorTree(Body _body, Bot _bot) {
        body = _body;
        bot = _bot;
    }

    protected Node rootNode;

    public void Traverse() {
        if (command != null) {
            if (command.GetState() == NodeState.Running)
                currentPriority = command.currPriority;
            else
                command = null;
        }
        else {
            rootNode.GetState();
            currentPriority = rootNode.currPriority;
        }
    }

    public bool SetCommand(LeafKey key, int priority) {
        if (priority > currentPriority)
            foreach (Leaf l in commandList)
                if (key == l.Key) {
                    command = l;
                    return true;
                }
        return false;
    }

	public void SetCommander(ICommander comm) {
		commander = comm;
	}

	public GameObject GetGameObject() {
		return body.gameObject;
	}
}

public class MeleeBehavior : BehaviorTree {
    public MeleeBehavior(Body _body, Bot _bot, int fleeDir, float mainTimer, int mainDist, int mainLeeway, float charTimer, float wigTimer, float maxWig, float wigDist) 
        : base(_body, _bot) {

        MedicLeaf medic = new MedicLeaf(bot);
        FleeLeaf flee = new FleeLeaf(bot, fleeDir);

        MaintainLeaf maintain = new MaintainLeaf(bot, mainTimer, mainDist, mainLeeway);
        ChargeLeaf charge = new ChargeLeaf(bot, charTimer);
        WiggleLeaf wiggle = new WiggleLeaf(bot, wigTimer, maxWig, wigDist);

		commandList = new List<Leaf>() { medic, flee, maintain, charge, wiggle, new MoveLeaf(_bot), new RegroupLeaf(_bot)};

        List<Node> WoundedList = new List<Node>() {
            medic, //timer, medic target from squad -- medic target == null
            flee //end zone from squad -- n/a
        };

        List<Node> PrepareList = new List<Node>() {
            maintain, //timer, distance from go, target from go -- n/a
            charge //target from go -- n/a
        };

        List<Node> FightList = new List<Node>() {
            wiggle, //target from go -- n/a
            new Sequencer("Prepare", _children:PrepareList) // n/a -- n/a
        };

        List<Node> RootList = new List<Node>() {
            new RandomSelector("Wounded", 
                delegate() { return (body.Wounded()) ? NodeState.Running : NodeState.Failure; },
                WoundedList), // n/a -- health < (maxHealth * 0.1)
            new RandomSelector("Fight", 
                delegate() { return (bot.targetObj) ? NodeState.Running : NodeState.Failure; }, 
                FightList) // n/a -- (target) && squad.Fight
        };

        rootNode = new Selector("RootNode", _children:RootList);
    }
}

public class RangedBehavior : BehaviorTree {
    public RangedBehavior(Body _body, Bot _bot, int fleeDir) : base(_body, _bot) {
        MedicLeaf medic = new MedicLeaf(bot);
        FleeLeaf flee = new FleeLeaf(bot, fleeDir);

        FocusLeaf focus = new FocusLeaf(bot, body, 1.5f);
        ShootLeaf shoot = new ShootLeaf(body, 1);

		commandList = new List<Leaf>() { medic, flee, focus, shoot, new MoveLeaf(bot), new RegroupLeaf(bot)};

        List<Node> WoundedList = new List<Node>() {
            medic, //timer, medic target from squad -- medic target == null
            flee //end zone from squad -- n/a
        };

        List<Node> ShootList = new List<Node>() {
            focus,
            shoot
        };

        List<Node> RootList = new List<Node>() {
            new RandomSelector("Wounded",
                delegate() { return (body.Wounded()) ? NodeState.Running : NodeState.Failure; },
                WoundedList), // n/a -- health < (maxHealth * 0.1)
            new RandomSelector("Fight",
                delegate() { return (bot.targetObj) ? NodeState.Running : NodeState.Failure; },
                ShootList) // n/a -- (target) && squad.Fight
        };

        rootNode = new Selector("RootNode", _children: RootList);
    }
}