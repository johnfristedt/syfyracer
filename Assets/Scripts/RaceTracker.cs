using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaceTracker : MonoBehaviour
{

    public GameObject LapTimePrefab;

    private GameObject[] checkpoints;
    private int checkpointCount = 0;
    private int totalCheckpoints;

    private int currentLap = 1;
    private int totalLaps = 3;

    private float lapTimer = 0;
    private List<GameObject> lapTimes;

    private int minutes;
    private int seconds;
    private int fractions;

    private bool raceOver = false;

    // Use this for initialization
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        totalCheckpoints = checkpoints.Length;
        lapTimes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!raceOver)
        {
            lapTimer += Time.deltaTime;

            minutes = (int)(lapTimer / 60);
            seconds = (int)(lapTimer % 60);
            fractions = (int)((lapTimer - (int)lapTimer) * 100);

            GameObject.Find("LapTimer").GetComponent<Text>().text = string.Format("{0}:{1}:{2}", (minutes > 0) ? minutes : 0,
                                                                                                 (seconds.ToString().Length > 1) ? seconds.ToString() : "0" + seconds.ToString(),
                                                                                                 (fractions.ToString().Length > 1) ? fractions.ToString() : "0" + fractions.ToString());
        }
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
                GameObject.Find("LapCounter").GetComponent<Text>().text = "Lap " + Mathf.Min(currentLap, totalLaps) + "/" + totalLaps;

                lapTimes.Add(Instantiate(LapTimePrefab));

                lapTimes[lapTimes.Count - 1].GetComponent<Text>().text = string.Format("{0}:{1}:{2}", (minutes > 0) ? minutes : 0,
                                                                                                      (seconds.ToString().Length > 1) ? seconds.ToString() : "0" + seconds.ToString(),
                                                                                                      (fractions.ToString().Length > 1) ? fractions.ToString() : "0" + fractions.ToString());

                lapTimes[lapTimes.Count - 1].transform.parent = GameObject.Find("Lap").transform;
                Vector3 pos = new Vector3(-460, -150 - (60 * (lapTimes.Count - 1)), 0);
                Debug.Log(pos);
                lapTimes[lapTimes.Count - 1].transform.localPosition = pos;
                lapTimes[lapTimes.Count - 1].transform.localRotation = Quaternion.identity;
                lapTimes[lapTimes.Count - 1].transform.localScale = Vector3.one;

                checkpointCount = 0;
                currentLap++;
                lapTimer = 0;




                if (currentLap > totalLaps)
                {
                    Debug.Log("Finish");
                    raceOver = true;
                }
                else
                    Debug.Log("Lap " + currentLap);
            }
        }
    }
}
