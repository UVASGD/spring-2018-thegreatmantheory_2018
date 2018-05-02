using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public ParticleSystem smoke;
    public ParticleSystem flash;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire() {
        smoke.Play();
        flash.Play();
    }
}
