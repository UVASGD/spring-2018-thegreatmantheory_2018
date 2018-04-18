using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauScript : MonoBehaviour {
    public float elevation = 5;
    public float height;

    public Collider2D plateauZone;

    public SlopeScript slope;


	// Use this for initialization
	void Start () {
        if (!plateauZone)
            plateauZone = GetComponent<Collider2D>();

        if (!slope)
            slope = transform.parent.GetComponentInChildren<SlopeScript>();

        height = elevation - slope.elevation;
	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body")) {
            Physics2D.IgnoreCollision(slope.slopeZone, collider);
            //TODO SET ELEVATION OF COLLIDER.ELEVATION
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Body"))
            Physics2D.IgnoreCollision(slope.slopeZone, collider);
    }
}
