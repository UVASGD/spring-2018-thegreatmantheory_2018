using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquadType { Hold, Advance, FiringLine, Patrol, TargetFollow }
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
    public List<GameObject> enemies = new List<GameObject>();

    public Collider2D patrolArea;
    public GameObject target;

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
        SetDefaultBehavior(squadType);

        flag.Setup();
    }

    void Update() {
        time -= Time.deltaTime;
        UpdateEnemies();
        if (time <= 0) {
            time = interval;
            if (Command != null) {
                Command();
            }
        }
        if (!UpdateMinions())
            Destroy(gameObject);
    }

    public void Cutscene() {
        foreach (BasicBot b in minions) {
            // b.body.Unfreeze();
            b.body.cutsceneOverride = true;
        }
    }

    public void EndCutscene() {
        foreach (BasicBot b in minions) {
            // b.body.Unfreeze();
            b.body.cutsceneOverride = false;
        }
    }

    public void SetDefaultBehavior(SquadType s) {
        switch (s) {
            case SquadType.Hold:
                Command = null;
                break;
            case SquadType.Advance:
                Command = delegate () { MoveCommand(new Vector2(0, direction*500000)); };
                break;
            case SquadType.FiringLine:
                // fukkitup
                Command = delegate () { FiringLine(); };
                break;
            case SquadType.Patrol:
                Command = delegate () { PatrolCommand(); };
                break;
            case SquadType.TargetFollow:
                if (target) {
                    Command = delegate () { TargetCommand(target); };
                }
                else {
                    Command = null;
                }
                break;
        }
    }

    public void MoveCommand(Vector2 target, BasicBot bot = null, float timeLeft = -1, int priority = 3) {
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            if (bot)
                if (b != bot)
                    continue;
            b.Command(new Command(
                new Sequencer("Move", new List<Node>() {
                new Gate(delegate () {
                    if (Vector2.Distance(target, b.transform.position) > Mathf.Min(50, b.squad.SquadRadius))
                        return NodeState.Success;
                    return NodeState.Failure;
                }, "Move Gate"),
                new MoveLeaf(b, target)
                    }),
                interval),
            priority);
        }
    }

    public void TargetCommand(GameObject target, BasicBot bot = null, float timeLeft = -1, int priority = 3) {
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            if (bot)
                if (b != bot)
                    continue;
            b.Command(new Command(
                new Sequencer("MoveTarget", new List<Node>() {
                    new Gate(delegate () {
                        return (target && Vector2.Distance(target.transform.position, b.transform.position) > Mathf.Min(50, b.squad.SquadRadius))
                         ?NodeState.Success
                         :NodeState.Failure;
                    }, "MoveTarget Gate"),
                    new MoveTargetLeaf(b, target)
                    }),
                interval),
            priority);
        }
    }

    public void PatrolCommand() {
        if (!patrolArea) {
            SetDefaultBehavior(SquadType.Hold);
            squadType = SquadType.Hold;
        }
            

        time = 10;
        foreach (BasicBot b in minions) {
            Vector2 target = new Vector2(
                Random.Range(patrolArea.bounds.min.x, patrolArea.bounds.max.x), 
                Random.Range(patrolArea.bounds.min.y, patrolArea.bounds.max.y)
                );
            /* if (!patrolArea.OverlapPoint(target)) {
                time = 0.01f;
                continue;
            } */
            b.Command(new Command(
                new Sequencer("Patrol", new List<Node>() {
                    new Gate(delegate () {
                        return (Vector2.Distance(target, b.transform.position) > Mathf.Min(10, b.squad.SquadRadius))
                         ?NodeState.Success
                         :NodeState.Failure;}),
                    new MoveLeaf(b, target)
                    }),
                time),
            3);
        }
    }

    public void AttackSquad(Squad targetSquad, BasicBot bot = null, float timeLeft = -1, int priority = 3) {
        SetDefaultBehavior(SquadType.Hold);
        timeLeft = (timeLeft < 0) ? interval : timeLeft;
        foreach (BasicBot b in minions) {
            if (bot) {
                if (b != bot)
                    continue;
            }
            if (enemies.Count > 0) {
                b.attackTarget = enemies[Random.Range(0, enemies.Count)];
            }
        }
    }

    public void AttackIntruder(GameObject target) {
        SetDefaultBehavior(SquadType.Hold);
        foreach (BasicBot b in minions)
            if (!b.attackTarget) {
                b.attackTarget = target;
            }
    }

    public bool UpdateMinions() {
        if (minions.Count < 1) {
            return false;
        }
        if (!flag.carrier) {
            officer = minions[Random.Range(0, minions.Count)];
            flag.carrier = officer;
        }
        for (int i = 0; i < minions.Count; i++) {
            BasicBot b = minions[i];
            if (!b || b.Ded)
                minions.Remove(b);
        }
        return true;
    }

    public void FiringLine() {
        foreach (BasicBot b in minions) {
            b.Command(new Command(
                new VolleyLeaf(b, 
                new Vector2(0f, direction * 100f)
                ), 10000f)
            , 0);
        }
        SetDefaultBehavior(SquadType.Hold);
    }

    public void Attack(GameObject target) {
        foreach (BasicBot b in minions) {
            b.attackTarget = target;
        }
    }

    public void ResetCommand() {
        foreach (BasicBot b in minions) {
            b.ResetCommand();
        }
    }

    public void UpdateEnemies() {
        for (int i = 0; i < enemies.Count; i++)
            if (!enemies[i])
                enemies.RemoveAt(i);
    }

    public GameObject GetEnemy() {
        if (enemies.Count > 0)
            return enemies[Random.Range(0, enemies.Count)];
        else {
            if (squadType != SquadType.FiringLine) {
                SetDefaultBehavior(squadType);
            }
            return null;
        }
    }
}