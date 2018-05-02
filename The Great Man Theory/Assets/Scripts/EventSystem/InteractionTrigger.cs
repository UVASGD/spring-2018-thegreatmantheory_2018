using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class InteractionTrigger : EventTrigger {

    public KeyCode key;
    Collider2D activationArea;

    public Sprite unenteredSprite;
    public Sprite enteredSprite;

    SpriteRenderer sprite;

    bool entered = false;

	// Use this for initialization
	void Start () {
        activationArea = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = unenteredSprite;
    }
	
	// Update is called once per frame
	protected override void Update () {
		if (entered) {
            if (Input.GetKeyDown(key)) {
                triggered = true;
            }
        }

        if (triggered && Input.GetKeyUp(key))
            triggered = false;

        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Transform objParent = other.gameObject.transform.parent;

        if (other.gameObject.CompareTag("Body") && objParent && objParent.gameObject.CompareTag("Player")) {
            Debug.Log("entered");
            entered = true;
            sprite.sprite = enteredSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        Transform objParent = other.gameObject.transform.parent;

        if (other.gameObject.CompareTag("Body") && objParent && objParent.gameObject.CompareTag("Player")) {
            Debug.Log("exited");
            entered = false;
            sprite.sprite = unenteredSprite;
        }
    }
}
