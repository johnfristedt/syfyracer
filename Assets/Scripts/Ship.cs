using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{

    /*Ship handling parameters*/
    public float fwdAccel = 100f;
    public float fwdMaxSpeedNormal = 40f;
    public float fwdMaxSpeedBoost = 80f;
    public float brakeSpeed = 200f;
    public float turnSpeed = 50f;

    /*Auto adjust to track surface parameters*/
    public float hoverHeight = 2f;
    public float heightSmooth = 10f;
    public float pitchSmooth = 5f;
    public float turnTilt = 30f;
    public float brakeTilt = 10f;

    /* Other stuff */
    public float fallSpeed = 4;

    private Vector3 prevUp;
    private Vector3 prevFwd;
    private Vector3 prevPos;
    private Vector3 pitchDelta;

    private Quaternion prevRot;

    private float yaw;
    private float prevYaw;
    private float roll;
    private float smoothY;
    private float currentSpeed;
    private float fwdMaxSpeed;
    private float deltaFactor;

    private bool onTrack = true;

    private RaycastHit hit;


    private Vector3 startPos;

    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }
    }

    void Start()
    {
        fwdMaxSpeed = fwdMaxSpeedNormal;
        yaw = transform.rotation.eulerAngles.y;
        startPos = transform.position;
    }

    void Update()
    {
        //if (Input.GetAxis("Vertical") != 0)
        //    current_speed += (current_speed >= fwd_max_speed) ? 0f : fwd_accel * (Input.GetAxis("Vertical") * Time.deltaTime);

        if (Input.GetButton("Y"))
        {
            Application.LoadLevel(1);
        }

        float throttle = -Input.GetAxis("RightTrigger");

        if (Input.GetAxis("RightTrigger") != 0)
            currentSpeed += (currentSpeed >= fwdMaxSpeed) ? 0f : fwdAccel * (throttle * Time.deltaTime);
        else if (currentSpeed > 0)
            currentSpeed -= Time.deltaTime * 40;

        Debug.Log(currentSpeed);

        prevYaw = yaw;

        yaw += turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");

        prevUp = transform.up;
        prevFwd = transform.forward;

        if (onTrack && Physics.Raycast(transform.position, -prevUp, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

            pitchDelta = transform.up - hit.normal;
            deltaFactor = pitchDelta.magnitude;

            Vector3 desired_up = Vector3.Lerp(prevUp, hit.normal, Time.deltaTime * pitchSmooth * (deltaFactor * 3));

            Quaternion tilt = Quaternion.FromToRotation(Vector3.up, desired_up);

            Debug.DrawRay(transform.position, hit.normal * 100, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
            Debug.DrawRay(transform.position, transform.right * 100, Color.blue);
            Debug.DrawRay(transform.position, transform.up * 100, Color.yellow);

            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, desired_up), desired_up);
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));

            smoothY = Mathf.Lerp(smoothY, hoverHeight - Mathf.Min(fallSpeed, hit.distance), Time.deltaTime * (heightSmooth * Mathf.Max(3, hit.distance)));
            transform.localPosition += prevUp * smoothY;

            prevPos = transform.position;
            prevRot = transform.rotation;
        }

        transform.position += transform.forward * (currentSpeed * Time.deltaTime);
    }
}
