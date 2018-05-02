using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BombSpawn : MonoBehaviour {

    Collider2D collide;
    Bounds bounds;

    public GameObject bomb;

    public float spawnMin = 0f;
    public float spawnMax = 3f;
    float cooldown;

	// Use this for initialization
	void Start () {
        collide = GetComponent<Collider2D>();
        bounds = collide.bounds;
        cooldown = Random.Range(spawnMin, spawnMax);
	}
	
	// Update is called once per frame
	void Update () {
        if (cooldown <= 0f) {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            Instantiate(bomb, new Vector3(x, y, transform.position.z), transform.rotation);

            cooldown = Random.Range(spawnMin, spawnMax);
        }
        else
            cooldown -= Time.deltaTime;
    }
}
