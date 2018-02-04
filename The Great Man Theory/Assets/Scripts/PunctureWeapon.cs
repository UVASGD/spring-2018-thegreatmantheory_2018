using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunctureWeapon : Weapon {

    //TODO Separate this into PunctureWeapon
    FixedJoint2D stickPoint;
    Vector2 stabPoint;

    float stabTime;
    bool stabFlag;

    float stuckTime;
    bool stuckFlag;

    public float sharpness;
    public float range;
    public float midPoint;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        if (pointer)
            stabPoint = pointer.StabPoint;
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        shouldCheck = true;

        if (collision.collider.CompareTag("Body") && collision.collider != thisBodyCollider) {
            //TODO make sure elevation is roughly the same & the target isn't the same team
            //Maybe use ContactFilter2D with useDepth ???

            if (targetColl && (collision.collider != targetColl || !stabFlag))
                Physics2D.IgnoreCollision(targetColl, thisCollider, false);
            targetColl = collision.collider;
            target = targetColl.gameObject;
            targetRB = target.GetComponent<Rigidbody2D>();

            //Calculate power of attack
            float force = collision.relativeVelocity.magnitude;
            float punctureForce = CheckPuncture(); //Maybe just calculate force in CheckPuncture
            float power = punctureForce * force;

            //Determine the point of contact
            ContactPoint2D[] contactPoints = new ContactPoint2D[1];
            collision.GetContacts(contactPoints);
            ContactPoint2D contact = contactPoints[0];
            Vector2 contactPoint = contact.point;

            //Get the body's script and deal the damage
            Body targetBodyScript = target.GetComponent<Body>();
            Hit(targetBodyScript, power, contactPoint, player);

            //Do we puncture, yo?
            if (target && punctureForce > 1) {
                float resistance = targetBodyScript.punctureResist;
                if (power > resistance) {
                    Physics2D.IgnoreCollision(targetColl, thisCollider);
                    StartStab();
                }
            }
        }
    }

    protected override void AffectMovement() {
        base.AffectMovement();

        //While the timer is going, yoink into the enemy's bod. when the timer is done, stickem!
        stabTime = TimerFunc(stabTime,
            delegate () {
                Vector2 weaponForce = (target.transform.position - transform.TransformPoint(stabPoint)).normalized * 300;
                //TODO Maybe make that 300 a variable?
                rb.AddForceAtPosition((weaponForce), transform.TransformPoint(stabPoint));
            },
            delegate () {
                if (stabFlag) {
                    stabFlag = false;
                    if (targetColl.Distance(thisCollider).isOverlapped) {
                        Stab(targetRB);
                    }
                    else Physics2D.IgnoreCollision(targetColl, thisCollider, false);
                }
            });

        //While the timer is going, just stay stuck. when the timer is done, loosen the fixed joint
        stuckTime = TimerFunc(stuckTime,
            delegate () {
            },
            delegate () {
                if (stuckFlag) {
                    stuckFlag = false;
                    if (stickPoint)
                        stickPoint.breakForce = 400; //TODO make this a variable?
                }
            });
    }

    void OnJointBreak2D(Joint2D joint) {
        Physics2D.IgnoreCollision(targetColl, thisCollider, false);
    }

    void StartStab() {
        stabTime = 0.2f; //TODO Might vary / Make this a variable
        stabFlag = true;
        rb.velocity *= 0;
        rb.angularVelocity *= 0;
    }

    void Stab(Rigidbody2D targetBody) {
        Destroy(stickPoint);
        stickPoint = gameObject.AddComponent<FixedJoint2D>();
        stickPoint.connectedBody = targetColl.attachedRigidbody;
        stuckTime = 2; //TODO Might vary / Make this a variable
        stuckFlag = true;
    }

    float CheckPuncture() {
        //return a max of (sharpness), min of 1
        float diff = Mathf.Abs(Vector2.SignedAngle(Vector2.up, transform.InverseTransformDirection(rb.velocity).normalized) - midPoint);
        float result = 1;
        if (diff < range) {
            result = (range - diff) / range * sharpness;
        }
        return Mathf.Clamp(result, 1, sharpness);
    }

    protected override bool TargetCheck() {
        if (!base.TargetCheck()) {
            Destroy(stickPoint);
            stabTime = 0;
            stabFlag = false;

            stuckTime = 0;
            stuckFlag = false;
            return false;
        }
        return true;
    }
}
