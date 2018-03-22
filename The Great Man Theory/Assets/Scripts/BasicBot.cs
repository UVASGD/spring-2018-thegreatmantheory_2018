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
		rootNode = new Selector("root", null, new List<Node>() {
			new Selector("priority 0", null, new List<Node>() {}),
			new Selector("priority 1", null, new List<Node>() {
				//Wounded node here
			}),
			new Selector("priority 2", null, new List<Node>() {
				//Default Attack goes here
			}),
			new Selector("priority 3", null, new List<Node>() {}),
			new Selector("priority 4", null, new List<Node>() {
				//Idle node here
			})
		});
	}

	public NodeState Traverse() {
		return rootNode.GetState ();
	}
}
