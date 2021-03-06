﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlideScript : MonoBehaviour {

    public AudioSource aud;

    bool going = false;

    public SlideScript next;

    public Text text;
    public Image image;

    public void Awake() {
        going = true;
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (going) {
            if (!aud.isPlaying) {
                going = false;
                if (next) {
                    StartCoroutine("LoadNext");
                }

                else {
                    StartCoroutine("LoadScene");
                }

            }
        }
	}

    public IEnumerator LoadScene() {
        while (image.color.a > 0.01) {
            image.color = Color.Lerp(image.color, Color.clear, 0.05f);
            text.color = Color.Lerp(text.color, Color.clear, 0.05f);
            yield return null;
        }
        SceneManager.LoadScene("Main Menu");
    }

    public IEnumerator LoadNext() {
        float time = 1;
        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        next.gameObject.SetActive(true);
    }
}
