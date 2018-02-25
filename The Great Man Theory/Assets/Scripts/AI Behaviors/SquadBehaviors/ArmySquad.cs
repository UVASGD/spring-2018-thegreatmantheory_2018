using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmySquad : Squad {

	public Transform officer;
	public Transform medic;
	public Transform target;

	// Use this for initialization
	void Start () {
		minions = new List<ICommandable> ();
		foreach (Transform t in transform) {
			foreach (Transform t2 in t) {
				if (t2.GetComponent<Mover>() != null) {
					minions.Add (t2.GetComponent<Mover> ());
					t2.GetComponent<Mover> ().SetCommander (this);
					Debug.Log ("Added a mover!");
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < minions.Count; i++) {
			if((minions[i].GetGameObject().transform.position - this.GetGameObject().transform.position).sqrMagnitude > SquadRadius()) {
				minions [i].SetCommand (LeafKey.Regroup, 4);
			}
		}
	}

	public override GameObject GetGameObject () {
		return officer.gameObject;
	}

	public override Transform FindOfficer() {
		if (officer == null) {
			//replace officer
			for (int i = 0; i < minions.Count; i++) {
				if(minions[i].GetGameObject().tag == "Officer") {
					officer = minions[i].GetGameObject().transform;
					break;
				}
			}
		}

		return officer;
	}

	public override Transform FindMedic() {
		if (medic == null) {
			//replace medic
			for (int i = 0; i < minions.Count; i++) {
				if(minions[i].GetGameObject().tag == "Medic") {
					officer = minions[i].GetGameObject().transform;
					break;
				}
			}
		}
		return medic;
	}

	public override Transform FindTarget() {
		return target;
	}

	public override float SquadRadius() {
		return 4 * minions.Count;
	}
}
