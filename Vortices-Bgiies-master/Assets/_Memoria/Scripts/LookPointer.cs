using System;
using System.Collections;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using UnityEngine;

namespace Memoria
{
    public abstract class LookPointer : GLMonoBehaviour
    {
        protected DIOManager dioManager;

        public float _initialAlpha;
        [HideInInspector]
        public bool zoomingIn;
        [HideInInspector]
        public bool zoomingOut;

        [HideInInspector]
        public PitchGrabObject actualPitchGrabObject;
        [HideInInspector]
        public PitchGrabObject posibleActualPitchGrabObject;

        public Vector3 _actualPitchObjectOriginalPosition;
        public Quaternion _actualPitchObjectOriginalRotation;
        public Vector3 _actualPitchObjectOriginalScale;

        [SerializeField]
        public float _positionSteps = 1.0f;

        [SerializeField]
        public float _scaleSteps = 0.2f;

        public void Initialize(DIOManager fatherDioManager)
        {
            dioManager = fatherDioManager;
        }

        public void LookPointerEnter(PitchGrabObject pitchGrabObject)
        {
			//Debug.Log ("El pitchGrabOjbect es:" + pitchGrabObject);
			//Debug.Log("El valor del initial alpha antes es:" +_initialAlpha);
			_initialAlpha = pitchGrabObject.dioController.visualizationController.alpha;
			//Debug.Log("El valor del initial alpha despues es:" +_initialAlpha);
            var objectColor = pitchGrabObject.objectMeshRender.material.color;
			//Debug.Log ("El valor de objectColor es:" + objectColor);
            objectColor.a = 1.0f;
			//Debug.Log ("Ahora el valor de objecto colo es:" + objectColor);
            pitchGrabObject.objectMeshRender.material.color = objectColor;

            posibleActualPitchGrabObject = pitchGrabObject;
        }

        public void LookPointerExit(PitchGrabObject pitchGrabObject)
        {
            if (actualPitchGrabObject == null)
            {
				
                var objectColor = pitchGrabObject.objectMeshRender.material.color;
                //DELETE THIS untie from bgiies mode and tie to panel bgiies
                /*
                if (dioManager.bgiiesMode && dioManager.panelBgiies.mostrarCategoria)
                    objectColor.a = 0.66f;
                else*/
                    objectColor.a = _initialAlpha;
                pitchGrabObject.objectMeshRender.material.color = objectColor;
            }

            posibleActualPitchGrabObject = null;
        }
        public abstract void LookPointerStay(PitchGrabObject pitchGrabObject);
        public abstract void SetZoomInInitialStatus(PitchGrabObject pitchGrabObject);
        public abstract void DirectZoomInCall(Action finalAction);
        public abstract void DirectZoomInCall(PitchGrabObject pitchGrabObject, Action finalAction);
        public abstract IEnumerator ZoomingIn(PitchGrabObject pitchGrabObject, Action finalAction);
        public abstract void DirectZoomOutCall(Action finalAction);
        public abstract IEnumerator ZoomingOut(Action finalAction);


        public bool ZoomInKeyboardInput()
        {
            //if (dioManager.useKeyboard && !dioManager.useMouse)
            //{
            //	return Input.GetKeyDown(dioManager.action1Key);
            //}

            return false;
        }
    }
}
