using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseControl : MonoBehaviour {

    public FollowPointer head;
    FollowPointer bodyPointer;

    Vector2 target;

    Rigidbody2D body;

    Camera cam;

    float veloc = 0f;
    float giddup = 30f;
    float maxVeloc = 60f;
    float velocReduce = 120f;

    bool mouseDown = false;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        bodyPointer = GetComponent<FollowPointer>();
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        if (!mouseDown)
            SetForces();
    }

    void GetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1)) {
            mouseDown = true;
        }
        else
            mouseDown = false;
    }

    void OldGetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Space)) {
            veloc += giddup;
        }
    }

    void SetForces() {

        // head.TargetPos = new Vector2((target - head.ForcePoint).x, 0f);
        Vector2 localHeadForce = transform.InverseTransformVector(target - head.ForcePoint);
        localHeadForce = new Vector2(localHeadForce.x, 0f);
        head.TargetPos = transform.TransformVector(localHeadForce);
        head.Forces();

        /*
        head.TargetPos = (target - head.ForcePoint);
        Vector2 forwards = body.transform.up;
        Debug.Log(forwards);
        head.TargetPos = Vector2.Scale(head.TargetPos, new Vector2(forwards.y, 0f));
        head.Forces();
        */
        if (veloc > 0f) {
            bodyPointer.TargetPos = transform.up * veloc;
            bodyPointer.Forces();
            veloc -= velocReduce * Time.deltaTime;
            Debug.Log("Veloc: " + veloc.ToString());
        }
        else if (veloc > maxVeloc) {
            veloc = maxVeloc;
        }

    }
}
