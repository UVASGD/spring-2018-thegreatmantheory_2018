using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour {

	private Camera cameron;

	private float targetZoom = 20;

	private float factor = 1;

	// Use this for initialization
	void Start () {
		cameron = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		cameron.orthographicSize = Mathf.Lerp (cameron.orthographicSize, targetZoom, factor * Time.deltaTime);
	}

	public void ZoomTo (float f) {
		targetZoom = Mathf.Clamp (f, 0, 2000);
	}
}
