using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip roblox_oof;
    public AudioClip ded_egh;
    public AudioClip blade_hit;
    public AudioClip wood_thunk;

    Dictionary<string, AudioClip> dict;

	// Use this for initialization
	void Start () {
        dict = new Dictionary<string, AudioClip> {
            { "body_death", ded_egh },
            { "body_hit", roblox_oof },
            { "blade_hit", blade_hit },
            { "haft_hit", wood_thunk },
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetSound(string soundName) {
        return dict[soundName];
    }
}
