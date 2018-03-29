using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public void Run(string coroutine) {
        StartCoroutine(coroutine);
    }
}
