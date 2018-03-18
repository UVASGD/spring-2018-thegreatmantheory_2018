using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip roblox_oof;
    public AudioClip ded_egh;
    public AudioClip blade_hit;
    public AudioClip wood_thunk;
    public AudioClip gun_shot;

    Dictionary<string, AudioClip> dict;

	// Use this for initialization
	void Start () {
        dict = new Dictionary<string, AudioClip> {
            { "body_death", ded_egh },
            { "body_hit", roblox_oof },
            { "blade_hit", blade_hit },
            { "generic_collision", wood_thunk },
            { "gun_shot", gun_shot },
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetSound(string soundName) {
        return dict[soundName];
    }
}
