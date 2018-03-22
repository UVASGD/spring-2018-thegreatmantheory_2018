using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : MonoBehaviour {

    public AudioClip roblox_oof;
    public AudioClip ded_egh;
    public AudioClip blade_hit;
    public AudioClip wood_thunk;
    public AudioClip gun_shot;

    Dictionary<string, AudioClip> dict;

    #region Singleton

    public static AudioManager Instance;

    #endregion

    void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        dict = new Dictionary<string, AudioClip> {
            { "body_death", ded_egh },
            { "body_hit", roblox_oof },
            { "blade_hit", blade_hit },
            { "generic_collision", wood_thunk },
            { "gun_shot", gun_shot },
        };

        char s = 'A';
        for (int i = 0; i < 26; i++) {
            int charint = (int)s + i;
            char charletter = (char)charint;
            string letter = charletter.ToString();
            string name = "English_" + letter;
            string path = "Assets/Audio/LetterSounds/English_" + letter + ".mp3";

            AudioClip clip = (AudioClip) AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
            dict[name] = clip;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetSound(string soundName) {
        if (dict.ContainsKey(soundName))
            return dict[soundName];
        else
            return null;
    }
}
