using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTree : DefaultTree {
    public SwordTree(BasicBot bot) {
        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {
                new Sequencer("wounded", new List<Node>() {
                    new Gate(delegate () {
                        return NodeState.Failure;
                    }, "Wounded Gate"),
                    new RandomSelector("Wounded Action", new List<Node>() {
                        new FleeLeaf(bot),
                        new MedicLeaf(bot)
                    })
                })
            }),
            new Selector("priority 2", new List<Node>() {
                new Sequencer("fight", new List<Node>() {
                    new Gate(delegate() {return (bot.attackTarget == null) ? NodeState.Failure : NodeState.Success; }),
                    new RandomSelector("Fight", new List<Node>() {
                        new WiggleLeaf(bot),
                        new Sequencer("Prepare", new List<Node>() {
                            new MaintainLeaf(bot),
                            new ChargeLeaf(bot)
                        })
                    }, new List<int>() {
                        1, 0
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

public class ArquebusTree : DefaultTree {
	public ArquebusTree(BasicBot bot) {
		priorityBuckets = new List<Node> () {
			new Selector("priority 0", new List<Node>() {

			}),
			new Selector("priority 1", new List<Node>() {
				new Sequencer("wounded", new List<Node>() {
					new Gate(delegate () {
						return NodeState.Failure;
					}, "Wounded Gate"),
					new RandomSelector("Wounded Action", new List<Node>() {
						new FleeLeaf(bot),
						new MedicLeaf(bot)
					})
				})
			}),
			new Selector("priority 2", new List<Node>() {
				new Sequencer("fight!", new List<Node>() {
					new Gate(delegate() {
						// Debug.Log("Acting: " + (bot.attackTarget != null && Vector2.Distance(bot.transform.position, bot.attackTarget.position) < 100));
						return (bot.attackTarget && Vector2.Distance(bot.transform.position, bot.attackTarget.transform.position) < 100)
							? NodeState.Success: NodeState.Failure;
					}, "Fight Gate"),
					//new IntervalGate(5),
					new AimLeaf(bot)
				})
			}),
			new Selector("priority 3", new List<Node>() {
				
			}),
			new Selector("priority 4", new List<Node>() {
				
			})
		};

		rootNode = new Selector("root", (priorityBuckets));
	}
}

public class CavalryTree : DefaultTree {
    public CavalryTree(BasicBot bot) {
        priorityBuckets = new List<Node>() {
            new Selector("priority 0", new List<Node>() {}),
            new Selector("priority 1", new List<Node>() {
				//Wounded node here
			}),
            new Selector("priority 2", new List<Node>() {
			}),
            new Selector("priority 3", new List<Node>() {
                new Sequencer("Cavalry Charge", new List<Node>() {
                    new CavalryChargeLeaf(bot),
                    new CavalryRecenterLeaf(bot)
                })
            }),
            new Selector("priority 4", new List<Node>() {
				//Idle node here
			})
        };

        rootNode = new Selector("root", (priorityBuckets));
    }
}