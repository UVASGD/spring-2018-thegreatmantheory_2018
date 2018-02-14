using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArquebusControl : MonoBehaviour {

    public float reloadTime = 2f;
    float recoilStrength = 5000f;

    Rigidbody2D body;

    float reload;
    float damage = 100f;

    int shotNum = 0;

    Vector2 barrelEnd;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        if (reload < reloadTime)
            reload += Time.deltaTime;
	}

    void GetInput () {
        if (Input.GetKeyDown(KeyCode.F) && reload > reloadTime) {
            shotNum++;
            reload = 0f;
            Shoot();
            Recoil();
        }
    }

    void Shoot() {
        barrelEnd = transform.position + (transform.up * 1.25f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(barrelEnd, transform.up);

        foreach(RaycastHit2D hit in hits) {
            Body target = hit.rigidbody.GetComponent<Body>();
            if (target) {
                float damage = 125 - hit.distance;
                target.Damage(damage);
                Debug.Log(damage);
                break;
            }
        }
    }

    void Recoil() {
        Debug.Log("BANG" + shotNum.ToString());
        Vector2 backwards = -transform.up;
        body.AddForce(backwards * recoilStrength);
        Debug.Log(backwards);
    }

}
