using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour, ICommandable {

	List<ICommandable> minions;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public bool SetCommand(LeafKey command, int priority) {
		bool worked = false;
		for (int i = 0; i < minions.Count; i++) {
			worked |= (minions [i].SetCommand (command, priority));
		}
		return worked;
	}
}
