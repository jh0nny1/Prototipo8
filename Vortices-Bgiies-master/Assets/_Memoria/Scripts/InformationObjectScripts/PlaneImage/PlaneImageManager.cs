using Gamelogic;
using Leap.Unity;
using Memoria;
using Memoria.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneImageManager : GLMonoBehaviour {

    //This shows the black screen with the loading percentage, and deactivates the canvas at the end.
    public LoadingScene loadingScene;
    public Text visualizationCounter;

    //Controller that loads the images into the objects
    public LoadImagesController loadImageController;

    //Game object with the required scripts to show the loading canvas and to load the images
    public GameObject planeImagesUtilities;

    //
    public DIOController informationPrefab;
    //DELETE THIS i don't really believe this should even exist, there are only plane images, what's the point?
    public DIOController informationPlanePrefab;

    //Lookpointer has the actions
    //[HideInInspector]
    public LookPointerVortices lookPointerInstance;
    public LookPointerVortices lookPointerPrefab;
    //A combination of the lookPointerBGIIES into the PlaneVisualizationManager to make a single working code and untie visualization-related actions from the
    //      objects should be considered. 
    //For now, the BGIIES actions will be asigned as the Visualization actions through the Plane Image Manager because even though they should be tied to the Plane visualization
    //      they're declared in the LookPointerBGIIES script, who then accesses the plane visualization. 
    //NOTE: ONLY the category-related functions, not the selection and zoom-related ones
    //[HideInInspector]
    public LookPointerBGIIES lookPointerInstanceBGIIES;
    public LookPointerBGIIES lookPointerBgiiesPrefab;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;
    public Vector3 lookPointerScale = new Vector3(0.005f,0.005f,0.005f);


	public LookPointerTIIES lookPointerInstanceTIIES;
	public LookPointerTIIES lookPointerTiiesPrefab;

    //initialization for Sphere visualization
    public void Initialize()
    {
        planeImagesUtilities.SetActive(true);
        loadImageController.Initialize();
        LoadPreferences();
    }

    void LoadPreferences()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        loadImageController.images = Convert.ToInt32(GLPlayerPrefs.GetString(Scope, "PlaneImageAmount"));
        loadImageController.LoadImageBehaviour.pathImageAssets = GLPlayerPrefs.GetString(Scope, "PlaneImageFolderPath");
        //loadImageController.LoadImageBehaviour.pathSmall = GLPlayerPrefs.GetString(Scope, "FolderSmallText");
        loadImageController.LoadImageBehaviour.pathSmall = "";
        loadImageController.LoadImageBehaviour.filename = GLPlayerPrefs.GetString(Scope, "PlaneImagePrefix");        
    }

    public void LoadLookPointerActions(List<Tuple<float, float>> radiusAlphaVisualizationListParam)
    {
        radiusAlphaVisualizationList = radiusAlphaVisualizationListParam;
        var lookPointerPosition = new Vector3(0.0f, 0.0f, radiusAlphaVisualizationList[1].First);
        lookPointerInstance = Instantiate(lookPointerPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition, Quaternion.identity);
        lookPointerInstance.transform.localScale = lookPointerScale;

        Action[] objectActions = new Action[]
            {
                () => lookPointerInstance.AcceptObject(),
                () => lookPointerInstance.DirectZoomInCall(null),
                () => lookPointerInstance.DirectZoomOutCall(null)
            };
        ActionManager.Instance.ReloadObjectActions(objectActions);
        MOTIONSManager.Instance.informationObjectInitialized = true;
        MOTIONSManager.Instance.CheckActionManagerInitialization();
    }

    public bool LoadLookPointerBGIIESActions(List<Tuple<float, float>> radiusAlphaVisualizationListParam)
    {
        radiusAlphaVisualizationList = radiusAlphaVisualizationListParam;
        var lookPointerPosition = new Vector3(0.0f, 0.0f, radiusAlphaVisualizationList[1].First);
        lookPointerInstanceBGIIES = Instantiate(lookPointerBgiiesPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition, Quaternion.identity);
        lookPointerInstanceBGIIES.transform.localScale = lookPointerScale;

        Action[] objectActions = new Action[]
            {
                //DELETE THIS the information object description will now never use the select/deselect because
                //     it's not asigned to actually work in the plane visualization, so instead there's a null.
                //It could be added thou, not much work in it
                null,
                () => lookPointerInstanceBGIIES.DirectZoomInCall(null),
                () => lookPointerInstanceBGIIES.DirectZoomOutCall(null)
            };

        ActionManager.Instance.ReloadObjectActions(objectActions);
        MOTIONSManager.Instance.informationObjectInitialized = true;
        MOTIONSManager.Instance.CheckActionManagerInitialization();
        return true;
    }

	public bool LoadLookPointerTIIESActions(List<Tuple<float, float>> radiusAlphaVisualizationParam)
	{
		radiusAlphaVisualizationList = radiusAlphaVisualizationParam;
		var lookPointerPosition = new Vector3(0.0f, 0.0f, radiusAlphaVisualizationList[1].First);
		//lookPointerInstanceTIIES = Instantiate (lookPointerTiiesPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition, Quaternion.identity);
		lookPointerInstanceTIIES = Instantiate(lookPointerTiiesPrefab, InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor, lookPointerPosition,  Quaternion.identity);
		lookPointerInstanceTIIES.transform.localScale = lookPointerScale;

		Action[] objectActions = new Action[]
		{
			null,
			() => lookPointerInstanceTIIES.DirectZoomInCall(null),
			() => lookPointerInstanceTIIES.DirectZoomOutCall(null)

		};

		ActionManager.Instance.ReloadObjectActions(objectActions);
		MOTIONSManager.Instance.informationObjectInitialized = true;
		MOTIONSManager.Instance.CheckActionManagerInitialization();
		return true;

	}

    public void LoadObjects(List<DIOController> listOfDio)
    {
		loadingScene.Initialize();
        StartCoroutine(loadImageController.LoadFolderImages(listOfDio));
    }
}
