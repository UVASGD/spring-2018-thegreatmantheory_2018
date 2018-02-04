using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldFollowPointer : MonoBehaviour {

    Rigidbody2D body;
    Vector2 anchorOffset;
    Vector2 targetPos;
    Vector2 forcePoint;
    //Vector2 stabPoint;
    public int maxSpeed = 100; //This will depend on the weapon
    public int CanMove = 1;
    public float angularDrag; //This will depend on the weapon

    public Vector2 ForcePoint {
        get { return body.GetRelativePoint(forcePoint); }
    }
    public Vector2 TargetPos {
        get { return targetPos; }
        set { targetPos = value; }
    }

    public Vector2 StabPoint {
        get { return forcePoint; }
    }

    // Use this for initialization
    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();

        anchorOffset = new Vector2(0f, 2);

        targetPos = body.centerOfMass + anchorOffset;
        forcePoint = body.centerOfMass + anchorOffset;
        //stabPoint = body.centerOfMass - (anchorOffset / 2);
    }

    // Update is called once per frame
    void Update() {
        // Forces();
    }

    public void SetTarget(Vector2 newTarget) {
        targetPos = newTarget;
    }

    public void Forces() {
        Vector2 force = targetPos * 20 * CanMove;
        if (body.GetRelativePointVelocity(forcePoint).magnitude < maxSpeed) {
            body.AddForceAtPosition(force, ForcePoint);
        }
    }
}
