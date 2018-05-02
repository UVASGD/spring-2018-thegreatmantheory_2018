using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeHorseAI : MonoBehaviour {

	public bool activated = false;

	Transform target;

	public static Transform ALLTARGET;

	public Transform selfTransform;

	public float speed;

	public Rigidbody2D rigidBody;

	public float repelvelocity = 100;

	// Use this for initialization
	void Start () {
		activated = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			Vector2 delta = target.position - selfTransform.position;
			Vector2 movement = delta.normalized * speed * (Mathf.Atan (delta.magnitude));
			rigidBody.velocity = movement;
		}

		Collider2D[] closeboys = Physics2D.OverlapCircleAll (selfTransform.position, 4);
		foreach(Collider2D boy in closeboys) {
			if (boy.CompareTag ("Horse")) {
				boy.gameObject.GetComponent<FreeHorseAI> ().rigidBody.velocity += (Vector2)((boy.transform.position) - selfTransform.position).normalized * repelvelocity;
			}
		}
	}

	public void Activate(Transform _target) {
		target = _target;
		if (!ALLTARGET) {
			ALLTARGET = _target;
		}
		activated = true;
	}
}
