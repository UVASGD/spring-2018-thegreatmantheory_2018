using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquadType { Hold, Advance }

public class Squad : MonoBehaviour {

    public DefaultTree maintree;

    public Team team;
    public SquadType squadType;

    public Flag flag;
    public BasicBot officer;
    public BasicBot medic;

    public int direction = 1;

    public float SquadRadius { get { return 2 * minions.Count; } } 

    public List<BasicBot> minions = new List<BasicBot>();
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
        SetTree(squadType);
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