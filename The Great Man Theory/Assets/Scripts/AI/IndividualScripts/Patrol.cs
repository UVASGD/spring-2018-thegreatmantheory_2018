using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    float timer;
    public float maxTimer = 10;
    public Collider2D area;

	// Use this for initialization
	void Start () {
        timer = maxTimer;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = maxTimer;

        }
	}
}
