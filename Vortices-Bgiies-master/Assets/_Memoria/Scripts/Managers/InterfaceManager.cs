using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using Memoria.Core;

public class InterfaceManager : MonoBehaviour {
    public static InterfaceManager Instance { set; get; }

    string Scope;
    //These are not game objects but components. In the inspector you must drag-n-drop the game object that holds the scripts you want and to access
    public EEGManager eegManager;
	//public LeapMotionGestureManager lpManager;

    //AQUI AGREGAR REFERENCIA AL BITALINO MANAGER; PARA QUE SEA GLOBAL 

    public EyetrackerManager eyeTrackerManager;
    public LeapMotionManager leapMotionManager;
    public MouseManager mouseManager;
	public TouchScreenManager touchScreenManager;

    private void Awake()
    {
        Instance = this;
    }

	//The script that these objects hold will, upon enable, try to stablish a connection with their target service.
    //This action (try to stablish a connection) should occur under two circumstances:
    // 1) The device is going to be used in a scene.
    // 2) The device is going to be configured.
    
    //The InterfaceManager has a function that activates all the objects set to be used in an evaluation (OnNewScene function) to fulfill case one.
    //To fulfill case two, upon entering configuration of each device, the button to open the configuration should access the InterfaceManager, the manager of the
    //      device group and the specific device itself to try to stablish a connection.


    /// <summary>
    /// Activates objects of device managers to stablish a connection with their services. If the device belongs to a group, it will ask the group to check.
    /// </summary>
    public void OnNewScene()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;

        //List of devices that do not belong to a group
        if (GLPlayerPrefs.GetBool(Scope, "useMouse"))
        {
            mouseManager.gameObject.SetActive(true);
        }

		if (GLPlayerPrefs.GetBool (Scope, "useTouchScreen"))
		{
			
			touchScreenManager.gameObject.SetActive (true);
		}

        if (GLPlayerPrefs.GetBool(Scope, "useLeapMotion"))
        {
            leapMotionManager.gameObject.SetActive(true);
        }

        //List of groups of devices that are asked to check
        eegManager.CheckInterfaces();

    }

    public void OnConfigScene()
    {
        mouseManager.gameObject.SetActive(false);
		touchScreenManager.gameObject.SetActive (false);
        leapMotionManager.gameObject.SetActive(false);
    }
}
