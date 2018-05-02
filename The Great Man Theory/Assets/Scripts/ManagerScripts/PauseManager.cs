using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    public static bool IsPaused = false;

    public GameObject pausePanel;

    public LevelSelectScript ls;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            if (IsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
	}


    public void Pause() {
        if (GameManager.state != GameState.Paused) {
            pausePanel.SetActive(true);
            GameManager.state = GameState.Paused;
            IsPaused = true;
            //AudioManager.Instance.
        }
    }

    public void Resume() {
            pausePanel.SetActive(false);
            GameManager.state = GameState.Gameplay;
            IsPaused = false;
            //UNDO LOW PASS FILTER
    }

    public void Restart() {
        ls.Restart();
    }

    public void LoadMenu() {
        ls.MainMenu();
    }
}
