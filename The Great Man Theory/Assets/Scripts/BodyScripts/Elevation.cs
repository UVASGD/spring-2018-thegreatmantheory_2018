using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevation : MonoBehaviour {

	public float baseScale = 1;

	public float maxScaleMultiplier = 4;

	private float targetElevation = 0;

	private float elevation = 0;

	private float timeToChangeElevation = 1;

	public static float maxHeight = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			float rand = (Random.value * 2 - 1) * maxHeight;
			Debug.Log (rand);
			SetElevation (rand);
		}

		//if (Mathf.Abs(elevation - targetElevation) > (maxHeight / 100f)) {
			elevation = Mathf.Lerp (elevation, targetElevation, 0.01f);
			Rescale ();
		//}
	}

	void Rescale() {
		float newScale = baseScale * Mathf.Pow (maxScaleMultiplier, elevation / maxHeight);
		gameObject.transform.parent.localScale = new Vector3(newScale, newScale, 1);
	}

	public void SetElevation(float f) {
		targetElevation = Mathf.Clamp (f, (-1 * maxHeight), (maxHeight));
	}
}
