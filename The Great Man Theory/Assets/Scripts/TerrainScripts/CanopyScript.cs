using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanopyScript : MonoBehaviour {

    public SpriteRenderer[] canopySprites;

    int collisions = 0;
    bool isEntered = false;
    Color a;

    // Use this for initialization
    void Start() {
        // canopySprite = GetComponent<SpriteRenderer>();
        if (canopySprites.Length > 0)
            a = canopySprites[0].color;
    }

    private void Update() {
        if (collisions == 0) {
            foreach (SpriteRenderer canopySprite in canopySprites) {
                if (canopySprite.color.a < a.a) {
                    canopySprite.color = Color.Lerp(canopySprite.color, a, 0.1f);
                }
            }
        }
        else {
            foreach (SpriteRenderer canopySprite in canopySprites) {
                if (canopySprite.color.a > 0.01f) {
                    canopySprite.color = Color.Lerp(canopySprite.color, Color.clear, 0.1f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Transform parentTransform = collider.transform.parent;
        // if (parent && parent.CompareTag("Player")) {
        Body bod = collider.gameObject.GetComponent<Body>();
        if (bod && bod.team == Team.GoodGuys && parentTransform || parentTransform && parentTransform.gameObject.CompareTag("Player")) {
            collisions++;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        Transform parentTransform = collider.transform.parent;
        // if (parent && parent.CompareTag("Player")) {
        Body bod = collider.gameObject.GetComponent<Body>();
        if (bod && bod.team == Team.GoodGuys && parentTransform || parentTransform && parentTransform.gameObject.CompareTag("Player")) {
            collisions--;
        }
    }
}
