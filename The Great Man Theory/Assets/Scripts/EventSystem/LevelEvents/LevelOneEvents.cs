using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOneEvents : EventManager {

    public Squad friendlyGuns;
    public Squad enemyGuns;

    public Transform enemyWaypoint;
    public Transform frinedlyWaypoint;

    public DialogueTrigger focusText;

    public Squad boundingEnemies;
    public Transform playerTransform;

    public Transform focusPoint;

    Camera cam;

    int deadEnemies = 0;

    void Start() {
        cam = Camera.main;
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

    public IEnumerator StopFriendlyGuns() {

        Debug.Log("AAAAAAAAA");

        friendlyGuns.squadType = SquadType.Advance;
        friendlyGuns.SetDefaultBehavior(SquadType.Advance);
        friendlyGuns.direction = -1;

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
}
