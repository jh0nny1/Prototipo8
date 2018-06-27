using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memoria;
using System;

public class InteractionManager : MonoBehaviour {

    public static InteractionManager Instance { set; get; }

    public PitchGrabManager pitchGrabManager;
    public LookPointerRaycasting raycastingSpherePlane;

    public List<Action> updateList;

    private void Awake()
    {
        Instance = this;
        updateList = new List<Action>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(var function in updateList)
        {
            function();
        }
	}

}
