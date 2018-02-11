using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseControl : MonoBehaviour {

    public FollowPointer pointer;

    Vector2 target;

    Rigidbody2D body;

    Camera cam;

    float veloc = 0f;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        SetForces();
	}

    void GetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            veloc += 10f;
        }
    }

    void SetForces() {
        // pointer.TargetPos = new Vector2((target - pointer.ForcePoint).x, 0f);
        Vector2 localForce = transform.InverseTransformVector(target - pointer.ForcePoint);
        localForce = new Vector2(localForce.x, veloc);
        pointer.TargetPos = transform.TransformVector(localForce);
        pointer.Forces();
        /*
        pointer.TargetPos = (target - pointer.ForcePoint);
        Vector2 forwards = body.transform.up;
        Debug.Log(forwards);
        pointer.TargetPos = Vector2.Scale(pointer.TargetPos, new Vector2(forwards.y, 0f));
        pointer.Forces();
        */
        if (veloc > 0f)
            veloc -= 0.1f;
    }
}
