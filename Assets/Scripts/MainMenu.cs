using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button LevelSelectButton;

	// Use this for initialization
	void Start () 
    {
        LevelSelectButton.onClick.AddListener(() => LevelSelect());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LevelSelect()
    {
        Application.LoadLevel(1);
    }
}
