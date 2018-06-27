using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Gamelogic;

public class LeapMotionManager : MonoBehaviour {


	//LeapMotion Configuration
    public LeapHeadMountedRig leapMotionRig;
	public LeapHandController leapHandController;
	string Scope;

	public bool[] GestureActive;

	public Ray screenPointToRay;

	// Use this for initialization
    void Start () {
		
		LeapMotionRecognition ();
	}

	public void LeapMotionRecognition(){
	
		Scope = ProfileManager.Instance.currentEvaluationScope;
		Debug.Log ("Entre al leapMotionRecognition del leapmotionmanager");
		if (!GLPlayerPrefs.GetBool (Scope, "useLeapMotion") || (GLPlayerPrefs.GetBool (Scope, "useLeapMotion") && GLPlayerPrefs.GetBool (Scope, "useOcculusRift"))) {
			Debug.Log ("Entre al if del useLeapMotiongame falso");
			//leapHandController.gameObject.SetActive (true);

		}

		if (GLPlayerPrefs.GetBool (Scope, "useLeapMotion")) {
			Debug.Log ("Entre al if del useLeapMotion true");
			//leapHandController.gameObject.SetActive (false);

		}
	
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
