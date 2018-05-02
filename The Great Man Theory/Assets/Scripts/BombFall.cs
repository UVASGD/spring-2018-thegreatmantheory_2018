using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BombFall : MonoBehaviour {

    public float fallTime = 3f;
    float fallCount = 0f;
    public GameObject bomb;

    SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(0.1f, 0.1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (fallCount < fallTime) {
            fallCount += Time.deltaTime;

            float fallProgress = fallCount / fallTime;

            Color color = sprite.color;
            sprite.color = new Color(color.r, color.g, color.b, fallProgress);

            transform.localScale = new Vector3(fallProgress, fallProgress, 1f);
        }
        else {
            Instantiate(bomb, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}
}
