using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

    public FollowPointer pointer;
    Camera cam;

    bool brace = false;

    float originalDrag;
    float dashDrag;

    Rigidbody2D body;
    HingeJoint2D[] armJoints;
    JointAngleLimits2D[] originalLimits;

    int dashMax = 2;
    float dashTimer;
    float dashThreshold;
    bool dashing = false;

    int holdButton = 0;
    int braceButton = 1;

    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        originalDrag = body.drag;
        dashDrag = originalDrag * 0.1f;
        cam = Camera.main;

        HingeJoint2D[] RightArm = transform.Find("RightArm").GetComponentsInChildren<HingeJoint2D>();
        HingeJoint2D[] LeftArm = transform.Find("LeftArm").GetComponentsInChildren<HingeJoint2D>();

        armJoints = new HingeJoint2D[]
            {
            RightArm[0],
            RightArm[1],
            LeftArm[0],
            LeftArm[1]
            };

        originalLimits = new JointAngleLimits2D[]
            {
            armJoints[0].limits,
            armJoints[1].limits,
            armJoints[2].limits,
            armJoints[3].limits,
            };

        dashTimer = dashMax;
        dashThreshold = dashMax * 0.75f;
    }

    // Update is called once per frame
    void Update() {
        SetForces();
        if (!brace)
            Dash();
        Brace();
        Hold();
    }

    void SetForces() {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        // pointer.SetTarget(mousePos);
        pointer.TargetPos = (mousePos - pointer.ForcePoint); // * dashMultiplier;
        pointer.Forces();
    }

    void Dash() {
        if (Input.GetKey(KeyCode.LeftShift) && dashTimer > dashThreshold) {
            if (!dashing) {
                originalDrag = body.drag;
                dashDrag = body.drag * 0.01f;
                body.drag = dashDrag;
            }
            dashing = true;
        }
        else {
            dashing = false;
            body.drag = originalDrag;
        }

        if (dashing && dashTimer > 0) {
            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer < dashMax) {
            dashTimer += Time.deltaTime * 2;
        }
    }

    void Brace() {
        if (Input.GetMouseButtonDown(braceButton)) {
            brace = true;
            body.drag = 50000;
        }
        if (Input.GetMouseButtonUp(braceButton)) {
            brace = false;
            body.drag = originalDrag;
        }
    }

    void Hold() {
        if (Input.GetMouseButtonDown(holdButton)) {
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                JointAngleLimits2D newLimits = new JointAngleLimits2D();
                newLimits.max = angle + 5f;
                newLimits.min = angle - 5f;
                joint.limits = newLimits;
            }
        }
        if (Input.GetMouseButtonUp(holdButton)) {
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                joint.limits = originalLimits[i];
            }
        }
    }
}

