using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneEvents : EventManager {

    public DialogueTrigger focusText;

    public Squad boundingEnemies;
    public Transform playerTransform;

    public Transform focusPoint;

    Camera cam;

    void Start() {
        cam = Camera.main;
    }

	public IEnumerator BoundingEnemiesTarget() {

        Debug.Log("Getim");

        List<Node> getimChildren = new List<Node> {
            new IntervalGate(100f),
            new MoveTargetCommand(boundingEnemies, 0, 100f, playerTransform)
        };

        Node getim = new Sequencer("Attack Dude", getimChildren);
        Command attackTarget = new Command(getim, 1f);

        boundingEnemies.Command(attackTarget, 0);

        yield return null;
    }

    public IEnumerator FocusOnEnemies() {
        GameManager.state = GameState.Paused;
        CameraFollow follow = cam.GetComponent<CameraFollow>();

        follow.followPoint = focusPoint;

        focusText.TriggerDialogue();

        yield return null;
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
}
