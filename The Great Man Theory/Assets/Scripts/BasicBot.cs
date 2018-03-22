using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBot : MonoBehaviour {

	DefaultTree maintree;

	public Body body;

	public Flag flag;

    public Transform attackTarget; //Current Attack Target

	protected List<Command> commandlist;

	// Use this for initialization
	void Start () {
		maintree = new DefaultTree (body, flag);
		commandlist = new List<Command> ();
	}
	
	// Update is called once per frame
	void Update () {
		maintree.Traverse ();


	}

	public void cull() {
		for (int i = commandlist.Count; i >= 0; i--) {
			commandlist [i].timeLeft -= Time.deltaTime;
			if (commandlist [i].timeLeft <= 0) {
				commandlist [i].subtree.expired = true;
				commandlist.RemoveAt (i);
			}
		}
	}

	public void command(Command comm, int priority) {
		commandlist.Add (comm);
		maintree.insertAtPriority (comm, priority);
	}
}

public class DefaultTree {
	protected Node rootNode;

	List<Node> priorityBuckets;

	public DefaultTree(Body body, Flag flag) {

		priorityBuckets = new List<Node> () {
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

	public NodeState Traverse() {
		return rootNode.GetState ();
	}

	public void insertAtPriority(Command comm, int priority) {
		priority = Mathf.Clamp (priority, 0, priorityBuckets.Count - 1);
		((Selector)(priorityBuckets [priority])).insertChild (comm.subtree);
	}
}
