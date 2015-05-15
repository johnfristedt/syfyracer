using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

    public float tiltSpeed = 4;
    public float maxTilt = 20;

    private float tilt;
    private float horizontal;

    public float Tilt 
    {
        get
        {
            return tilt;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        horizontal = Input.GetAxis("Horizontal");

        tilt = Time.deltaTime * (tiltSpeed * horizontal);
        tilt = Mathf.Min(tilt, 1);

        transform.localRotation = Quaternion.Euler(-Vector3.forward * horizontal * maxTilt);
	}
}
