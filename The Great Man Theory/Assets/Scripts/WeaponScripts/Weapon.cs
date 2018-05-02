using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    protected GameObject target;
    protected Collider2D targetColl;
    protected Rigidbody2D targetRB;

    public bool player = false;

    public GameObject bodyObj;
    public GameObject dangerObj;
    protected Body thisBody;
    public Body ThisBody { get { return thisBody; } }
    protected Collider2D thisBodyCollider;
    public Collider2D thisCollider;
    public Rigidbody2D rb;
    public FollowPointer pointer;

    public WeaponSoundbox soundbox;

    protected bool shouldCheck = false;

    public float limpThreshold = 8;
    float limpTime;
    bool limpFlag;
    bool disableFlag;

    public float DAMAGE_MULTIPLIER = 1;

    protected virtual void Start() {
        thisBodyCollider = bodyObj.GetComponent<Collider2D>();
        thisBody = bodyObj.GetComponent<Body>();
        thisCollider = GetComponent<Collider2D>();
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        if (!pointer) {
            pointer = GetComponent<FollowPointer>();
        }

        soundbox = GetComponentInChildren<WeaponSoundbox>();
    }

    protected virtual void Update() {
        if (TargetCheck()) {
            AffectMovement();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        shouldCheck = true;

        //TODO make sure elevation is roughly the same; maybe use ContactFilter2D with useDepth ???
        if (collision.collider.CompareTag("Body") && collision.collider != thisBodyCollider) {
            //Check whether collided body is the same team
            Body hitBody = collision.collider.gameObject.GetComponent<Body>();
            if (hitBody && hitBody.team != thisBody.team) {
                if (targetColl && collision.collider != targetColl)
                    Physics2D.IgnoreCollision(targetColl, thisCollider, false);
                targetColl = collision.collider;
                target = targetColl.gameObject;
                targetRB = target.GetComponent<Rigidbody2D>();

                //Determine the point of contact
                ContactPoint2D[] contactPoints = new ContactPoint2D[1];
                collision.GetContacts(contactPoints);
                ContactPoint2D contact = contactPoints[0];
                Vector2 contactPoint = contact.point;

                HitCalc(contactPoint, hitBody, collision);
            }
        }
        if (soundbox)
            soundbox.Hit(collision.relativeVelocity.magnitude / 100f);
        // soundbox.Hit(1f);
    }

    protected virtual void HitCalc(Vector2 contactPoint, Body targetBodyScript, Collision2D collision) {
        //Calculate power of attack
        float power = collision.relativeVelocity.magnitude;
        Hit(targetBodyScript, power*DAMAGE_MULTIPLIER, contactPoint, false, player);
    }

    //Anomymous methods are fun, yo
    protected delegate void MoveDel();

    protected virtual void AffectMovement() {

    //Go limp if the timer is going, stop being limp if the timer is done
    limpTime = TimerFunc(limpTime, 
            delegate() {
                if (limpFlag) {
                    limpFlag = false;
                    disableFlag = true;
                    EnableMovement(false); //EnableMovement doesn't set disableFlag true on its own, but it can set it false
                }
            },
            delegate() {
                if (disableFlag)
                    EnableMovement(true);
            });
    }

    //Simple logic to handle the timer and what to do when it's still going or done
    protected float TimerFunc(float timer, MoveDel ifGoing, MoveDel ifDone) {
        if (timer > 0) {
            timer -= Time.deltaTime;
            ifGoing();
        }
        else if (timer <= 0) {
            timer = 0;
            ifDone();
        }
        return timer;
    }

    protected void Hit(Body bodyHit, float power, Vector2 hitPoint, bool puncturing = false, bool playerHit = false) {
        //TODO Play a sound at the volume (power) or something?
        limpTime = (power > limpThreshold) ? power / 100 : 0;
        limpTime = Mathf.Clamp(limpTime, 0, 0.5f);
        limpFlag = (limpTime > 0);
        bodyHit.Hit(power, hitPoint, puncturing, playerHit);
    }

    void EnableMovement(bool canMove) {
        pointer.CanMove = (canMove) ? 1 : 0;
        disableFlag = (canMove) ? false : disableFlag;
    }

    protected virtual bool TargetCheck() {
        if (!target) {
            if (shouldCheck) {
                targetRB = null;

                limpTime = 0;
                limpFlag = false;
                if (disableFlag)
                    EnableMovement(true);

                shouldCheck = false;
            }
            return false;
        }
        return true;
    }
}
