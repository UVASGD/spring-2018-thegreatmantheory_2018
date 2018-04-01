using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { end = -1, off = 0, on = 1, start = 2 }

public class MouseControl : MonoBehaviour {

    public Body body;
    public FollowPointer pointer;

    Camera cam;
    Vector2 target;

    int holdButton = 1;
    int braceButton = 0;
    MoveState brace = MoveState.off;

    MoveState hold = MoveState.off;

    float originalDrag;
    float dashDrag;
    public float dashMax = 2f;
    float dashTimer;
    //float dashThreshold;

    MoveState dash = MoveState.off;

    Rigidbody2D rb;
    HingeJoint2D[] armJoints;
    JointAngleLimits2D[] originalLimits;
    public bool hasArms = true;

    void Update() {
        if (GameManager.state == GameState.Gameplay) {
            GetInput();
            //if (behavior != null)
            //behavior.Traverse();
            // Move();
            if (pointer)
                SetForces();
        }
    }

    void Start() {
        cam = Camera.main;

        rb = gameObject.GetComponent<Rigidbody2D>();
        if (!body) {
            body = GetComponent<Body>();
        }

        originalDrag = rb.drag;
        dashDrag = originalDrag * 0.1f;


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
        //dashThreshold = dashMax * 0.75f;
    }


    void GetInput() {
        target = cam.ScreenToWorldPoint(Input.mousePosition);

        //DASHING
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            dash = MoveState.start;
            StartDash();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            dash = MoveState.end;
            EndDash();
        }
        else if (dashTimer <= 0 && dash == MoveState.on) {
            dash = MoveState.end;
            EndDash();
        }
        else if (Input.GetKey(KeyCode.LeftShift)) {
            Dash();
        }

        //BRACING
        if (Input.GetMouseButtonDown(braceButton)) {
            brace = MoveState.start;
            StartBrace();
        }
        else if (Input.GetMouseButtonUp(braceButton)) {
            brace = MoveState.end;
            EndBrace();
        }

        //HOLDING
        if (Input.GetMouseButtonDown(holdButton)) {
            hold = MoveState.start;
            StartHold();
        }
        else if (Input.GetMouseButtonUp(holdButton)) {
            hold = MoveState.end;
            EndHold();
        }
    }

    public void Dash() {
        if (dash == MoveState.on && dashTimer > 0) { //Decrement drag timer if dashing
            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer < dashMax) { //Rejuvenate drag timer
            dashTimer += Time.deltaTime * 2.5f;
        }
    }

    void StartDash() {
        originalDrag = rb.drag;
        dashDrag = rb.drag * 0.01f;
        rb.drag = dashDrag;
        dash = MoveState.on;
    }

    void EndDash() {
        rb.drag = originalDrag;
        dash = MoveState.off;
    }

    void SetForces() {
        // pointer.SetTarget(mousePos);
        pointer.TargetPos = (target - pointer.ForcePoint);
        pointer.Forces();
    }

    void StartBrace() {
        //BRACING
        rb.drag = 50000;
        brace = MoveState.on;

    }

    void EndBrace() {
        rb.drag = originalDrag;
        brace = MoveState.off;
    }

    public void Brace(bool start = true) {
        if (start && (int)brace < 1) brace = MoveState.start;
        else brace = MoveState.end;

        //BRACING
        if (brace == MoveState.start) { //Increase drag and turn on brace state
            rb.drag = 50000;
            brace = MoveState.on;
        }

        else if (brace == MoveState.end) { //Restore drag and turn off brace state
            rb.drag = originalDrag;
            brace = MoveState.off;
        }
    }

    void StartHold() {
        //Set limits on arm joints and turn on hold state
        for (int i = 0; i < 4; i++) {
            HingeJoint2D joint = armJoints[i];
            float angle = joint.jointAngle;
            JointAngleLimits2D newLimits = new JointAngleLimits2D();
            newLimits.max = angle + 5f;
            newLimits.min = angle - 5f;
            joint.limits = newLimits;
        }
        hold = MoveState.on;

    }

    void EndHold() {
        for (int i = 0; i < 4; i++) {
            HingeJoint2D joint = armJoints[i];
            float angle = joint.jointAngle;
            joint.limits = originalLimits[i];
        }
        hold = MoveState.off;
    }

    public void Hold(bool start = true) {
        if (start && (int)hold < 1) {
            hold = MoveState.start;
        }
        else {
            hold = MoveState.end;
        }


        //HOLDING
        //Set limits on arm joints and turn on hold state
        if (hold == MoveState.start) {
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                JointAngleLimits2D newLimits = new JointAngleLimits2D();
                newLimits.max = angle + 5f;
                newLimits.min = angle - 5f;
                joint.limits = newLimits;
            }
            hold = MoveState.on;
        }

        else if (hold == MoveState.end) { //Restore limits of arm joints and turn off hold state
            for (int i = 0; i < 4; i++) {
                HingeJoint2D joint = armJoints[i];
                float angle = joint.jointAngle;
                joint.limits = originalLimits[i];
            }
            hold = MoveState.off;
        }

    }
}
