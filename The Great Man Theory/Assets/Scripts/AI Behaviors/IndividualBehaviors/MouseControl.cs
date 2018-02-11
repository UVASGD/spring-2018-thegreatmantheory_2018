using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {


    private delegate void MoveDel();
    private enum MoveState { end = -1, off = 0, on = 1, start = 2 }

    public FollowPointer pointer;
    Camera cam;

    Vector2 target;

    public bool hasArms = true;

    float originalDrag;
    float dashDrag;
    int dashMax = 1;
    float dashTimer;
    float dashThreshold;
    MoveState dash = MoveState.off;

    int holdButton = 0;
    int braceButton = 1;
    MoveState brace = MoveState.off;

    MoveState hold = MoveState.off;

    Rigidbody2D body;
    HingeJoint2D[] armJoints;
    JointAngleLimits2D[] originalLimits;

    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        originalDrag = body.drag;
        dashDrag = originalDrag * 0.1f;
        cam = Camera.main;

        if (hasArms) {
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
        }

        dashTimer = dashMax;
        dashThreshold = dashMax * 0.75f;
    }


    // Update is called once per frame
    void Update() {
        GetInput();
        Move();
        SetForces();
    }

    void GetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);

        //DASHING
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer > dashThreshold && (int)brace < 1) {
            dash = MoveState.start;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            dash = MoveState.end;
        }
        else if (dashTimer <= 0) {
            dash = MoveState.end;
        }

        //BRACING
        if (Input.GetMouseButtonDown(braceButton)) {
            brace = MoveState.start;
        }
        else if (Input.GetMouseButtonUp(braceButton)) {
            brace = MoveState.end;
        }

        //HOLDING
        if (Input.GetMouseButtonDown(holdButton) && hasArms) {
            hold = MoveState.start;
        }
        else if (Input.GetMouseButtonUp(holdButton) && hasArms) {
            hold = MoveState.end;
        }
    }

    void Move() {
        //DASHING
        SetMoveState(ref dash,
        delegate {
            originalDrag = body.drag;
            dashDrag = body.drag * 0.01f;
            body.drag = dashDrag;
        },
        delegate { body.drag = originalDrag; },
        delegate {
            if ((int)dash > 0 && dashTimer > 0) {
                dashTimer -= Time.deltaTime;
            }
            else if (dashTimer < dashMax) {
                dashTimer += Time.deltaTime * 2;
            }
        });

        //BRACING
        SetMoveState(ref brace,
        delegate { body.drag = 50000; },
        delegate { body.drag = originalDrag; }
        );

        //HOLDING
        SetMoveState(ref hold,
        delegate {
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                JointAngleLimits2D newLimits = new JointAngleLimits2D();
                newLimits.max = angle + 5f;
                newLimits.min = angle - 5f;
                joint.limits = newLimits;
            }
        },
        delegate {
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                joint.limits = originalLimits[i];
            }
        }
        );
    }

    void SetMoveState(ref MoveState moveState, MoveDel StartDel, MoveDel EndDel, MoveDel WhileDel = null) {
        int state = (int)moveState;
        if (state > 0) {
            if (state > 1) {
                StartDel();
                moveState = MoveState.on;
            }
        }
        else if (state < 1) {
            if (state < 0) {
                EndDel();
                moveState = MoveState.off;
            }
        }

        if (WhileDel != null) {
            WhileDel();
        }
    }

    void SetForces() {
        // pointer.SetTarget(mousePos);
        pointer.TargetPos = (target - pointer.ForcePoint); // * dashMultiplier;
        pointer.Forces();
    }
}


