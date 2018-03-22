using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBot : MonoBehaviour {

	DefaultTree maintree;

	public Body body;

	public Flag flag;

    public Transform attackTarget; //Current Attack Target

	// Use this for initialization
	void Start () {
		maintree = new DefaultTree (body, flag);
	}
	
	// Update is called once per frame
	void Update () {
		maintree.Traverse ();
	}
}

public class DefaultTree {
	protected Node rootNode;

	public DefaultTree(Body body, Flag flag) {
		rootNode = new Selector("root", new List<Node>() {
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
		});
	}

	public NodeState Traverse() {
		return rootNode.GetState ();
	}
}
