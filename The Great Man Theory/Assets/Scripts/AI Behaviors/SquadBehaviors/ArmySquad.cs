using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquadType { Hold, Advance }

public class ArmySquad : Squad {

    public DefaultTree maintree;

    public Team team;
    public SquadType squadType;

    public Flag flag;
    public BasicBot officer;
    public BasicBot medic;

    public int direction = 1;

    public float SquadRadius { get { return 2 * minions.Count; } } 

    List<BasicBot> minions = new List<BasicBot>();
    public List<Transform> enemies = new List<Transform>();


    void Start() {
        foreach (Transform t in transform) {
            foreach (Transform t2 in t) {
                BasicBot b = t2.GetComponent<BasicBot>();
                if (b) {
                    minions.Add(b);
                    b.squad = this;
                    b.body.team = team;
                    b.body.SetColors();
                    b.body.ApplyColors();
                    officer = (t.CompareTag("Officer") && !officer) ? b : officer;
                    medic = (t.CompareTag("Medic") && !medic) ? b : medic;
                }
            }
        }

        if (!officer)
            officer = minions[Random.Range(0, minions.Count)];

        flag.carrier = officer;
    }

    public Transform FindEnemy() {
        if (enemies.Count == 0)
            return null;

        for (int i = 0; i < enemies.Count; i++)
            if (!enemies[i])
                enemies.RemoveAt(i);

        return enemies[Random.Range(0, enemies.Count)];
    }

    public void SetTree(SquadType s) {
        switch (s) {
            case SquadType.Hold:
                maintree = new SquadHoldTree(this);
                break;
            case SquadType.Advance:
                maintree = new SquadAdvanceTree(this);
                break;
        }
    }

    public void Command(Command comm, int priority) {
        foreach (BasicBot bot in minions) {
            bot.Command(comm, priority);
        }
    }

    public void Command(BasicBot bot, Command comm, int priority) {
        bot.Command(comm, priority);
    }
}

/* public class ArmySquad : Squad {

    List<Transform> enemies = new List<Transform>();
    Body thisBody;

    public Flag flag;
	public Transform medic;
    public Transform target;

	// Use this for initialization
	void Start () {
        target = new GameObject("Target").transform;
        minions = new List<ICommandable> ();
		foreach (Transform t in transform) {
			foreach (Transform t2 in t) {
				if (t2.GetComponent<ICommandable>() != null) {
					minions.Add (t2.GetComponent<ICommandable> ());
					t2.GetComponent<ICommandable> ().SetCommander (this);
					//Debug.Log ("Added a mover!");
				}
			}
		}
	}

    //TODO let's move this functionality to the Officer.
    //Basically, when someone leaves his exits his trigger collider, he'll push out a command to the squad to regroup.
    void Update() {
        /*if (target) {
            for (int i = 0; i < minions.Count; i++) {
                ((Bot)minions[i]).targetObj = target;
            }
        }

		for (int i = 0; i < minions.Count; i++) {
			if((minions[i].GetGameObject().transform.position - officer.position).sqrMagnitude > SquadRadius()) {
				minions [i].SetCommand (LeafKey.Regroup, 4);
			}
		}
        */
    /*}

    public override GameObject GetGameObject () {
		return flag.gameObject;
	}

	public override Transform FindOfficer() {
        /*
		if (flag.transform.parent == null) {
			//replace officer
			for (int i = 0; i < minions.Count; i++) {
				if(minions[i].GetGameObject().transform.parent.CompareTag("Officer")) { //Check whether the parent container of the body is tagged 'Officer'
                    officer = minions[i].GetGameObject().transform;
					break;
				}
			}
		}*/

	/*	return flag.transform;
	}

	public override Transform FindMedic() {
		if (medic == null) {
			//replace medic
			for (int i = 0; i < minions.Count; i++) {
				if(minions[i].GetGameObject().transform.parent.CompareTag("Medic")) { //Check whether the parent container of the body is tagged 'Medic'
					medic = minions[i].GetGameObject().transform;
					break;
				}
			}
		}
		return medic;
	}

	public override Transform FindTarget() {
		return target;
	}

    public Transform FindEnemy() {
        return flag.FindEnemy();
    }

    public override float SquadRadius() {
		return 4 * minions.Count;
	}
} */
