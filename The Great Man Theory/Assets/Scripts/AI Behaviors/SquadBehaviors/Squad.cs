using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* public abstract class Squad : MonoBehaviour, ICommandable, ICommander {

	public List<ICommandable> minions;

    public ICommander commander;

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

	public void SetCommander(ICommander comm) {
		commander = comm;
	}

	public abstract float SquadRadius ();

	public abstract GameObject GetGameObject();

	public abstract Transform FindOfficer();

	public abstract Transform FindMedic();

	public abstract Transform FindTarget();
} */
