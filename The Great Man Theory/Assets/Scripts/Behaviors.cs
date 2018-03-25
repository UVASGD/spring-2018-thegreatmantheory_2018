using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultTree {
    protected Node rootNode;

    protected List<Node> priorityBuckets;

    public DefaultTree() {

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {
				//Wounded node here
			}),
            new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
            new Selector("priority 3", new List<Node>() {}),
            new Selector("priority 4", new List<Node>() {
				//Idle node here
			})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }

    public virtual NodeState Traverse() {
        return rootNode.GetState();
    }

    public void insertAtPriority(Command comm, int priority) {
        priority = Mathf.Clamp(priority, 0, priorityBuckets.Count - 1);
        ((Selector)(priorityBuckets[priority])).insertChild(comm.subtree);
    }
}

public class MeleeTree : DefaultTree {
    public MeleeTree(BasicBot bot) {
        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {
                new RandomSelector("Wounded", new List<Node>() {
                    new FleeLeaf(bot),
                    new MedicLeaf(bot)
                })
			}),
            new Selector("priority 2", new List<Node>() {
                new Gate(delegate() {
                    return (!bot.attackTarget || Vector2.Distance(bot.transform.position, bot.attackTarget.position) > 10) 
                    ? NodeState.Success: NodeState.Failure;
                }),
				new RandomSelector("Fight", new List<Node>() {               
                    new WiggleLeaf(bot),
                    new Sequencer("Prepare", new List<Node>() {
                        new MaintainLeaf(bot),
                        new ChargeLeaf(bot)
                    })
                })
			}),
            new Selector("priority 3", new List<Node>() {}),
            new Selector("priority 4", new List<Node>() {
				//Idle node here
			})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

public class SquadHoldTree : DefaultTree {
    Vector2 pos;
    float orderTime = Random.Range(10, 15);

    public SquadHoldTree(ArmySquad squad) {
        pos = squad.transform.position;

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Hold", new List<Node>() {
                    new IntervalGate(orderTime),
                    new CommandNode(squad, 
                        new MoveCommand(pos, orderTime), 3)
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

public class SquadAdvanceTree : DefaultTree {

    public SquadAdvanceTree(ArmySquad squad) {
        float moveInterval = Random.Range(20, 30);

        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {}),
            new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Advance", new List<Node>() {
                    new IntervalGate(moveInterval),
                    new CommandNode(squad, 
                        new MoveCommand(new Vector2(0, squad.direction*1000), moveInterval), 3)
                })
            }),
            new Selector("priority 4", new List<Node>() {})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}

/*
public class BehaviorTree {

    protected BasicBot bot;

    protected List<Leaf> commandList; //Maybe change this to node, but probably not


    public BehaviorTree(BasicBot _bot) {
        bot = _bot;
    }

    protected Node rootNode;

    public void Traverse() {
        rootNode.GetState();
    }
}

public class MeleeBehavior : BehaviorTree {
    public MeleeBehavior(BasicBot _bot, float mainTimer = 1.5f, int mainDist = 3, int mainLeeway = 8, float charTimer = 1.5f, float wigTimer = 3, float maxWig = 0.4f, float wigDist = 8) 
        : base(_bot) {

        MedicLeaf medic = new MedicLeaf(bot);
        FleeLeaf flee = new FleeLeaf(bot);

        MaintainLeaf maintain = new MaintainLeaf(bot, mainTimer, mainDist, mainLeeway);
        ChargeLeaf charge = new ChargeLeaf(bot, charTimer);
        WiggleLeaf wiggle = new WiggleLeaf(bot, wigTimer, maxWig, wigDist);

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
                //delegate() { return (body.Wounded()) ? NodeState.Running : NodeState.Failure; },
                WoundedList), // n/a -- health < (maxHealth * 0.1)
            new RandomSelector("Fight", 
                //delegate() { return (bot.targetObj) ? NodeState.Running : NodeState.Failure; }, 
                FightList) // n/a -- (target) && squad.Fight
        };

        rootNode = new Selector("RootNode", _children:RootList);
    }
}

public class RangedBehavior : BehaviorTree {
    public RangedBehavior(Body _body, BasicBot _bot, int fleeDir) : base(_bot) {
        MedicLeaf medic = new MedicLeaf(bot);
        FleeLeaf flee = new FleeLeaf(bot);

        FocusLeaf focus = new FocusLeaf(bot, 1.5f);
        ShootLeaf shoot = new ShootLeaf(bot, 1);

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
                //delegate() { return (body.Wounded()) ? NodeState.Running : NodeState.Failure; },
                WoundedList), // n/a -- health < (maxHealth * 0.1)
            new RandomSelector("Fight",
                //delegate() { return (bot.targetObj) ? NodeState.Running : NodeState.Failure; },
                ShootList) // n/a -- (target) && squad.Fight
        };

        rootNode = new Selector("RootNode", _children: RootList);
    }
}
*/
