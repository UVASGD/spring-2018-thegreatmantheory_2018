using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonsenseWiggles : MonoBehaviour {

    public float minRange;
    public float maxRange;

    Rigidbody2D body;
    GameManager gm;
    Vector2 anchorOffset = new Vector2();
    Vector2 targetPos;
    Vector2 forcePoint;

    // public GameObject showpoint;

    // Use this for initialization
    void Start() {
        body = GetComponentInParent<Rigidbody2D>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        anchorOffset += new Vector2(0f, gm.offset);
        targetPos = body.centerOfMass + anchorOffset;
        forcePoint = body.centerOfMass + anchorOffset;
    }

    // Update is called once per frame
    void Update() {
        Forces();
    }

    void Forces() {

        Vector2 randPos = RandPos();
        // Vector2 objPos = gameObject.transform.position;
        targetPos += randPos;

        if (targetPos.magnitude > gm.wigglemax) {
            //  keep dat shit in check
            targetPos = targetPos.normalized * gm.wigglemax;
        }

        // Vector2 forcePoint = body.GetRelativePoint(body.worldCenterOfMass + anchorOffset);

        // Debug.Log(forcePoint);
        // showpoint.transform.position = forcePoint;
        // Vector2 force = (targetPos - forcePoint) * gm.extraForce;
        // force.Normalize();
        // force *= gm.maxForce;
        Vector2 force = targetPos * gm.extraForce;
        body.AddForceAtPosition(force, forcePoint);
    }

    Vector2 RandPos() {
        Vector2 direct = Random.insideUnitCircle;
        float random = Random.Range(0, maxRange);
        return direct * random;
    }
}
