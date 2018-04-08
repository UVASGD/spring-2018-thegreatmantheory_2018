using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraPoints : MonoBehaviour {

    #region Singleton

    public static CameraPoints Instance;

    #endregion

    void Awake() {
        Instance = this;
    }

    public Transform[] cameraPoints;

	public Transform CameraPoint(int i) {
        return cameraPoints[i];
    }
}
