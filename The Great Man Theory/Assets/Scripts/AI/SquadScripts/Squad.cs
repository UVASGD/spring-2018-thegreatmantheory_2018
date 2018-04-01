﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquadType { Hold, Advance, FiringLine }
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
            Debug.Log("Time Less than or equal to zero");
            if (Command != null) {
                Debug.Log("Calling Command");
                Debug.Log("Command:" + Command);
                Command();
            }
            time = interval;
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
                Debug.Log("In FiringLine Case");
                Command = delegate () { FiringLine(); Debug.Log("Delegate is being done"); };
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

    public void UpdateMinions() {
        foreach (BasicBot b in minions) {
            if (!b || b.Ded)
                minions.Remove(b);
        }
        if (minions.Count == 0)
            Destroy(gameObject);
        if (!flag.carrier) {
            officer = minions[Random.Range(0, minions.Count)];

            flag.carrier = officer;
        }
    }

    public void FiringLine() {
        Debug.Log("Start of FiringLine");
        foreach (BasicBot b in minions) {
            b.Command(new Command(
                new VolleyLeaf(b, 
                new Vector2(0f, direction * 100f)
                ), 10000f)
            , 0);
        }
        Debug.Log("Command Set");
        SetDefaultBehavior(SquadType.Hold);
    }

    public void UpdateEnemies() {
        foreach (GameObject g in enemies)
            if (g == null)
                enemies.Remove(g);
    }

    public GameObject GetEnemy() {
        if (enemies.Count > 0)
            return enemies[Random.Range(0, enemies.Count)];
        SetDefaultBehavior(squadType);
        return null;
    }
}