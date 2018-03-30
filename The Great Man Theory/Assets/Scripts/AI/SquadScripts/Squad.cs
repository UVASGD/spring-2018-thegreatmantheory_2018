using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquadType { Hold, Advance }
public delegate void SquadDel();

public class Squad : MonoBehaviour {

    public SquadDel Command;

    public Team team;
    public SquadType squadType;

    public float interval;
    private float time;

    public Flag flag;
    public BasicBot officer;
    public BasicBot medic;

    public int direction = 1;

    public float SquadRadius { get { return 2 * minions.Count; } } 

    public List<BasicBot> minions = new List<BasicBot>();
    public List<Transform> enemies = new List<Transform>();


    void Start() {
        interval = 0.5f;
        time = interval;

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
        //SetTree(squadType);
    }

    void Update() {
        time -= Time.deltaTime;
        if (time <= 0) {
            if (Command != null) {
                Command();
            }
            time = interval;
        }
    }

    public void MoveCommand(Vector2 target, float timeLeft = -1, int priority = 3) {
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            b.Command(new Command(
                new Sequencer("Move", new List<Node>() {
                    new Gate(delegate () {
                        if (Vector2.Distance(target, b.transform.position) > b.squad.SquadRadius)
                            return NodeState.Success;
                        return NodeState.Failure;
                    }, "Move Gate"),
                    new MoveLeaf(b, target)
                    }),
                interval), 
            priority);
        }
    }

    public void TargetCommand(Transform target, float timeLeft = -1, int priority = 3) {
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            b.Command(new Command(
                new Sequencer("MoveTarget", new List<Node>() {
                    new Gate(delegate () {
                        return (target && Vector2.Distance(target.position, b.transform.position) > b.squad.SquadRadius)
                         ?NodeState.Success
                         :NodeState.Failure;
                    }, "MoveTarget Gate"),
                    new MoveTargetLeaf(b, target)
                    }),
                interval),
            priority);
        }
    }

    public void AttackSquad(Squad targetSquad, float timeLeft = -1, int priority = 3) {
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            b.Command(new Command(
                new Sequencer("Attack Squad", new List<Node>() {
                    new Gate(delegate () {
                        return (targetSquad.flag && Vector2.Distance(targetSquad.flag.transform.position, b.transform.position) > b.squad.SquadRadius)
                         ?NodeState.Success
                         :NodeState.Failure;
                    }),
                    new MoveTargetLeaf(b, targetSquad.flag.transform)
                    }),
                interval),
            priority);
        }
    }


    /*
    private void Update() {
        maintree.Traverse();
        Cull();
    }

    public void Command(Command comm, int priority) {
        commandlist.Add(comm);
        maintree.insertAtPriority(comm.subtree, priority);
    }

    public void Cull() {
        for (int i = commandlist.Count - 1; i >= 0; i--) {
            commandlist[i].timeLeft -= Time.deltaTime;
            if (commandlist[i].timeLeft <= 0) {
                commandlist[i].subtree.expired = true;
                commandlist.RemoveAt(i);
            }
        }
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

    public void Command(BasicBot bot, Command comm, int priority) {
        bot.Command(comm, priority);
    }*/
}