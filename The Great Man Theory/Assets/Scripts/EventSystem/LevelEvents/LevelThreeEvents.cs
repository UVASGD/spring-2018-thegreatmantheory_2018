using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeEvents : EventManager {
    public GameObject playerContainer;
    public Body playerBody;
    public GameObject bomb;

    public GameObject horsePlayer;
    public GameObject horseArt;
    public Transform horseTrigger;

    public SeeYouOnTheOtherSide rolfelerikles;
    public BombSpawn armageddonBox;
    public BombSpawn moreBox;

    public SpriteRenderer box;

    public Squad firingLine;

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
        //SceneManager.LoadScene("LevelThree");
        yield return null;
    }

    public IEnumerator WIN() {
        GameManager.state = GameState.SceneTransition;
        SceneManager.LoadScene("WIN");
        yield return null;
    }

    public IEnumerator Bomba() {
        Instantiate(bomb);
        yield return null;
    }

    public IEnumerator Mount() {

        GameObject dude = Instantiate(horsePlayer, horseTrigger.position, horseTrigger.rotation);

        // float damage = playerBody.maxHealth - playerBody.Health;

        // Body horsemanBody = horseman.GetComponentInChildren<Body>();
        // horsemanBody.Setup();

        CameraFollow cf = cam.GetComponent<CameraFollow>();
        ThisIsBod boddo = dude.GetComponent<ThisIsBod>();
        Body someBody = boddo.bod;
        cf.followPoint = someBody.gameObject.transform;

        rolfelerikles.player = boddo.gameObject.transform;

        playerContainer.SetActive(false);

        Destroy(horseArt);

        // horsemanBody.Damage(damage);

        yield return null;
    }

    public IEnumerator Armageddon() {
        Debug.Log("BANG BANG BOOM");
        armageddonBox.enabled = true;
        moreBox.enabled = true;

        float wait = 0f;
        while(wait < 10f) {
            wait += Time.deltaTime;
            if (wait < 8f) {
                float alpha = wait / 8;
                box.color = new Color(box.color.r, box.color.g, box.color.b, alpha);
            }
            else
                box.color = Color.white;
            yield return null;
        }
        SceneManager.LoadScene("WildAndFree");
        GameManager.state = GameState.Gameplay;
    }

    public IEnumerator FiringLine() {
        firingLine.squadType = SquadType.FiringLine;
        firingLine.SetDefaultBehavior(SquadType.FiringLine);
        firingLine.FiringLine();
        yield return null;
    }
}
