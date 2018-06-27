using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OpenGlove_API_C_Sharp_HL;
using OpenGlove_API_C_Sharp_HL.ServiceReference1;

namespace Memoria
{
    public class UnityHapticGlove : MonoBehaviour
    {

        public OpenGloveAPI openGloveAPI;
        public Glove gloveLeft = null;
        public Glove gloveRight = null;

        private DIOManager _dioManager;



        public void Initialize(DIOManager dioManager)
        {
            Debug.Log("inicializa");
            openGloveAPI = OpenGloveAPI.GetInstance();
            try
            {
                
                gloveRight = openGloveAPI.Devices[0];
                Debug.Log("Guante derecho esta conectado");
            }
            catch
            {
                Debug.Log("Guante derecho no esta conectado");
            }

            try
            {
                gloveLeft = openGloveAPI.Devices[1];
                Debug.Log("Guante izquierdo conectado");
            }
            catch
            {
                Debug.Log("Guante izquierdo no esta conectado");
            }

        }

        public void ActiveMotorRegions(int[] regions, int impact, OpenGlove_API_C_Sharp_HL.ServiceReference1.Glove glove)
        {
            if (glove != null)
            {
                foreach(int region in regions)
                {
                    openGloveAPI.Activate(glove, region, 255);
                }
                
            }
        }

        public IEnumerator DeactiveMotorRegions(float seconds, int[] regions, int impact, OpenGlove_API_C_Sharp_HL.ServiceReference1.Glove glove)
        {
            if (glove != null)
            {
                yield return new WaitForSeconds(seconds);

                foreach (int region in regions)
                {
                    openGloveAPI.Activate(glove, region, 0);
                }
            }
        }
        public void ActivateMotorIndex(string impact)
        {

            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerIndexDistal, int.Parse(impact));
        }

        public void DeactivateMotorIndex()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerIndexDistal, 0);
        }
        public void ActivateMotorMiddle(string impact)
        {
            Debug.Log("activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerMiddleDistal, int.Parse(impact));
        }

        public void DeactivateMotorMiddle()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerMiddleDistal, 0);
        }

        public void ActivateMotorThumb(string impact)
        {
            Debug.Log("activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerThumbDistal, int.Parse(impact));
        }

        public void DeactivateMotorThumb()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerThumbDistal, 0);
        }

        public void ActivateMotorPinky(string impact)
        {
            Debug.Log("activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerSmallDistal, int.Parse(impact));
        }

        public void DeactivateMotorPinky()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerSmallDistal, 0);
        }

        public void ActivateMotorRing(string impact)
        {
            Debug.Log("activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerRingDistal, int.Parse(impact));
        }

        public void DeactivateMotorRing()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.FingerRingDistal, 0);
        }

        public void ActivateMotorPalm(string impact)
        {
            Debug.Log("activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.ThenarIndex, int.Parse(impact));
        }

        public void DeactivateMotorPalm()
        {
            Debug.Log("de-activated");
            openGloveAPI.Activate(gloveRight, (int)PalmarRegion.ThenarIndex, 0);
        }
    }
}