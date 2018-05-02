using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodMovement : MonoBehaviour {

	public float speed = 10;

	public Rigidbody2D rigidBody;

	public Transform selfTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousepos = Input.mousePosition;

		Vector3 mouseviewportpos = Camera.main.ScreenToViewportPoint (mousepos);
		Vector3 mouseworldpoint = Camera.main.ViewportToWorldPoint(new Vector2(Mathf.Max(Mathf.Min(mouseviewportpos.x, 1),0), Mathf.Max(Mathf.Min(mouseviewportpos.y, 1), 0)));
		Vector2 delta = mouseworldpoint - selfTransform.position;
		Vector2 movement = delta.normalized * speed * (Mathf.Atan (delta.magnitude));
		rigidBody.velocity = movement;
	}
}
