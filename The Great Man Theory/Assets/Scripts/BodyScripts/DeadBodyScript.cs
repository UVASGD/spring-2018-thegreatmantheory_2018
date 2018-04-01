using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyScript : MonoBehaviour {

    SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sprite.color = Color.Lerp(sprite.color, Color.clear, 0.01f);

        if (sprite.color.a <= 0.005f) {
            Destroy(gameObject);
        }
	}
}
