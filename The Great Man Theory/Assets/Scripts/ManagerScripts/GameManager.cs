using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Camera mainCamera;
    public float extraForce;
    public float maxForce;
    public float offset;
    public float wigglemax;

	// Use this for initialization
	void Start () {
        ManagerGetter.gm = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public static class ManagerGetter {
    public static GameManager gm;

    public static GameManager Get() {
        return gm;
    }
}
