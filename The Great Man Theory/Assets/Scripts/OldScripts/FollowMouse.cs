using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    Vector2 anchorOffset = new Vector2();
    Rigidbody2D body;
    GameManager gm;

	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        anchorOffset += new Vector2(0f, gm.offset);
    }

    // Update is called once per frame
    void Update () {
        Forces();
	}

    void Forces() {
        // Debug.Log("Geddit" + ManagerGetter.Get());
        Camera cam = gm.mainCamera;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 forcePoint = body.GetRelativePoint(body.centerOfMass + anchorOffset);
        Debug.Log("FollowMouse ForcePoint: " + forcePoint);
        Vector2 force = (mousePos - forcePoint) * gm.extraForce;
        // force.Normalize();
        // force *= gm.maxForce;
        // Debug.Log(force.magnitude);
        body.AddForceAtPosition(force, forcePoint);
    }
}
