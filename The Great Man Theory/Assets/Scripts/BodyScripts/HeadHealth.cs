using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHealth : MonoBehaviour {

    Body body;
    SpriteRenderer sp;

    Color baseColor = new Color(0.3f, 0.3f, 0.3f);

    float lastHealth;
    float maxHealth;

	// Use this for initialization
	void Start () {
        Invoke("Setup", 0.2f);
	}

    void Setup() {
        body = GetComponentInParent<Body>();
        sp = GetComponent<SpriteRenderer>();

        // baseColor = sp.color;
        sp.color = baseColor;

        lastHealth = body.Health;
        maxHealth = body.Health;
    }

    // Update is called once per frame
    void Update() {
        if (body) {
            if (Mathf.Abs(body.Health - lastHealth) > 5) {
                // Debug.Log((maxHealth - body.Health) / maxHealth);
                sp.color = Color.Lerp(baseColor, Color.red, (maxHealth - body.Health) / maxHealth);
                lastHealth = body.Health;
            }
        }
	}
}
