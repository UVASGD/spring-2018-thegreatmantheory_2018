using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArquebusControl : MonoBehaviour {

    public float reloadTime = 2f;
    float recoilStrength = 5000f;

    Rigidbody2D body;

    float reload;

    int shotNum = 0;

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
            Recoil();
        }
    }

    void Recoil() {
        Debug.Log("BANG" + shotNum.ToString());
        Vector2 backwards = -transform.up;
        body.AddForce(backwards * recoilStrength);
        Debug.Log(backwards);
    }
}
