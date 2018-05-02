using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour {

    public Image i;

    public void LevelOne() {
        StartCoroutine("LoadLevel", "LevelOne");
    }

    public void LevelTwo() {
        StartCoroutine("LoadLevel", "LevelTwo");
    }

    public void LevelThree() {
        StartCoroutine("LoadLevel", "LevelThree");
    }

    public void WildAndFree() {
        StartCoroutine("LoadLevel", "WildAndFree");
    }

    IEnumerator LoadLevel(string levelName) {
        i.enabled = true;

        while (i.color.a < 0.98f) {
            i.color = Color.Lerp(i.color, Color.black, 0.1f);
            yield return null;
        }

        SceneManager.LoadScene(levelName);
    }
}
