using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static bool created = false;

    public AudioClip Dvorak;
    public AudioClip TeDeum;
    public AudioClip Triomphe;

    public AudioClip roblox_oof;
    public AudioClip ded_egh;
    public AudioClip blade_hit;
    public AudioClip wood_thunk;
    public AudioClip gun_shot;

    Dictionary<string, AudioClip> audioDict;
    Dictionary<string, AudioClip> musicDict;


    #region Singleton

    public static AudioManager Instance;

    #endregion

    void Awake() {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        audioDict = new Dictionary<string, AudioClip> {
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
            string path = "Audio/LetterSounds/English_" + letter; //+ ".mp3";

            AudioClip clip = (AudioClip)Resources.Load(path, typeof(AudioClip));
            // Debug.Log(clip);
            audioDict[name] = clip;
        }

        musicDict = new Dictionary<string, AudioClip> {
            { "Dvorak", Dvorak },
            { "TeDeum", TeDeum },
            { "Triomphe", Triomphe },
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetSound(string soundName) {
        if (audioDict.ContainsKey(soundName))
            return audioDict[soundName];
        else
            return null;
    }

    public AudioClip GetSong(string songName) {
        if (musicDict.ContainsKey(songName))
            return musicDict[songName];
        else
            return null;
    }
}