using UnityEngine;
using System.Collections;

public class RaceTracker : MonoBehaviour {

    private GameObject[] checkpoints;
    private int checkpointCount = 0;
    private int totalCheckpoints;

    private int currentLap = 1;
    private int totalLaps = 3;

	// Use this for initialization
	void Start () {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        totalCheckpoints = checkpoints.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            checkpointCount++;
        }
        else if (other.gameObject.tag == "Finish")
        {
            if (checkpointCount == totalCheckpoints)
            {
                checkpointCount = 0;
                currentLap++;
                if (currentLap > totalLaps)
                    Debug.Log("Finish");
                else
                    Debug.Log("Lap " + currentLap);
            }
        }
    }
}
