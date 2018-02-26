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
				if (t2.GetComponent<Bot>() != null) {
					minions.Add (t2.GetComponent<Bot> ());
					t2.GetComponent<Bot> ().SetCommander (this);
					Debug.Log ("Added a mover!");
				}
			}
		}
	}

    //TODO let's move this functionality to the Officer.
    //Basically, when someone leaves his exits his trigger collider, he'll push out a command to the squad to regroup.
    void Update () {
        if (target) {
            for (int i = 0; i < minions.Count; i++) {
                ((Bot)minions[i]).targetObj = target;
            }
        }
    }
        /*
		for (int i = 0; i < minions.Count; i++) {
			if((minions[i].GetGameObject().transform.position - officer.position).sqrMagnitude > SquadRadius()) {
				minions [i].SetCommand (LeafKey.Regroup, 4);
			}
		}
        */

	public override GameObject GetGameObject () {
		return officer.gameObject;
	}

	public override Transform FindOfficer() {
		if (officer == null) {
			//replace officer
			for (int i = 0; i < minions.Count; i++) {
				if(minions[i].GetGameObject().transform.parent.CompareTag("Officer")) { //Check whether the parent container of the body is tagged 'Officer'
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
				if(minions[i].GetGameObject().transform.parent.CompareTag("Medic")) { //Check whether the parent container of the body is tagged 'Medic'
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
