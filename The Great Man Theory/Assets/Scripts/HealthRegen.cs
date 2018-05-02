using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : MonoBehaviour {

    Body body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Body>();
	}
	
	// Update is called once per frame
	void Update () {
        if (body.Health < body.maxHealth) body.Damage(-5 * Time.deltaTime);
	}
}
