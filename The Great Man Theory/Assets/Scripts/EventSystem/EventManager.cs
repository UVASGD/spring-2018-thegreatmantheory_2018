using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    #region Singleton

    public static EventManager Instance;

    private void Awake() {

        Instance = this;
    }

    #endregion

    public void Run(string coroutine) {
        StartCoroutine(coroutine);
    }
}
