using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundbox : MonoBehaviour {

    public AudioManager am;
    public AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hit(float volume) {
        source.volume = volume;
        source.PlayOneShot(am.GetSound("generic_collision"));
    }
}
