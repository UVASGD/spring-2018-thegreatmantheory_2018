using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followPoint;
    public Vector3 offset;
    public float smoothSpeed = 1f;

    private void Start() {
        offset = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (followPoint) {
            if ((followPoint.position - transform.position).magnitude > 5) {
                Vector3 target = followPoint.position + offset;
                Vector3 smoothTarget = Vector3.Lerp(transform.position, target, smoothSpeed);
                transform.position = smoothTarget;
            }
        }
	}

}