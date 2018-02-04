using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TRASH THIS SHIT
public delegate void gameEvent();

public class LevelManager : MonoBehaviour {
    
    public List<IHasObjective> objectiveHolders = new List<IHasObjective>();
    public Dictionary<string, gameEvent> objectiveHandler = new Dictionary<string, gameEvent>();

    public Animation transitionScreen;
    private CameraFollow cF;

    void Start() {
        //Normally read this information in, but for now it'll just be set here
        foreach (GameObject division in GameObject.FindGameObjectsWithTag("Division")) {
            objectiveHolders.Add(division.GetComponent<SquadScript>());
        }

        objectiveHandler.Add("GoodGuysDEAD", new gameEvent(RestartLevel));
        objectiveHandler.Add("BadGuysDEAD", new gameEvent(NextLevel));
    }

    void Update() {
        for (int i = 0; i < objectiveHolders.Count; i++) {
            IHasObjective obj = objectiveHolders[i];
            CheckHandler(obj.GetObjectiveState());
            if (obj.ObjectiveFinished()) {
                objectiveHolders.Remove(obj);
            }
        }
    }

    void CheckHandler(string objectiveState) {
        if (objectiveHandler.ContainsKey(objectiveState)) {
            gameEvent gE = objectiveHandler[objectiveState];
            Debug.Log(objectiveState);
            objectiveHandler.Remove(objectiveState);
            gE();
        }
    }

    public void NextLevel() {
        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex) {
            StartCoroutine(EndLevel(SceneManager.GetActiveScene().buildIndex+1));
        }
    }

    public void RestartLevel() {
        StartCoroutine(EndLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public IEnumerator EndLevel(int levelIndex) {
        transitionScreen.Play();
        while (transitionScreen.isPlaying) {
            float size = Camera.main.orthographicSize;
            Camera.main.orthographicSize = Mathf.Lerp(size, 2, 0.005f);
            yield return null;
        }
        SceneManager.LoadScene(levelIndex);
    }
}
