using Gamelogic;
using Leap.Unity;
using Memoria;
using Memoria.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationObjectManager : MonoBehaviour {

    public static InformationObjectManager Instance { set; get; }

    public PlaneImageManager planeImages;

    //Visualization configuration
    public bool autoTuneVisualizationOnPlay;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        
    }

    
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
