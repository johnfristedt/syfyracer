using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public GameObject Player;

    private float shipSpeed;

	// Use this for initialization
	void Start ()
    {
        GameObject.Find("Speedo").GetComponent<Slider>().maxValue = Player.GetComponent<Ship>().fwdMaxSpeedNormal;

        float maxTilt = Player.transform.FindChild("CameraMount").GetComponent<CameraRotate>().maxTilt;
        GameObject.Find("RightRoll").GetComponent<Slider>().maxValue = maxTilt;
        GameObject.Find("RightRoll").GetComponent<Slider>().minValue = -maxTilt;

        GameObject.Find("LeftRoll").GetComponent<Slider>().maxValue = maxTilt;
        GameObject.Find("LeftRoll").GetComponent<Slider>().minValue = -maxTilt;
	}
	
	// Update is called once per frame
	void Update () 
    {
        var ship = Player.GetComponent<Ship>();
        var cameraMount = GameObject.Find("CameraMount");

        shipSpeed = ship.CurrentSpeed;

        GameObject.Find("Speedo").GetComponent<Slider>().value = shipSpeed;

        GameObject.Find("RightRoll").GetComponent<Slider>().value = -Input.GetAxis("Horizontal") * 20;
        GameObject.Find("LeftRoll").GetComponent<Slider>().value = Input.GetAxis("Horizontal") * 20;
	}
}
