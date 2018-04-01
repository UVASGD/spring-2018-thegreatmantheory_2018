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
        GameManager.state = GameState.Paused;
        yield return null;
    }

    public IEnumerator Gameplay() {
        GameManager.state = GameState.Gameplay;
        yield return null;
    }

    public IEnumerator StopFriendlyGuns() {
        // friendlyGuns.Command = delegate () { friendlyGuns.Halt(); };
        if (friendlyGuns.squadType != SquadType.FiringLine) {
            Debug.Log("Before Behavior Set");
            friendlyGuns.SetDefaultBehavior(SquadType.FiringLine);
            Debug.Log("After Behavior Set");
        }

        // if (enemyGuns.squadType != SquadType.FiringLine)
        //    enemyGuns.SetDefaultBehavior(SquadType.FiringLine);
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
