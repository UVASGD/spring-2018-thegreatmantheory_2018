using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { end = -1, off = 0, on = 1, start = 2 }

public class Mover : MonoBehaviour { 

    protected delegate void MoveDel();

    //public BehaviorTree behavior;

    public Body body;

    public FollowPointer pointer;

    protected Vector2 target;
    public Vector2 Target { get { return (targetObj) ? (Vector2)targetObj.transform.position : target; } }
    public Transform targetObj;

    float originalDrag;
    float dashDrag;
    int dashMax = 1;
    protected float dashTimer;
    float dashThreshold;
    protected MoveState dash = MoveState.off;

    protected int holdButton = 0;
    protected int braceButton = 1;
    protected MoveState brace = MoveState.off;

    protected MoveState hold = MoveState.off;

    Rigidbody2D rb;
    HingeJoint2D[] armJoints;
    JointAngleLimits2D[] originalLimits;
    public bool hasArms = true;

    void Start() {
        //targetObj = null;

        rb = gameObject.GetComponent<Rigidbody2D>();
        if (!body) {
            body = GetComponent<Body>();
        }

        originalDrag = rb.drag;
        dashDrag = originalDrag * 0.1f;

        SetMover();

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

    protected virtual void SetMover() {
    }

    void Update() {
        GetInput();
        //if (behavior != null)
            //behavior.Traverse();
        Move();
        if (pointer)
            SetForces();
    }

    protected virtual void GetInput() {
        if (!targetObj) {
            target = pointer.ForcePoint;
        }

        if (dashTimer <= 0) {
            dash = MoveState.end;
        }
    }

    void Move() {
        //DASHING
        SetMoveState(ref dash,
        delegate {
            originalDrag = rb.drag;
            dashDrag = rb.drag * 0.01f;
            rb.drag = dashDrag;
        },
        delegate { rb.drag = originalDrag; },
        delegate {
            if ((int)dash > 0 && dashTimer > 0) {
                dashTimer -= Time.deltaTime;
            }
            else if (dashTimer < dashMax) {
                dashTimer += Time.deltaTime * 2.5f;
            }
        });

        //BRACING
        SetMoveState(ref brace,
        delegate { rb.drag = 50000; },
        delegate { rb.drag = originalDrag; }
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
        pointer.TargetPos = (target - pointer.ForcePoint);
        pointer.Forces();
    }

    public void SetTarget(Vector2 _target) {
        target = _target;
    }

    public void Dash() {
        if (dashTimer > dashThreshold && (int)brace < 1 && (int)dash < 1) dash = MoveState.start;
    }

    public void Brace(bool start = true) {
        if (start && (int)brace < 1) brace = MoveState.start;
        else brace = MoveState.end;
    }

    public void Hold(bool start = true) {
        if (start && (int)hold < 1) {
            rb.freezeRotation = true;
            hold = MoveState.start;
        }
        else {
            rb.freezeRotation = false;
            hold = MoveState.end; }
    }
}