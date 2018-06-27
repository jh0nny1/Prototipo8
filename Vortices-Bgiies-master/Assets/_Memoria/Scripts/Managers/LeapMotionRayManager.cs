using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionRayManager : MonoBehaviour {

	HandModel handmodel;
	Hand leap_hand;
	// Use this for initialization
	void Start () {
		handmodel = GetComponent<HandModel> ();
		leap_hand = handmodel.GetLeapHand ();
		if (leap_hand == null) {
			Debug.Log ("no leap hand founded");
		}
	}
	
	// Update is called once per frame
	void Update () {
		FingerModel finger = handmodel.fingers [1];
		Debug.DrawRay (finger.GetTipPosition (), finger.GetRay ().direction, Color.red);
		InterfaceManager.Instance.leapMotionManager.screenPointToRay = finger.GetRay ();
	}
}
