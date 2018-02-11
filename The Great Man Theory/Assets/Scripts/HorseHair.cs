using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseHair : MonoBehaviour {

    public Rigidbody2D neck;


	// Use this for initialization
	void Start () {
		if (!neck) {
            Debug.Log("gorramit get yer neck");
        }
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 localNeck = neck.transform.InverseTransformPoint(transform.position);
		if (true) {

        }
	}
}
