using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{

    /*Ship handling parameters*/
    public float fwd_accel = 100f;
    public float fwd_max_speed_normal = 40f;
    public float fwd_max_speed_boost = 80f;
    public float brake_speed = 200f;
    public float turn_speed = 50f;

    /*Auto adjust to track surface parameters*/
    public float hover_height = 2f;
    public float height_smooth = 10f;
    public float pitch_smooth = 5f;
    public float turn_tilt = 30f;
    public float brake_tilt = 10f;

    /* Other stuff */
    private Vector3 prevUp;
    private Vector3 prevFwd;
    private float yaw;
    private float prevYaw;
    private float roll;
    private float smooth_y;
    private float current_speed;
    private float fwd_max_speed;
    private bool onTrack = true;

    private Vector3 lastPos;
    private Quaternion lastRot;

    public float CurrentSpeed 
    { 
        get 
        { 
            return current_speed; 
        } 
    }

    void Start()
    {
        fwd_max_speed = fwd_max_speed_normal;
        yaw = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        //if (Input.GetAxis("Vertical") != 0)
        //    current_speed += (current_speed >= fwd_max_speed) ? 0f : fwd_accel * (Input.GetAxis("Vertical") * Time.deltaTime);
        if(Input.GetAxis("RightTrigger") != 0)
            current_speed += (current_speed >= fwd_max_speed) ? 0f : fwd_accel * (-Input.GetAxis("RightTrigger") * Time.deltaTime);

        else
        {
            if (current_speed > 0)
            {
                current_speed -= brake_speed * Time.deltaTime;
            }
            else
            {
                current_speed = 0f;
            }
        }

        prevYaw = yaw;

        yaw += turn_speed * Time.deltaTime * Input.GetAxis("Horizontal");

        prevUp = transform.up;
        prevFwd = transform.forward;

        //transform.rotation = Quaternion.Euler(0, yaw, 0);

        RaycastHit hit;
        if (onTrack && Physics.Raycast(transform.position, -prevUp, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

            Vector3 desired_up = Vector3.Lerp(prevUp, hit.normal, Time.deltaTime * pitch_smooth);

            Quaternion tilt = Quaternion.FromToRotation(Vector3.up, desired_up);

            Debug.DrawRay(transform.position, hit.normal * 100, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
            Debug.DrawRay(transform.position, transform.right * 100, Color.blue);
            Debug.DrawRay(transform.position, transform.up * 100, Color.yellow);

            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, desired_up), desired_up);
            transform.Rotate(Vector3.up, turn_speed * Time.deltaTime * Input.GetAxis("Horizontal"));
            //transform.rotation = Quaternion.Euler(x.eulerAngles + transform.rotation.eulerAngles);

            smooth_y = Mathf.Lerp(smooth_y, hover_height - hit.distance, Time.deltaTime * height_smooth);
            transform.localPosition += prevUp * smooth_y;

            lastPos = transform.position;
            lastRot = transform.rotation;
        }
        else
            onTrack = false;

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            transform.position = lastPos;
            transform.rotation = lastRot;
            Debug.Log("Reset");
            onTrack = true;
        }

        transform.position += transform.forward * (current_speed * Time.deltaTime);
    }
}
