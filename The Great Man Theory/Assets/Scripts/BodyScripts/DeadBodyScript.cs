using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyScript : MonoBehaviour {

    SpriteRenderer sprite;
    public float step = 0.01f;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sprite.color = Color.Lerp(sprite.color, Color.clear, step);

        if (sprite.color.a <= 0.005f) {
            Destroy(gameObject);
        }
	}
}
