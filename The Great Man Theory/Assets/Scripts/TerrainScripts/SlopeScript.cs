using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeScript : MonoBehaviour {
    public float elevation = 2.5f;

    private static float forceConstant = 50;

    public Collider2D slopeZone;

    public PlateauScript plateau;

	// Use this for initialization
	void Start () {
        if (!slopeZone)
            slopeZone = GetComponent<Collider2D>();

        if (!plateau)
            plateau = transform.parent.GetComponentInChildren<PlateauScript>();
	}

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("Body") && collider.attachedRigidbody.velocity.magnitude > 1f) {
            ColliderDistance2D entrantDist = plateau.plateauZone.Distance(collider);
            Vector2 topPoint = entrantDist.pointA;
            Vector2 botPoint = slopeZone.Distance(collider).pointA;

            Vector2 slopeForce = ((plateau.height/Vector2.Distance(topPoint, botPoint)) * forceConstant) * (topPoint - botPoint).normalized;
            float elevation = (Vector2.Distance(entrantDist.pointB, topPoint) / Vector2.Distance(topPoint, botPoint)) * plateau.height;

            collider.attachedRigidbody.AddForce(slopeForce);
            //TODO SET COLLIDER ELEVATION
        }
    }
}
