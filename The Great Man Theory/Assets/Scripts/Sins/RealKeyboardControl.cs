using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealKeyboardControl : MonoBehaviour {

    bool evil = true;

    Rigidbody2D body;

    // Use this for initialization
    void Start() {
        if (evil) {
            Debug.Log("WHY ARE YOU DOING THIS PLEASE STOP");
        }
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (evil)
            Debug.Log("AAAAAAAAAAAAAAAA");

        Vector2 mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.AddForce(mov * 5000);
    }
}
