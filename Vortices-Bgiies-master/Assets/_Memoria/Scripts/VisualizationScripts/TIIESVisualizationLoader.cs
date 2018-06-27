using Gamelogic;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class TIIESVisualizationLoader : MonoBehaviour {

	//LeapMotion Configuration
	public LeapHeadMountedRig leapMotionRig;
	public LeapHandController leapHandController;


	public void LoadInstances()
	{
		InterfaceManager.Instance.leapMotionManager.leapMotionRig = leapMotionRig;
		InterfaceManager.Instance.leapMotionManager.leapHandController = leapHandController;


		string Scope = ProfileManager.Instance.currentEvaluationScope;


		if (GLPlayerPrefs.GetBool(Scope, "useMouse"))
		{
			Debug.Log ("entre al LoadInstance del PlaneVisualization del TIIESVisualization con mouse");
			InteractionManager.Instance.updateList.Add(() =>
				InteractionManager.Instance.raycastingSpherePlane.CreateRayCategories(
					InterfaceManager.Instance.mouseManager.screenPointToRay, VisualizationManager.Instance.planeVisualization.actualVisualization)
			);
		}

		if (GLPlayerPrefs.GetBool (Scope, "useTouchScreen")) {
			Debug.Log ("Entre al LoadInstance del PlanceVisualization con la interfaz touch");
			InteractionManager.Instance.updateList.Add (() =>
				InteractionManager.Instance.raycastingSpherePlane.CreateRayCategories (
					InterfaceManager.Instance.touchScreenManager.screenPointToRay, VisualizationManager.Instance.planeVisualization.actualVisualization)
			);
		}

		if (GLPlayerPrefs.GetBool (Scope, "useLeapMotion")) {
		
			leapHandController.gameObject.SetActive (true);
			Debug.Log ("entre al LoadInstance del PlaneVisualization del TIIESVisualization con la opcion leapmotion");
			InteractionManager.Instance.updateList.Add (() =>
				InteractionManager.Instance.raycastingSpherePlane.CreateRayCategories (
				InterfaceManager.Instance.leapMotionManager.screenPointToRay, VisualizationManager.Instance.planeVisualization.actualVisualization)
			);
		}







	}


}
