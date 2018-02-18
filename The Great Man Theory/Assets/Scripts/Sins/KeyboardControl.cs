using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour {

    bool evil = true;

	// Use this for initialization
	void Start () {
		if (evil) {
            Debug.Log("WHY ARE YOU DOING THIS PLEASE STOP");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (evil)
            Debug.Log("AAAAAAAAAAAAAAAA");

        Vector2 mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position = Vector2.MoveTowards(transform.position, mov, 10f);
	}
}
