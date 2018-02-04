using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTracking : MonoBehaviour {

    public float maxDegrees = 1000f;
    public GameObject point;

	
	// Update is called once per frame
	void Update () {
        Look();
	}

    void Look() {
        if (point)
            gameObject.transform.LookAt(point.transform, Vector3.forward);
        Vector3 angles = gameObject.transform.rotation.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angles.z));
    }
}
