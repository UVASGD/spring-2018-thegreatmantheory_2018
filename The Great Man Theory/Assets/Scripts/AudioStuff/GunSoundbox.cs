using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundbox : WeaponSoundbox {

    // Use this for initialization
    void Start() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Shoot() {
        source.volume = 1f;
        source.PlayOneShot(am.GetSound("gun_shot"));
    }
}
