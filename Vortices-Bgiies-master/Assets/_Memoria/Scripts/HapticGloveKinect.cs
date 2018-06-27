using System;
using Gamelogic;
using UnityCallbacks;
using UnityEngine;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;
using System.Linq;
using System.Collections;

namespace Memoria
{
    public class HapticGloveKinect : MonoBehaviour
    {
        public string impact = "255";
        private DIOManager _dioManager;

        UnityHapticGlove unityHapticGlove; 

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

            unityHapticGlove = _dioManager.unityHapticGlove;

        }


        public void DetectSelection()
        {
            if (unityHapticGlove.gloveRight != null)
            {
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerThumbDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerSmallDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerRingDistal, Int32.Parse(impact));

            }
        }

        public void NoDetectSelection()
        {
            if (unityHapticGlove.gloveRight != null)
            {
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, 0);
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, 0);
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerThumbDistal, 0);
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerSmallDistal, 0);
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerRingDistal, 0);
            }
        }

        public void DetectMoveVisualizationNext()
        {
            if (unityHapticGlove.gloveRight != null)
            {
                    unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                    unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
                
            }
        }

        public void DetectMoveVisualizationBack()
        {
            if (unityHapticGlove.gloveLeft != null)
            {
                    unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                    unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveLeft, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
                
            }
        }

        public void NoDetectVisualizationNext()
        {
            if(unityHapticGlove.gloveRight != null)
            { 
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
            }
        }
        public void NoDetectVisualizationBack()
        {
            if (unityHapticGlove.gloveLeft != null)
            {
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerIndexDistal, Int32.Parse(impact));
                unityHapticGlove.openGloveAPI.Activate(unityHapticGlove.gloveRight, (int)PalmarRegion.FingerMiddleDistal, Int32.Parse(impact));
            }
        }
    }
}