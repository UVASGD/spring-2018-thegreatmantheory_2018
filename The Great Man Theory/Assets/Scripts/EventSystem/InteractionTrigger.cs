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
                if (onceOnly)
                    triggered = true;
                else
                    strEvent.Invoke("");
            }
        }

        if (triggered && Input.GetKeyUp(key))
            // triggered = false;

        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Transform objParent = other.gameObject.transform.parent;

        if (other.gameObject.CompareTag("Body") && objParent && objParent.gameObject.CompareTag("Player")) {
            Debug.Log("entered");
            entered = true;
            sprite.color = new Color(1f, 0.92f, 0.016f, 0.5f);
            sprite.sprite = enteredSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        Transform objParent = other.gameObject.transform.parent;

        if (other.gameObject.CompareTag("Body") && objParent && objParent.gameObject.CompareTag("Player")) {
            Debug.Log("exited");
            entered = false;
            sprite.color = Color.white;
            sprite.sprite = unenteredSprite;
        }
    }
}
