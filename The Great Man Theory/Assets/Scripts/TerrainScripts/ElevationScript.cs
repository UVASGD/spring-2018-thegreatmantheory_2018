using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationScript : MonoBehaviour {
    public float pub_elevation = 5;
    private float priv_elevation = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(priv_elevation-pub_elevation > 0.0)
        {
            priv_elevation = pub_elevation;
            gameObject.transform.localScale(priv_elevation);
        }
	}
}
