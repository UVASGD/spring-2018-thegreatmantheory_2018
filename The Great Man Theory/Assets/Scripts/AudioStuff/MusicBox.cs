using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : EventManager {

    AudioManager am;
    AudioSource source;

	// Use this for initialization
	void Start () {
        am = AudioManager.Instance;
        source = GetComponent<AudioSource>();
	}
	
	public IEnumerator TeDeum() {
        source.PlayOneShot(am.GetSong("TeDeum"));
        yield return null;
    }
    
    public IEnumerator Dvorak() {
        source.PlayOneShot(am.GetSong("Dvorak"));
        yield return null;
    }

    public IEnumerator Triomphe() {
        source.PlayOneShot(am.GetSong("Triomphe"));
        yield return null;
    }

    public IEnumerator DiesIrae() {
        source.PlayOneShot(am.GetSong("DiesIrae"));
        yield return null;
    }

    public IEnumerator Silence() {
        source.Stop();
        yield return null;
    }
}
