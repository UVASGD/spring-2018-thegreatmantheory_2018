using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTwoEvents : EventManager {
    public GameObject player;
    public GameObject arquibusPlayer;
    public Squad friendlySquad;
    Camera cam;

    void Awake() {
        cam = Camera.main;
    }


    bool playerDead = false;
    float resetCountdown = 3f;
    void Update() {
        if (playerDead) {
            resetCountdown -= Time.deltaTime;
        }
        if (resetCountdown < 0f) {
            playerDead = false;
            SceneManager.LoadSceneAsync("LevelTwo");
        }
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
        playerDead = true;
        yield return null;
    }

    public IEnumerator WIN() {
        GameManager.state = GameState.SceneTransition;
        // SceneManager.LoadScene("WIN");
        SceneManager.LoadScene("LevelThree");
        yield return null;
    }

    public IEnumerator EquipGun() {

        Body playerBod = player.GetComponentInChildren<Body>();

        arquibusPlayer.transform.position = playerBod.gameObject.transform.position;
        arquibusPlayer.transform.rotation = playerBod.gameObject.transform.rotation;

        player.SetActive(false);
        arquibusPlayer.SetActive(true);

        float damage = playerBod.maxHealth - playerBod.Health;

        Body arqBod = arquibusPlayer.GetComponentInChildren<Body>();
        arqBod.Setup();
        arqBod.Damage(damage);

        friendlySquad.target = arqBod.gameObject;
        friendlySquad.TargetCommand(arqBod.gameObject);

        DeathTrigger dt = player.GetComponentInChildren<DeathTrigger>();
        dt.dewit = false;

        Destroy(dt);

        Destroy(player);

        CameraFollow cf = cam.GetComponent<CameraFollow>();
        cf.followPoint = arqBod.gameObject.transform;

        yield return null;
    }
}
