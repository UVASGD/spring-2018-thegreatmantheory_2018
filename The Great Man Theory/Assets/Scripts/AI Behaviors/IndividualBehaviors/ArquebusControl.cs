using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon {

    public Transform cloud;
    Transform lastCloud;

    public float reloadTime = 2f;
    float recoilStrength = 8000f;

    float reload;
    float damage = 100f;

    int shotNum = 0;

    Vector2 barrelEnd;
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
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

    public void Trigger() {
        Shoot();
        Recoil();
    }

    void Shoot() {
        barrelEnd = transform.position + (transform.up * 1.25f);
        RaycastHit2D[] hits = Physics2D.RaycastAll(barrelEnd, transform.up);

        foreach(RaycastHit2D hit in hits) {
            Body target = hit.rigidbody.GetComponent<Body>();
            if (target) {
                float damage = 125 - hit.distance;
                target.Hit(damage/2, hit.point);
                Debug.Log(damage);
                break;
            }
        }

        if (lastCloud) { Destroy(lastCloud.gameObject); } 
        lastCloud = Instantiate(cloud, barrelEnd, Quaternion.identity);
    }

    void Recoil() {
        Debug.Log("BANG" + shotNum.ToString());
        Vector2 backwards = -transform.up;
        rb.AddForce(backwards * recoilStrength);
        Debug.Log(backwards);
    }

}
