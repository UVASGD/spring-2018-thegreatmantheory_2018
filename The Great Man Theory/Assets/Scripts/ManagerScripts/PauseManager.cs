using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour {

    public static bool IsPaused = false;

    public GameObject pausePanel;

    public LevelSelectScript ls;

    public AudioMixer am;

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
            am.SetFloat("lowpass", 1000f);
        }
    }

    public void Resume() {
            pausePanel.SetActive(false);
            GameManager.state = GameState.Gameplay;
            IsPaused = false;
            am.SetFloat("lowpass", 22000f);
    }

    public void Restart() {
        IsPaused = false;
        am.SetFloat("lowpass", 22000f);
        ls.Restart();
    }

    public void LoadMenu() {
        IsPaused = false;
        am.SetFloat("lowpass", 22000f);
        GameManager.state = GameState.Menu;
        ls.MainMenu();
    }
}
