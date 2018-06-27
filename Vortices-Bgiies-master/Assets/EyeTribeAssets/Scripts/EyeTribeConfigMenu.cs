using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EyeTribeConfigMenu : MonoBehaviour {

    public Toggle UIToggle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        if (GazeCamera.Instance.useGazeTracker)
        {
            UIToggle.isOn = true;
        }
        else
        {
            UIToggle.isOn = false;
        }
    }
}
