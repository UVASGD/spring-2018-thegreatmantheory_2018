using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum MoveState { end = -1, off = 0, on = 1, start = 2 }

public class BasicBot : MonoBehaviour {

	DefaultTree maintree;

    Rigidbody2D rb;
    public Body body;
    public FollowPointer pointer;

    public Vector2 target = Vector2.zero;

	public Squad squad;

    public GameObject attackTarget = null; //Current Attack Target

	protected List<Command> commandlist;

    //Variables for dashing
    float originalDrag;
    float dashDrag;
    int dashMax = 1;
    protected float dashTimer;
    float dashThreshold;
    protected MoveState dash = MoveState.off;

    //Variables for bracing
    protected int holdButton = 0;
    protected int braceButton = 1;
    protected MoveState brace = MoveState.off;

    //Variables for holding
    HingeJoint2D[] armJoints;
    JointAngleLimits2D[] originalLimits;
    public bool hasArms = true;
    protected MoveState hold = MoveState.off;

    bool ded = false;
    public bool Ded { get { return ded; } }

    // Use this for initialization
    void Start () {
        if (!body) {
            body = GetComponent<Body>();
        }

        ResetCommand();

        rb = gameObject.GetComponent<Rigidbody2D>();

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
        dashThreshold = dashMax * 0.75f;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.state == GameState.Gameplay || body.cutsceneOverride) {
            if (attackTarget == null)
                if (squad)
                    attackTarget =squad.GetEnemy();
            if (maintree != null) {
                maintree.Traverse();
                Cull();
                SetMoveState();
            }
        }
	}

	public void Cull() {
		for (int i = commandlist.Count - 1; i >= 0; i--) {
			commandlist [i].timeLeft -= Time.deltaTime;
			if (commandlist [i].timeLeft <= 0) {
				commandlist [i].subtree.expired = true;
				commandlist.RemoveAt (i);
			}
		}
	}

	public void Command(Command comm, int priority) {
        commandlist.Add (comm);
		maintree.insertAtPriority (comm.subtree, priority);
	}

    public void ResetCommand() {
        switch (body.unitType) {
            case UnitType.Pike:
                maintree = new SwordTree(this);
                break;
            case UnitType.Longsword:
            case UnitType.Sword:
                maintree = new SwordTree(this);
                break;
            case UnitType.Arquebus:
                maintree = new ArquebusTree(this);
                break;
            case UnitType.HorseSword:
                maintree = new CavalryTree(this);
                break;
        }
        commandlist = new List<Command>();
    }

    public void Move(Vector2 _target) {
        target = _target;
        if (!pointer) pointer = body.weapon.pointer;
        pointer.TargetPos = (target - pointer.ForcePoint);
        pointer.Forces();
    }

    void SetMoveState() {
        //DASHING
        if ((int)dash > 0) { //Reduce drag and turn on drag state
            if ((int)dash > 1) {
                originalDrag = rb.drag;
                dashDrag = rb.drag * 0.1f;
                rb.drag = dashDrag;
                dash = MoveState.on;
            }
        }

        else if ((int)dash < 1) { //Restore drag and turn off drag state
            if ((int)dash < 0) {
                rb.drag = originalDrag;
                dash = MoveState.off;
            }
        }

        if ((int)dash > 0 && dashTimer > 0) { //Decrement drag timer if dashing
            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer < dashMax) { //Rejuvenate drag timer
            dashTimer += Time.deltaTime * 2.5f;
        }

        //BRACING
        if (brace == MoveState.start) { //Increase drag and turn on brace state
            rb.drag = 50000;
            brace = MoveState.on;
        }

        else if (brace == MoveState.end) { //Restore drag and turn off brace state
            rb.drag = originalDrag;
            brace = MoveState.off;
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

    public void Dash() {
        if (dashTimer > dashThreshold && (int)brace < 1 && (int)dash < 1) dash = MoveState.start;
    }

    public void Brace(bool start = true) {
        if (start && (int)brace < 1) brace = MoveState.start;
        else brace = MoveState.end;
    }

    public void Hold(bool start = true) {
        if (start && (int)hold < 1) {
            //rb.freezeRotation = true;
            hold = MoveState.start;
        }
        else {
            //rb.freezeRotation = false;
            hold = MoveState.end;
        }
    }

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.CompareTag ("Weapon")) {
			Team side = other.collider.transform.parent.GetComponentInChildren<Body>().team;
			if (side != body.team) {
				attackTarget = other.collider.gameObject;
			}
		}
	}

    void OnDestroy() {
        if (squad)
            squad.UpdateMinions();    
    }
}
