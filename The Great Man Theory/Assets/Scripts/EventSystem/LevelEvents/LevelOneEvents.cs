using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start() {
        cam = Camera.main;
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

    public IEnumerator StopFriendlyGuns() {
        // friendlyGuns.Command = delegate () { friendlyGuns.Halt(); };
        if (friendlyGuns.squadType != SquadType.FiringLine)
            friendlyGuns.SetDefaultBehavior(SquadType.FiringLine);

        if (enemyGuns.squadType != SquadType.FiringLine)
            enemyGuns.SetDefaultBehavior(SquadType.FiringLine);
        yield return null;
    }
}
