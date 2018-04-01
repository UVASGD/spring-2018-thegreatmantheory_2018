using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointer : MonoBehaviour {
    
    Rigidbody2D body;

    public Vector2 anchorOffset = new Vector2(0f, 2);
    Vector2 targetPos;
    Vector2 forcePoint;
    //Vector2 stabPoint;

    public int maxSpeed = 10; //This will depend on the weapon
    public int clamp = 20;
    public int multiplier = 5;
    public int CanMove = 1;

    public Vector2 ForcePoint {
        get { return body.GetRelativePoint(forcePoint); }
    }
    public Vector2 TargetPos {
        get { return targetPos; }
        set { targetPos = value; } }

    public Vector2 StabPoint {
        get { return forcePoint; }
    }

	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();

        targetPos = body.centerOfMass + anchorOffset;
        forcePoint = body.centerOfMass + anchorOffset;
        //stabPoint = body.centerOfMass - (anchorOffset / 2);
	}

    public void Forces() {
        Vector2 force = Vector2.ClampMagnitude(targetPos * multiplier, clamp) * CanMove;

        if (body.GetRelativePointVelocity(forcePoint).magnitude < maxSpeed) {
            body.AddForceAtPosition(force, ForcePoint, ForceMode2D.Impulse);
        }

    }

	public void BalanceForces(Vector2 gunBack) {
		Vector2 force = Vector2.ClampMagnitude(targetPos * multiplier, clamp) * CanMove;

		if (body.GetRelativePointVelocity(forcePoint).magnitude < maxSpeed) {
			body.AddForceAtPosition(force, ForcePoint, ForceMode2D.Impulse);
			body.AddForceAtPosition (force * -0.95f, gunBack, ForceMode2D.Impulse); 
		}
	}
}
