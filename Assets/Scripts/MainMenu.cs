using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button LevelSelectButton;

    // Use this for initialization
    void Start()
    {
        LevelSelectButton.onClick.AddListener(() => LevelSelect());
        LevelSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    LevelSelectButton.Select();
        //}
    }

    void LevelSelect()
    {
        Application.LoadLevel(1);
    }
}
