using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStuck : MonoBehaviour {

    public AudioClip doorStuck;
    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
    public void DOORSTUCK() {
        source.PlayOneShot(doorStuck);
    }
}
