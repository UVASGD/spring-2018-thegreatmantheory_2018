using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public ParticleSystem smoke;
    public ParticleSystem flash;
    public ParticleSystem fire;

    PointEffector2D effector;
    Collider2D collider;

    public float damage = 50f;

    float life = 0.2f;

	// Use this for initialization
	void Start () {
        effector = GetComponent<PointEffector2D>();
        collider = GetComponent<Collider2D>();

        smoke.Play();
        flash.Play();
        fire.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (life > 0f) {
            life -= Time.deltaTime;
        }
        else {
            Destroy(effector);
            Destroy(collider);
        }
	}

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Boom");

        Body bod = other.gameObject.GetComponent<Body>();

        if (bod) {
            Debug.Log("Boof");
            bod.Damage(damage);
        }
    }
}
