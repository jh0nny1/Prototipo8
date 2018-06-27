using System;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;
using System.Linq;

namespace Memoria
{
	public class HapticDetector : GLMonoBehaviour, IOnValidate, IOnTriggerEnter, IOnTriggerExit
	{
		public string impact = "255";
		private DIOManager _dioManager;

		public void OnValidate()
		{
			try
			{
				var impactValue = Convert.ToInt32(impact);
				impactValue = Mathf.Clamp(impactValue, 0, 255);

				impact = impactValue.ToString();
			}
			catch (Exception)
			{
				impact = "255";
			}
		}

		public void Initialize(DIOManager dioManager)
		{
			_dioManager = dioManager;
		}

        public void OnTriggerEnter(Collider other)
        {
            if (_dioManager == null)
                return;

            if (!_dioManager.useLeapMotion || !_dioManager.useHapticGlove)
                return;

            var handMapping = other.GetComponent<HandMapping>();

            if (handMapping == null)
                return;

            var unityHapticGlove = _dioManager.unityHapticGlove;
            if (unityHapticGlove.gloveLeft != null)
            {
                switch (handMapping.handMap)
                {
                    case HandMap.LeftIndex:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftIndex(impact);
                        break;
                    case HandMap.LeftMiddle:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftMiddle(impact);
                        break;
                    case HandMap.LeftThumb:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerThumbDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftThumb(impact);
                        break;
                    case HandMap.LeftPinky:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerSmallDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftPinky(impact);
                        break;
                    case HandMap.LeftRing:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerRingDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftRing(impact);
                        break;
                    case HandMap.LeftPalm:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.ThenarIndex, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorLeftPalm(impact);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if(unityHapticGlove.gloveRight != null)
            {
                switch (handMapping.handMap)
                {
                    case HandMap.RightIndex:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightIndex(impact);
                        break;
                    case HandMap.RightMiddle:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightMiddle(impact);
                        break;
                    case HandMap.RightThumb:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerThumbDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightThumb(impact);
                        break;
                    case HandMap.RightPinky:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerSmallDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightPinky(impact);
                        break;
                    case HandMap.RightRing:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerRingDistal, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightRing(impact);
                        break;
                    case HandMap.RightPalm:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.ThenarIndex, Int32.Parse(impact));
                        //unityHapticGlove.ActivateMotorRightPalm(impact);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

		public void OnTriggerExit(Collider other)
		{
			if (!_dioManager.useLeapMotion || !_dioManager.useHapticGlove)
				return;

			var handMapping = other.GetComponent<HandMapping>();

			if (handMapping == null)
				return;

			var unityHapticGlove = _dioManager.unityHapticGlove;
            if (unityHapticGlove.gloveLeft != null)
            {
                switch (handMapping.handMap)
                {

                    case HandMap.LeftIndex:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerIndexDistal, 0);
                        break;
                    case HandMap.LeftMiddle:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerMiddleDistal, 0);
                        break;
                    case HandMap.LeftThumb:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerThumbDistal, 0);
                        break;
                    case HandMap.LeftPinky:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerSmallDistal, 0);
                        break;
                    case HandMap.LeftRing:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerRingDistal, 0);
                        break;
                    case HandMap.LeftPalm:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.ThenarIndex, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (unityHapticGlove.gloveRight != null)
            {
                switch (handMapping.handMap)
                {
                    case HandMap.RightIndex:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, 0);
                        break;
                    case HandMap.RightMiddle:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, 0);
                        break;
                    case HandMap.RightThumb:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerThumbDistal, 0);
                        break;
                    case HandMap.RightPinky:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerSmallDistal, 0);
                        break;
                    case HandMap.RightRing:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerRingDistal, 0);
                        break;
                    case HandMap.RightPalm:
                        unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.ThenarIndex, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
		}
	}
}