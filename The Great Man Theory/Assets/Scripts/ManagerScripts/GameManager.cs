using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Paused, Gameplay, Menu, SceneTransition }

public class GameManager : MonoBehaviour {

    public Camera mainCamera;

    public static GameState state = GameState.Gameplay;

    public GameObject player;

    void Awake() {
        ManagerGetter.gm = this;
    }

	// Use this for initialization
	void Start () {
        // ManagerGetter.gm = this;
        // state = GameState.Gameplay;
	}
	
	// Update is called once per frame
	void Update () {
	}
}

public static class ManagerGetter {
    public static GameManager gm;

    public static GameManager Get() {
        return gm;
    }
}
