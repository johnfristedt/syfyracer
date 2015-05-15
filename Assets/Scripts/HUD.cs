using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public GameObject Player;

    private float shipSpeed;

	// Use this for initialization
	void Start ()
    {
        transform.FindChild("Speedo").GetComponent<Slider>().maxValue = Player.GetComponent<Ship>().fwd_max_speed_normal;

        float maxTilt = Player.transform.FindChild("CameraMount").GetComponent<CameraRotate>().maxTilt;
        transform.FindChild("RightRoll").GetComponent<Slider>().maxValue = maxTilt;
        transform.FindChild("RightRoll").GetComponent<Slider>().minValue = -maxTilt;

        transform.FindChild("RightRoll").GetComponent<Slider>().maxValue = -maxTilt;
        transform.FindChild("RightRoll").GetComponent<Slider>().minValue = maxTilt;
	}
	
	// Update is called once per frame
	void Update () 
    {
        var ship = Player.GetComponent<Ship>();
        var cameraMount = Player.transform.FindChild("CameraMount");

        shipSpeed = ship.CurrentSpeed;

        GameObject.Find("Speedo").GetComponent<Slider>().value = shipSpeed;

        cameraMount.FindChild("RightRoll").GetComponent<Slider>().value = cameraMount.GetComponent<CameraRotate>().Tilt;
        cameraMount.FindChild("LeftRoll").GetComponent<Slider>().value = -cameraMount.GetComponent<CameraRotate>().Tilt;
	}
}
