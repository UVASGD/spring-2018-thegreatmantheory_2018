using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeEvents : EventManager {
    public GameObject player;
    public GameObject bomb;
    Camera cam;

    void Awake() {
        cam = Camera.main;
    }

    public IEnumerator Pause() {
        GameManager.state = GameState.Paused;
        yield return null;
    }

    public IEnumerator Gameplay() {
        GameManager.state = GameState.Gameplay;
        yield return null;
    }

    public IEnumerator PlayerDead() {
        SceneManager.LoadSceneAsync("LevelTwo");
        yield return null;
    }

    public IEnumerator WIN() {
        GameManager.state = GameState.SceneTransition;
        SceneManager.LoadScene("WIN");
        yield return null;
    }
}
