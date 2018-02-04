using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TRASH THIS SHIT
public class SquadScript : MonoBehaviour, IHasObjective {

    public Transform enemySquad; //MUST BE SET IN EDITOR

    public List<GameObject> squadMembers;
    public List<GameObject> enemyMembers;
    public string id;
    private string objectiveState;
    public bool objectiveDone;

    void Start() {
        objectiveState = "";
        objectiveDone = false;
        FillSquad(transform, squadMembers, true);
        FillSquad(enemySquad, enemyMembers);
    }

    void Update() {
        if (!objectiveDone) {
            CheckObjective();
        }
    }

    public void FillSquad(Transform entries, List<GameObject> squad, bool own = false) {
        foreach (Transform bodyHolder in entries) {
            foreach (Transform bodyMember in bodyHolder) {
                GameObject bm = bodyMember.gameObject;
                if (bm.CompareTag("Body")) {
                    squad.Add(bm);
                    if (own) {
                        BotMovement bodyScript = bodyMember.GetComponent<BotMovement>();
                        bodyScript.squad = this;
                    }
                }
            }
        }
    }

    public bool FindTarget(BotMovement bot) {
        if (enemyMembers.Count > 0) {
            bot.target = enemyMembers[Random.Range(0, enemyMembers.Count)].transform;
            return true;
        }
        else { return false; }
    }

    bool CheckSquad(List<GameObject> squad) {
        for (int i = 0; i < squad.Count; i++) {
            GameObject g = squad[i];
            if (!g.activeSelf) {
                squad.RemoveAt(i);
            }
        }
        return (squad.Count > 0);
    }

    public void CheckObjective() {
        if (!CheckSquad(squadMembers)) {
            objectiveState = id + "DEAD";
            objectiveDone = true;
        }
    }

    public string GetObjectiveState() { return objectiveState; }

    public bool ObjectiveFinished() { return objectiveDone; }
}
