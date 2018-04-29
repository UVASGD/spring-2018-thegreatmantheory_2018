using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeJointAdder : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Go();
	}

    public void Go() {
        HingeJoint2D[] joints = GetComponentsInChildren<HingeJoint2D>();

        for (int i = 0; i < joints.Length; i++) {
            GameObject parentRef = joints[i].gameObject;

            Destroy(joints[i]);

            HingeJoint2D copy = parentRef.AddComponent<HingeJoint2D>();
            HingeDeepCopy(copy, joints[i]);
        }
    }

    static void HingeDeepCopy(HingeJoint2D copy, HingeJoint2D orig) {
        copy.connectedBody = orig.connectedBody;
        copy.anchor = new Vector2(orig.anchor.x, orig.anchor.y);
        copy.connectedAnchor = new Vector2(orig.connectedAnchor.x, orig.connectedAnchor.y);
        copy.enableCollision = orig.enableCollision;
        copy.useLimits = orig.useLimits;
        if (copy.useLimits) {
            JointAngleLimits2D e = new JointAngleLimits2D();
            e.min = orig.limits.min;
            e.max = orig.limits.max;
            copy.limits = e;
        }
    }
}
