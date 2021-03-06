﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundbox : WeaponSoundbox {

    public AudioSource fireSound;

    // Use this for initialization
    void Start() {
        AudioSource[] sources = GetComponents<AudioSource>();
        source = sources[0];
        fireSound = sources[1];
        am = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Shoot() {
		if (am == null) {
			return;
		}
        fireSound.volume = 1f;
        fireSound.PlayOneShot(am.GetSound("gun_shot"));
    }
}
