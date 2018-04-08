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

    public Squad boundingEnemies;
    Transform playerTransform;

    public Transform focusPoint;

    CameraPoints camPoints;

    Camera cam;

    int deadEnemies = 0;

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
        Debug.Log("PAUSED");
        GameManager.state = GameState.Paused;
        yield return null;
    }

    public IEnumerator Gameplay() {
        Debug.Log("GAMEPLAY");
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

    public IEnumerator EnemyPikeDefeated() {

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
