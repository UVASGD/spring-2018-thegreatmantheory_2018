using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon {

    public float reloadTime = 2f;
    float recoilStrength = 8000f;

    float reload;
    float damage = 100f;

    int shotNum = 0;

    Vector2 barrelEnd;

    public ParticleSystem[] fxs;

    protected override void Start() {
        base.Start();
        fxs = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        GetInput();
        if (reload < reloadTime)
            reload += Time.deltaTime;
    }

    void GetInput() {
        if (Input.GetKeyDown(KeyCode.F) && reload > reloadTime) {
            shotNum++;
            reload = 0f;
            Shoot();
            Recoil();
            FX();
        }
    }

    public void Trigger() {
        Shoot();
        Recoil();
        FX();
    }

    void Shoot() {
        barrelEnd = transform.position + (transform.up * 1.25f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(barrelEnd, transform.up);

        foreach (RaycastHit2D hit in hits) {
            Body target = hit.rigidbody.GetComponent<Body>();
            if (target) {
                float damage = 125 - hit.distance;
                target.Hit(damage / 2, hit.point);
                Debug.Log(damage);
                break;
            }
        }

    }

    void Recoil() {
        Debug.Log("BANG" + shotNum.ToString());
        Vector2 backwards = -transform.up;
        rb.AddForce(backwards * recoilStrength);
        Debug.Log(backwards);
    }

    void FX() {
        Debug.Log(fxs);
        foreach (ParticleSystem fx in fxs) {
            fx.Play();
        }
    }


}
