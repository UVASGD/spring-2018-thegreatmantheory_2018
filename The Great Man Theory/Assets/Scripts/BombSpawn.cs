using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BombSpawn : MonoBehaviour {

    Collider2D collide;
    Bounds bounds;

    public GameObject bomb;

    public float cooldown = 0f;
    float counter;

	// Use this for initialization
	void Start () {
        collide = GetComponent<Collider2D>();
        bounds = collide.bounds;
        counter = cooldown;
	}
	
	// Update is called once per frame
	void Update () {
        if (counter <= 0f) {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            Instantiate(bomb, new Vector3(x, y, transform.position.z), transform.rotation);

            counter = cooldown;
        }
        else
            counter -= Time.deltaTime;
    }
}
