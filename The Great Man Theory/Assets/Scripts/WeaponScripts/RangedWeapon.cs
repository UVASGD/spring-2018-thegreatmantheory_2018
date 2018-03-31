using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon {

    public float reloadTime = 2f;
    float recoilStrength = 5000f;

    float reload;
    float damage = 100f;

    int shotNum = 0;

    public float maxrange = 100f;

    Vector2 barrelEnd;

    public ParticleSystem[] fxs;
    LineRenderer line;

    GunSoundbox gunbox;

    protected override void Start() {
        base.Start();
        fxs = GetComponentsInChildren<ParticleSystem>();
        line = GetComponent<LineRenderer>();

        gunbox = GetComponentInChildren<GunSoundbox>();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (player)
            GetInput();

        if (reload < reloadTime)
            reload += Time.deltaTime;
        if (reload > 0.1f)
            line.enabled = false;
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
        bool gotem = false;
        barrelEnd = transform.position + (transform.up * 1.25f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(barrelEnd, transform.up, maxrange);

        foreach (RaycastHit2D hit in hits) {
            if (hit.rigidbody != null) {
                Body target = hit.rigidbody.GetComponent<Body>();
                if (target) {
                    float dam = damage - hit.distance;
                    target.Hit(dam / 2, hit.point);
                    ShowLine(hit.distance);
                    gotem = true;
                    break;
                }
            }
        }
        if (!gotem)
            ShowLine(maxrange);

    }

    void Recoil() {
        Vector2 backwards = -transform.up;
        rb.AddForce(backwards * recoilStrength);
    }

    void FX() {
        foreach (ParticleSystem fx in fxs) {
            fx.Play();
        }
		if (gunbox != null) {
            Debug.Log("BOOMBOOMBOOM");
			gunbox.Shoot();
		}
    }

    void ShowLine(float dist) {
        var the_line = transform.up * dist;
        line.enabled = true;
        line.SetPosition(0, new Vector3(barrelEnd.x, barrelEnd.y, 0f));
        line.SetPosition(1, new Vector3(barrelEnd.x, barrelEnd.y, 0f) + the_line);
    }

}
