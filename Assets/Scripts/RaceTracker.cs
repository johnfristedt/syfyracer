using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaceTracker : MonoBehaviour
{
    public GameObject HUD;
    public GameObject FinishMenu;
    public GameObject LapTimePrefab;

    private GameObject[] checkpoints;
    private int checkpointCount = 0;
    private int totalCheckpoints;

    private int currentLap = 1;
    private int totalLaps = 3;

    private float lapTimer = 0;
    private List<float> lapTimes;

    private int minutes;
    private int seconds;
    private int fractions;

    private bool raceOn = false;

    private Button ready;

    // Use this for initialization
    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        totalCheckpoints = checkpoints.Length;
        lapTimes = new List<float>();
        ready = GameObject.Find("Ready").GetComponent<Button>();
        ready.Select();
        ready.onClick.AddListener(() => StartRace());
        HUD.SetActive(false);
        //GameObject.Find("ReadyCheck").SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (raceOn)
        {
            lapTimer += Time.deltaTime;

            minutes = (int)(lapTimer / 60);
            seconds = (int)(lapTimer % 60);
            fractions = (int)((lapTimer - (int)lapTimer) * 100);
            
            GameObject.Find("LapTimer").GetComponent<Text>().text = string.Format("{0}:{1}:{2}", (minutes > 0) ? minutes : 0,
                                                                                                 (seconds.ToString().Length > 1) ? seconds.ToString() : "0" + seconds.ToString(),
                                                                                                 (fractions.ToString().Length > 1) ? fractions.ToString() : "0" + fractions.ToString());
        }
        else
        {

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

                lapTimes.Add(lapTimer);

                checkpointCount = 0;
                currentLap++;
                lapTimer = 0;




                if (currentLap > totalLaps)
                {
                    Debug.Log("Finish");
                    StopRace();
                }
                else
                    Debug.Log("Lap " + currentLap);
            }
        }
    }

    void StartRace()
    {
        HUD.SetActive(true);
        //GameObject.Find("HUD").SetActive(true);
        GameObject.Find("ReadyCheck").SetActive(false);

        raceOn = true;
    }

    void StopRace()
    {
        raceOn = false;

        HUD.SetActive(false);
        FinishMenu.SetActive(true);

        for (int i = 0; i < lapTimes.Count; i++)
        {
            minutes = (int)(lapTimes[i] / 60);
            seconds = (int)(lapTimes[i] % 60);
            fractions = (int)((lapTimes[i] - (int)lapTimes[i]) * 100);

            GameObject lapTime = Instantiate(LapTimePrefab);

            lapTime.GetComponent<Text>().text = string.Format("Lap {0}: {1}:{2}:{3}", (i + 1),
                                                                                      (minutes > 0) ? minutes : 0,
                                                                                      (seconds.ToString().Length > 1) ? seconds.ToString() : "0" + seconds.ToString(),
                                                                                      (fractions.ToString().Length > 1) ? fractions.ToString() : "0" + fractions.ToString());

            lapTime.transform.parent = FinishMenu.transform;

            Vector3 pos = new Vector3(-150, 80 - (60 * (i - 1)), 0);
            Debug.Log(pos);
            lapTime.transform.localPosition = pos;
            lapTime.transform.localRotation = Quaternion.identity;
            lapTime.transform.localScale = Vector3.one;
        }

        GameObject.Find("BackToMenu").GetComponent<Button>().Select();
    }
}
