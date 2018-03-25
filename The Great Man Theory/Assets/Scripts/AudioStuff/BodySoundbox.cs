using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySoundbox : MonoBehaviour {

    public AudioManager am;
    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hit(float volume) {
		if (am == null) {
			return;
		}
        source.volume = volume;
        source.PlayOneShot(am.GetSound("body_hit"));
    }

    public void Death() {
		if (am == null) {
			return;
		}
        source.volume = 1f;
        source.PlayOneShot(am.GetSound("body_death"));
    }
}
