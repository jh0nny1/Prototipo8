using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapMotionGestureManager : MonoBehaviour {

	public void LoveAndPeaceGestureTrue(){

		InterfaceManager.Instance.leapMotionManager.GestureActive[0] = true;

	}

	public void ThumbToTheLeftTrue(){

		InterfaceManager.Instance.leapMotionManager.GestureActive[1] = true;
	}

	public void ThumbDownTrue(){
	
		InterfaceManager.Instance.leapMotionManager.GestureActive [2] = true;
	}

	public void OpenHandTrue(){

		InterfaceManager.Instance.leapMotionManager.GestureActive [3] = true;
	}

	public void ClosedHandTrue(){

		InterfaceManager.Instance.leapMotionManager.GestureActive [4] = true;
	}

	/*
	public void PruebaDeGestoTrue(){

		Debug.Log ("Hice el gesto");
	}

	public void PruebaDeGestoFalse(){
		Debug.Log ("Deje de hacer el gesto");
	}
	*/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
