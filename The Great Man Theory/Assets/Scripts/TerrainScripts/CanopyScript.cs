using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanopyScript : MonoBehaviour {

    SpriteRenderer canopySprite;

    int collisions = 0;
    bool isEntered = false;
    Color a;

    // Use this for initialization
    void Start() {
        canopySprite = GetComponent<SpriteRenderer>();
        a = canopySprite.color;
    }

    private void Update() {
        if (collisions == 0) {
            if (canopySprite.color.a < a.a) {
                canopySprite.color = Color.Lerp(canopySprite.color, a, 0.1f);
            }
        }
        else {
            if (canopySprite.color.a > 0.01f) {
                canopySprite.color = Color.Lerp(canopySprite.color, Color.clear, 0.1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body")) {
            collisions++;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Body")) {
            collisions--;
        }
    }
}
