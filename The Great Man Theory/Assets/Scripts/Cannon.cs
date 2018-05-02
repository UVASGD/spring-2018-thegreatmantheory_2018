using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject bomb;

    public ParticleSystem smoke;
    public ParticleSystem flash;

    public Rigidbody2D player;

    bool engaged = false;
	
	// Update is called once per frame
	void Update () {
        if (engaged) {
            if (Input.GetKeyDown(KeyCode.G))
                Disengage();
            else if (Input.GetMouseButtonDown(0))
                Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

	}

    public void Fire(Vector3 target) {
        smoke.Play();
        flash.Play();
        Instantiate(bomb, new Vector3(target.x, target.y, 0f), Quaternion.identity);
    }

    public void Engage() {
        if (!engaged) {
            player.constraints = RigidbodyConstraints2D.FreezeAll;
            CameraZoom cam = Camera.main.GetComponent<CameraZoom>();
            CameraFollow camF = Camera.main.GetComponent<CameraFollow>();
            cam.ZoomTo(30);
            camF.offset = new Vector3(-15, 20, -10);
            engaged = true;
        }
    }

    public void Disengage() {
        player.constraints = RigidbodyConstraints2D.None;
        CameraZoom cam = Camera.main.GetComponent<CameraZoom>();
        CameraFollow camF = Camera.main.GetComponent<CameraFollow>();
        cam.ZoomTo(15);
        camF.offset = new Vector3(0, 0, -10);
        engaged = false;
    }
}
