using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectMappingLoader : MonoBehaviour {

    string interfaceName = "Kinect";

    int[] actionIndex;

    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {

    }

    #region Legacy scripts

    /* Code in Update function in LookPointerBGIIES used only to zoom out of image, which should not exist given the ActionManager architecture
            //this update triggers the zoom out if the right click is pressed or if the kinect gesture zoom out has been triggered.
            if ((Input.GetMouseButtonDown(1) || dioManager.kinectGestures.kinectGestureZoomOut()) && actualPitchGrabObject != null && !dioManager.movingPlane)
            {
                if (!zoomingOut && !zoomingIn)
                {
                    if (zoomActive)
                    {
                        StartCoroutine(ZoomingOut(null));
                        //dioManager.panelBgiies.noInteractableButtons();
                        zoomActive = false;
                    }
                }

            }
            */

    /* Code inside the lookPointer loop [in LookPointerBGIIES], triggered by the OnDetect function in pitchGrabObject, which is triggered by the lookPointerRaycasting if a new object is detected.
             * 
            if ((Input.GetMouseButtonDown(0) || dioManager.kinectGestures.kinectGestureZoomIn()) && actualPitchGrabObject == null && !dioManager.movingPlane)
            {
                if (!zoomingIn && !zoomingOut)
                {
                    if (!zoomActive)
                    {
                        StartCoroutine(ZoomingIn(pitchGrabObject, null));
                        dioManager.panelBgiies.interactableButtons(posibleActualPitchGrabObject);
                        zoomActive = true;
                    }
                }
            }
            */

    #endregion
}
