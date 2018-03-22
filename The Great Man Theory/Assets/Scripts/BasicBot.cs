using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBot : MonoBehaviour {

	DefaultTree maintree;

	public Body body;
    public FollowPointer pointer;

	public Flag flag;

    public Transform attackTarget; //Current Attack Target

	protected List<Command> commandlist;

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

    // Use this for initialization
    void Start () {
		maintree = new DefaultTree (body, flag);
		commandlist = new List<Command> ();

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
        dashThreshold = dashMax * 0.75f;
    }
	
	// Update is called once per frame
	void Update () {
		maintree.Traverse ();
        cull();
        SetMoveState();
	}

	public void cull() {
		for (int i = commandlist.Count; i >= 0; i--) {
			commandlist [i].timeLeft -= Time.deltaTime;
			if (commandlist [i].timeLeft <= 0) {
				commandlist [i].subtree.expired = true;
				commandlist.RemoveAt (i);
			}
		}
	}

	public void command(Command comm, int priority) {
		commandlist.Add (comm);
		maintree.insertAtPriority (comm, priority);
	}

    public void Move(Vector2 target) {
        if (!pointer) pointer = body.weapon.pointer;
        pointer.TargetPos = (target - pointer.ForcePoint);
        pointer.Forces();
    }

    void SetMoveState() {
        //DASHING
        if ((int)dash > 0) { //Reduce drag and turn on drag state
            if ((int)dash > 1) {
                originalDrag = rb.drag;
                dashDrag = rb.drag * 0.01f;
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
        if ((int)brace > 0) { //Increase drag and turn on brace state
            if ((int)brace > 1) {
                rb.drag = 50000;
                brace = MoveState.on;
            }
        }

        else if ((int)brace < 1) { //Restore drag and turn off brace state
            if ((int)brace < 0) {
                rb.drag = originalDrag;
                brace = MoveState.off;
            }
        }

        //HOLDING
        if ((int)hold > 0) { //Set limits on arm joints and turn on hold state
            if ((int)hold > 1) {
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
        }

        else if ((int)hold < 1) { //Restore limits of arm joints and turn off hold state
            if ((int)hold < 0) {
                for (int i = 0; i < 4; i++) {
                    HingeJoint2D joint = armJoints[i];
                    float angle = joint.jointAngle;
                    joint.limits = originalLimits[i];
                }
                hold = MoveState.off;
            }
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
            rb.freezeRotation = true;
            hold = MoveState.start;
        }
        else {
            rb.freezeRotation = false;
            hold = MoveState.end;
        }
    }
}

public class DefaultTree {
	protected Node rootNode;

	List<Node> priorityBuckets;

	public DefaultTree(Body body, Flag flag) {

		priorityBuckets = new List<Node> () {
			new Selector("priority 0", new List<Node>() {}),
			new Selector("priority 1", new List<Node>() {
				//Wounded node here
			}),
			new Selector("priority 2", new List<Node>() {
				//Default Attack goes here
			}),
			new Selector("priority 3", new List<Node>() {}),
			new Selector("priority 4", new List<Node>() {
				//Idle node here
			})
		};

		rootNode = new Selector("root", (priorityBuckets));
	}

	public NodeState Traverse() {
		return rootNode.GetState ();
	}

	public void insertAtPriority(Command comm, int priority) {
		priority = Mathf.Clamp (priority, 0, priorityBuckets.Count - 1);
		((Selector)(priorityBuckets [priority])).insertChild (comm.subtree);
	}
}
