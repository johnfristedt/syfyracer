using UnityEngine;
using System.Collections;

public class ShipTest : MonoBehaviour {

    /*Ship handling parameters*/
    public float fwd_accel = 100f;
    public float fwd_max_speed_normal = 40f;
    public float fwd_max_speed_boost = 80f;
    public float brake_speed = 200f;
    public float turn_speed = 50f;

    /*Auto adjust to track surface parameters*/
    public float hover_height = 3f;     //Distance to keep from the ground
    public float height_smooth = 10f;   //How fast the ship will readjust to "hover_height"
    public float pitch_smooth = 5f;     //How fast the ship will adjust its rotation to match track normal
    public float turn_tilt = 30f;
    public float brake_tilt = 10f;

    /*We will use all this stuff later*/
    private Vector3 prevUp;
    private Vector3 prevFwd;
    public float yaw;
    private float roll;
    private float smooth_y;
    private float current_speed;
    private float fwd_max_speed;
    private bool boost = false;

    void Start()
    {
        fwd_max_speed = fwd_max_speed_normal;
    }

    void Update()
    {
        if (boost)
            fwd_max_speed = fwd_max_speed_boost;
        else
            fwd_max_speed = fwd_max_speed_normal;

        

        /*Here we get user input to calculate the speed the ship will get*/
        /*Increase our current speed only if it is not greater than fwd_max_speed*/
        if (Input.GetAxis("Vertical") != 0)
            current_speed += (current_speed >= fwd_max_speed) ? 0f : fwd_accel * (Input.GetAxis("Vertical") * Time.deltaTime);

        else
        {
            if (current_speed > 0)
            {
                /*The ship will slow down by itself if we dont accelerate*/
				current_speed -= brake_speed * Time.deltaTime;
            }
            else
            {
                current_speed = 0f;
            }
        }

        /*We get the user input and modifiy the direction the ship will face towards*/
        yaw += turn_speed * Time.deltaTime * Input.GetAxis("Horizontal");
        /*We want to save our current transform.up vector so we can smoothly change it later*/
        prevUp = transform.up;
        prevFwd = transform.forward;
        /*Now we set all angles to zero except for the Y which corresponds to the Yaw*/
        transform.rotation = Quaternion.Euler(0, yaw, 0);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -prevUp, out hit))
        {
            Debug.DrawLine(transform.position, hit.point);

            /*Here are the meat and potatoes: first we calculate the new up vector for the ship using lerp so that it is smoothed*/
            Vector3 desired_up = Vector3.Lerp(prevUp, hit.normal, Time.deltaTime * pitch_smooth);
            /*Then we get the angle that we have to rotate in quaternion format*/

            Quaternion tilt = Quaternion.FromToRotation(transform.up, desired_up);

            Debug.DrawRay(transform.position, tilt.eulerAngles, Color.red);
            Debug.DrawRay(transform.position, transform.forward * 100, Color.green);


            /*Now we apply it to the ship with the quaternion product property*/

            //transform.rotation = Quaternion.LookRotation(transform.up, hit.normal) * Quaternion.FromToRotation(transform.up, hit.normal);
            //Quaternion.Euler(0, turn_speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0)
            
            //transform.RotateAround(transform.position, tr;

            //* Quaternion.Euler(0, turn_speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0)

            //transform.rotation = Quaternion.LookRotation(prevFwd, hit.normal) * Quaternion.Euler(0, turn_speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0);

            transform.rotation = tilt * transform.rotation;

            /*Smoothly adjust our height*/
            smooth_y = Mathf.Lerp(smooth_y, hover_height - hit.distance, Time.deltaTime * height_smooth);
            transform.localPosition += prevUp * smooth_y;
        }

        /*Finally we move the ship forward according to the speed we calculated before*/
        transform.position += transform.forward * (current_speed * Time.deltaTime);
    }
}
