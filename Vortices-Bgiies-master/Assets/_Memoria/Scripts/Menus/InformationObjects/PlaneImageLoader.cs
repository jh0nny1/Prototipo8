using Memoria;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneImageLoader : MonoBehaviour {
    string[] planeImagesActionListNames = new string[] {
            "Select/Deselect image",
            "Zoom in image",
            "Zoom out image"};

    Action[] planeImagesActionList;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AsignActions(LookPointerVortices planeImage)
    {
        planeImagesActionList = new Action[]
        {
            null,
            () => planeImage.AcceptObject(),
            () => planeImage.DirectZoomInCall(null),
            () => planeImage.DirectZoomOutCall(null)
        };

    }

    
}
