using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandFactory {
	Node GenerateSubtree (BasicBot b);
}

public class MoveFactory : ICommandFactory {

	Vector2 target;

	public MoveFactory(Vector2 _target) {
		target = _target;
	}

	public Node GenerateSubtree (BasicBot b) {
		Sequencer seq = new Sequencer ("Move", new List<Node> () {
			new Gate(delegate() {
				if(Vector2.Distance(target, b.gameObject.transform.position) > b.squad.SquadRadius) {
					return NodeState.Success;
				}
				return NodeState.Failure;
			}),
			new MoveLeaf(b, target)
		});

		return seq;
	}

}