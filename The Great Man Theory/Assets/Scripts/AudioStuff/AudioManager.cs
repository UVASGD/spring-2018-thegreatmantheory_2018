using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip roblox_oof;

    Dictionary<string, AudioClip> dict;

	// Use this for initialization
	void Start () {
        dict = new Dictionary<string, AudioClip> {
            { "body_death", roblox_oof },
            { "body_hit", roblox_oof },
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetSound(string soundName) {
        return dict[soundName];
    }
}
