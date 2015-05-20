using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{

    public Transform Anchor;
    public float Distance = 0.3f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Anchor.position + (Anchor.forward * Distance);
        transform.rotation = Quaternion.LookRotation(transform.position - Anchor.position, Anchor.up);
        transform.Rotate(Vector3.forward * Anchor.GetComponent<CameraRotate>().Tilt);
    }
}
