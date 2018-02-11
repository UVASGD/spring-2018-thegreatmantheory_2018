using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

    public BehaviorTree behavior;
    public float actMax = 0.05f;
    float actTimer = 0;

    // Use this for initialization
    void Start () {
        behavior = new MeleeBehavior();
	}
	
	// Update is called once per frame
	void Update () {
        if (actTimer > 0) {
            actTimer -= Time.deltaTime;
        }
        else {
            actTimer = actMax;
            behavior.Traverse();
        }
	}
}
