using Gamelogic;
using UnityEngine;

namespace Memoria.Core
{
	public class LeapHeadMountedRig : GLMonoBehaviour
	{
		public GameObject centerEyeAnchor;
		public GameObject leapSpace;
		public Camera leapCamera;
		string Scope;

		void Start(){

			LeapMotionRecognition ();
		}

		public void LeapMotionRecognition(){


			Scope = ProfileManager.Instance.currentEvaluationScope;

			bool leapmotion = GLPlayerPrefs.GetBool (Scope, "useLeapMotion");

			Debug.Log ("El booleano de leapmotion es:" + leapmotion);


			if (!GLPlayerPrefs.GetBool (Scope, "useLeapMotion")) {
				leapSpace.SetActive (false);
			}
			if(GLPlayerPrefs.GetBool (Scope, "useLeapMotion") && GLPlayerPrefs.GetBool (Scope, "useOcculusRift"))
			{
				leapSpace.SetActive (true);			
			}

		}
	}
}