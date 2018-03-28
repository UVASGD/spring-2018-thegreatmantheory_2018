using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    private static bool created = false;

    void Awake() {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    public void Run(string coroutine) {
        Debug.Log("InRun");
        StartCoroutine(coroutine);
    }


    IEnumerator Blah() {
        Debug.Log("I am a coroutine");
        yield return null;
    }

    IEnumerator Boop() {
        Debug.Log("BOOP BOOP BOOP");
        yield return null;
    }
}
