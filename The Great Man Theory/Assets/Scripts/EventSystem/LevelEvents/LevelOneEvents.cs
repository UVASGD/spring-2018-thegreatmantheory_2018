using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOneEvents : EventManager {

    public Squad friendlyGuns;
    public Squad friendlyPike;

    public Squad enemyGuns;
    public Squad enemyPike;

    // public Transform enemyWaypoint;
    // public Transform frinedlyWaypoint;

    public DialogueTrigger focusText;
    public DialogueTrigger chargeDialogue;
    public DialogueTrigger firstFightDone;
    public DialogueTrigger treesClearedDialogue;

    public Squad boundingEnemies;
    Transform playerTransform;

    public Transform focusPoint;

    CameraPoints camPoints;

    Camera cam;

    int deadEnemies = 0;
    int firstEnemiesDead = 0;

    bool hasDoneEnemyPikeDefeat = false;

    public Collider2D patrolArea;
    public SquadSpawner mainSpawner;

    public GameObject door;

    void Awake() {
        cam = Camera.main;
    }

    void Start() {
        camPoints = CameraPoints.Instance;
        playerTransform = ManagerGetter.gm.player.transform;
    }

    void Update() {
        if (deadEnemies >= 2) {
            StartCoroutine("WIN");
        }
        if (firstEnemiesDead == 2 && !hasDoneEnemyPikeDefeat)
            StartCoroutine("EnemyPikeDefeated");
    }

	public IEnumerator BoundingEnemiesTarget() {

        boundingEnemies.Command = delegate () { boundingEnemies.TargetCommand(playerTransform.gameObject); };

        yield return null;
    }

    public IEnumerator FocusOnEnemies() {
        GameManager.state = GameState.Paused;
        CameraFollow follow = cam.GetComponent<CameraFollow>();

        follow.followPoint = focusPoint;

        while (!follow.OnTarget) {
            yield return null;
        }
        focusText.TriggerDialogue();
    }

    public IEnumerator FocusOnPlayer() {
        CameraFollow follow = cam.GetComponent<CameraFollow>();

        follow.followPoint = playerTransform;

        yield return null;
    }

    public IEnumerator Pause() {
        GameManager.state = GameState.Paused;
        yield return null;
    }

    public IEnumerator Gameplay() {
        GameManager.state = GameState.Gameplay;
        yield return null;
    }

    public IEnumerator HaltFriendlyGuns() {
        // friendlyGuns.Command = delegate () { friendlyGuns.Halt(); };
        if (friendlyGuns.squadType != SquadType.Hold) {
            friendlyGuns.squadType = SquadType.Hold;
            friendlyGuns.SetDefaultBehavior(SquadType.Hold);
        }
        friendlyGuns.ResetCommand();

        // if (enemyGuns.squadType != SquadType.FiringLine)
        //    enemyGuns.SetDefaultBehavior(SquadType.FiringLine);
        yield return null;
    }

    public IEnumerator FriendlyGunsFire() {
        // Debug.Log("SHOOTEM");
        friendlyGuns.Cutscene();
        if (friendlyGuns.squadType != SquadType.FiringLine) {
            friendlyGuns.squadType = SquadType.FiringLine;
            friendlyGuns.SetDefaultBehavior(SquadType.FiringLine);
        }

        TimeTrigger trigger = friendlyGuns.GetComponent<TimeTrigger>();
        trigger.active = true;

        yield return null;
    }

    public IEnumerator RetreatFriendlyGuns() {
        friendlyGuns.squadType = SquadType.Advance;
        friendlyGuns.SetDefaultBehavior(SquadType.Advance);
        friendlyGuns.direction = -1;
        friendlyGuns.ResetCommand();
        friendlyGuns.EndCutscene();

        yield return null;
    }

    public IEnumerator Charge() {
        friendlyPike.squadType = SquadType.Advance;
        friendlyPike.SetDefaultBehavior(SquadType.Advance);

        enemyPike.squadType = SquadType.Advance;
        enemyPike.SetDefaultBehavior(SquadType.Advance);

        enemyGuns.squadType = SquadType.Advance;
        enemyGuns.SetDefaultBehavior(SquadType.Advance);

        yield return null;
    }

    public IEnumerator ChargeDialogue() {
        chargeDialogue.TriggerDialogue();
        yield return null;
    }

    public IEnumerator FirstEnemyDead() {
        firstEnemiesDead++;
        yield return null;
    }

    public IEnumerator EnemyPikeDefeated() {
        firstFightDone.TriggerDialogue();
        hasDoneEnemyPikeDefeat = true;
        yield return null;
    }


    int treeDead = 0;
    public IEnumerator TreeDeath() {
        treeDead++;
        if (treeDead > 20)
            StartCoroutine("EndPatrol");
        yield return null;
    }

    bool hasPatroled = false;
    bool donePatrol = false;
    public IEnumerator SquadsPatrol() {

        if (!hasPatroled && friendlyPike) {
            friendlyPike.patrolArea = patrolArea;
            friendlyPike.SetDefaultBehavior(SquadType.Patrol);
            friendlyPike.squadType = SquadType.Patrol;
        }

        if (!hasPatroled) {
            foreach (Squad squad in mainSpawner.ActiveSquads) {
                squad.patrolArea = patrolArea;
                squad.SetDefaultBehavior(SquadType.Patrol);
                squad.squadType = SquadType.Patrol;
            }
        }


        while (!donePatrol) {
            if (!hasPatroled && mainSpawner.NewestSquad) {
                mainSpawner.NewestSquad.patrolArea = patrolArea;
                mainSpawner.NewestSquad.SetDefaultBehavior(SquadType.Patrol);
                mainSpawner.NewestSquad.squadType = SquadType.Patrol;
            }
            yield return null;
        }
    }

    bool endedPatrol = false;
    public IEnumerator EndPatrol() {
        if (!endedPatrol) {
            endedPatrol = true;

            treesClearedDialogue.TriggerDialogue();

            donePatrol = true;
            Debug.Log("END PATORL AGOPIAEHGIEOPAIHGEIOP");
            foreach (Squad squad in mainSpawner.ActiveSquads) {
                squad.SetDefaultBehavior(SquadType.Advance);
                squad.squadType = SquadType.Advance;
            }
            /*
            for (int i=0; i<mainSpawner.ActiveSquads.Count / 2; i++) {
                mainSpawner.ActiveSquads[i].SetDefaultBehavior(SquadType.Advance);
                mainSpawner.ActiveSquads[i].squadType = SquadType.Advance;
            }
            */
            StartCoroutine("AttackDoor");
        }
        yield return null;
    }

    public IEnumerator AttackDoor() {
        if (friendlyPike) {
            friendlyPike.Attack(door);
        }

        foreach (Squad squad in mainSpawner.ActiveSquads) {
            squad.Attack(door);
        }

        yield return null;
    }

    public IEnumerator DeadEnemy() {
        deadEnemies++;
        yield return null;
    }

    public IEnumerator WIN() {
        SceneManager.LoadScene("WIN");
        yield return null;
    }

    public IEnumerator FocusPoint1() {
        CameraFollow follow = cam.GetComponent<CameraFollow>();

        follow.followPoint = camPoints.CameraPoint(0);
        yield return null;
    }
}
