using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunctureWeapon : Weapon {

    FixedJoint2D stickPoint;
    Vector2 stabPoint;

    float maxStab = 0.2f;
    float stabTime;
    bool stabFlag;

    public float maxStuck = 0.5f;
    float stuckTime;
    bool stuckFlag;

    public Collider2D stabArea;
    public float sharpness;
    public float range;
    public float midPoint;

    int aimAssist = 400; //The force with which the weapon yoinks into the body upon stabbing
    public int breakForce = 400; //The force required for the fixedjoint in the body to be broken

    // Use this for initialization
    protected override void Start () {
        base.Start();
        if (pointer)
            stabPoint = pointer.StabPoint;
    }

    protected override void AffectMovement() {
        base.AffectMovement();

        //While the timer is going, yoink into the enemy's bod. when the timer is done, stickem!
        stabTime = TimerFunc(stabTime,
            delegate () {
                Vector2 weaponForce = (target.transform.position - transform.TransformPoint(stabPoint)).normalized * aimAssist;
                rb.AddForceAtPosition((weaponForce), transform.TransformPoint(stabPoint));
            },
            delegate () {
                if (stabFlag) {
                    stabFlag = false;
                    if (targetColl.Distance(thisCollider).isOverlapped) {
                        Stab(targetRB);
                    }
                    else {
                        Physics2D.IgnoreCollision(targetColl, thisCollider, false);
                    }
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
                        stickPoint.breakForce = breakForce;
                }
            });
    }

    protected override void HitCalc(Vector2 contactPoint, Body targetBodyScript, Collision2D collision) {
            //Calculate power of attack
        float force = collision.relativeVelocity.magnitude;
        if (!stabArea || targetColl.Distance(stabArea).distance < 1) {
            float punctureForce = CheckPuncture();
            float power = force + punctureForce;
            Hit(targetBodyScript, power, contactPoint, (punctureForce > 1), player);
            //Do we puncture, yo?
            if (target && punctureForce > 1) {
                float resistance = targetBodyScript.punctureResist;
                if (power > resistance) {
                    Physics2D.IgnoreCollision(targetColl, thisCollider);
                    StartStab();
                }
            }
        }
        else { Hit(targetBodyScript, force/4, contactPoint, false, player); }
    }

    void OnJointBreak2D(Joint2D joint) {
        Physics2D.IgnoreCollision(targetColl, thisCollider, false);
    }

    void StartStab() {
        stabTime = maxStab;
        stabFlag = true;
        rb.velocity *= 0;
        rb.angularVelocity *= 0;
    }

    void Stab(Rigidbody2D targetBody) {
        Destroy(stickPoint);
        stickPoint = gameObject.AddComponent<FixedJoint2D>();
        stickPoint.connectedBody = targetColl.attachedRigidbody;
        stuckTime = maxStuck;
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
