using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseControl : MonoBehaviour {

    public FollowPointer head;
    FollowPointer bodyPointer;

    Vector2 target;

    public bool player = false;
    public BasicBot rider = null;

    //Rigidbody2D body;

    Camera cam;

    float veloc = 0f;
    float giddup = 30f;
    float maxVeloc = 60f;
    float velocReduce = 120f;

    bool mouseDown = false;

    void Start () {
        //body = GetComponent<Rigidbody2D>();
        bodyPointer = GetComponent<FollowPointer>();
        cam = Camera.main;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), rider.GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        if (!mouseDown)
            SetForces();
    }

    void GetInput() {
        if (player) {
            target = cam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButton(1)) {
                mouseDown = true;
            }
            else
                mouseDown = false;
        }
        else if (rider) {
            target = rider.target;
        }
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
        Vector2 localBodyForce = transform.InverseTransformVector(target - bodyPointer.ForcePoint);
        localBodyForce = new Vector2(0f, localHeadForce.y);
        bodyPointer.TargetPos = transform.TransformVector(localHeadForce);
        bodyPointer.Forces();
        */
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Body")) {
            Body body = collision.collider.GetComponent<Body>();
            if (body.team != GetComponent<Body>().team) {
                ContactPoint2D[] contactPoints = new ContactPoint2D[1];
                collision.GetContacts(contactPoints);
                ContactPoint2D contact = contactPoints[0];
                Vector2 contactPoint = contact.point;

                collision.collider.GetComponent<Body>().Hit(collision.relativeVelocity.magnitude, contactPoint);
            }
        }
    }
}
