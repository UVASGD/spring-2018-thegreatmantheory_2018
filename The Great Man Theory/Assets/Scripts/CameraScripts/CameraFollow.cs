using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followPoint;
    public Vector3 offset = new Vector3(0, 0, -10);
    float smoothSpeed = 5f;

    public bool OnTarget { get { return onTarget; } }
    bool onTarget = false;

    // Update is called once per frame
    void LateUpdate () {
        if (followPoint) {
            //if ((followPoint.position - transform.position).magnitude > 5) {
            Vector3 target = followPoint.position + offset;
            float smoothVal = smoothSpeed * Time.deltaTime;
            Vector3 smoothTarget = Vector3.Lerp(transform.position, target, smoothVal);
            transform.position = smoothTarget;
            //}
            if ((target - transform.position).magnitude < 1) {
                onTarget = true;
            }
            else
                onTarget = false;
        }
	}

}