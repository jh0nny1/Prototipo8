using Memoria;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using System;
using System.Linq;
using UnityCallbacks;

public class SphereVisualizationManager : GLMonoBehaviour
{

    //Sphere Configuration    
    public SphereController spherePrefab;
    public List<SphereController> sphereControllers;
    public SphereVisualizationLoader loader;
    

    // variables
    public int actualVisualization;
    public List<Tuple<float, float>> radiusAlphaVisualizationList;
    public bool movingSphere;
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 1.0f;
    public float radiusFactor = 0.005f;
    public float radiusSpeed = 2.0f;
    public float alphaFactor = 0.02f;
    public float alphaSpeed = 2.0f;
    public float alphaWaitTime = 0.8f;

    public bool InLastVisualization
    {
        get { return actualVisualization == (sphereControllers.Count - 1 ); }
    }

    Action[] visualizationActions;

    public bool AreAllDioOnSphere
    {
        get
        {
            var fullDioList = sphereControllers.SelectMany(s => s.dioControllerList);
            return fullDioList.All(dio => dio.inVisualizationPosition);
        }
    }


    // Use this for initialization
    void Start () {
        movingSphere = false;
        string currentObject = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentInformationObject");
        loader.LoadInstances();
        //plane image behaviour
        if (currentObject.Equals("PlaneImage"))
        {
            //initialization of all variables and plane images
            var visualizationTextureIndex = 0;
            var visualizationIndex = 0;
            actualVisualization = 0;
            InformationObjectManager.Instance.planeImages.Initialize();
            radiusAlphaVisualizationList = new List<Tuple<float, float>> { Tuple.New(0.0f, 0.0f) };
            
            //Auto-tune of spheres given the amount of images to load
            AutoTuneSpheresForImages(InformationObjectManager.Instance.planeImages.loadImageController.images);
            foreach (var sphereController in sphereControllers)
            {
                sphereController.InitializeDioControllers(visualizationIndex, transform.position, visualizationTextureIndex, true);
                radiusAlphaVisualizationList.Add(Tuple.New(sphereController.sphereRadius, sphereController.alpha));

                visualizationTextureIndex += sphereController.elementsToDisplay;
                visualizationIndex += 1;
            }

            //look pointer is initialized given the spheres generated
            InformationObjectManager.Instance.planeImages.LoadLookPointerActions(radiusAlphaVisualizationList);
            //images are loaded
            InformationObjectManager.Instance.planeImages.LoadObjects(sphereControllers.SelectMany(sc => sc.dioControllerList).ToList());

            //Sphere actions are asigned to the action manager
            visualizationActions = new Action[]
            {
                () => MoveSphereInside(1, null, null),
                () => MoveSphereOutside(1, null, null)
            };
            //ActionManager.Instance.currentVisualizationActions = new Action[visualizationActions.Length];
            //visualizationActions.CopyTo(ActionManager.Instance.currentVisualizationActions,0);
            ActionManager.Instance.ReloadVisualizationActions(visualizationActions);

            MOTIONSManager.Instance.visualizationInitialized = true;
            MOTIONSManager.Instance.CheckActionManagerInitialization();

            //DELETE THIS only for testing, awful way to do it
            //InterfaceManager.Instance.leapMotionManager.leapMotionRig.centerEyeAnchor.GetComponent<LookPointerRaycasting>().Initialize();
        }        
        
    }
	
    

    public void AutoTuneSpheresForImages(int objectAmount)
    {
        //This 39 represents an ideal maximum amount of images in a single sphere considering the size of the images, but
        //  it can can should change depending on the object size. 
        //Use this as reference and play with the "39" value, object amount, alpha and radius. And of course, the IdealSphereConfiguration. 
        int sphereToShow = objectAmount / 39;
        int extraImages = objectAmount % 39;

        if (extraImages != 0)
            sphereToShow++;
        else
            extraImages = 39;

        if (sphereControllers != null)
        {
            foreach (var sphereController in sphereControllers)
            {
                DestroyImmediate(sphereController.gameObject);
            }
        }

        sphereControllers = new List<SphereController>();

        for (int i = 0; i < sphereToShow; i++)
        {
            var newAlpha = 0.7f - 0.3f * i;
            var newRadius = 0.45f + 0.15f * i;

            if (newAlpha < 0.0f)
                newAlpha = 0.0f;

            var newSphere = IdealSphereConfiguration(
                i == sphereToShow - 1 ? extraImages : 39,
                newRadius,
                newAlpha);

            sphereControllers.Add(newSphere);
        }
    }

    SphereController IdealSphereConfiguration(int elements, float radius, float alpha)
    {
        //Just like the AutoTuneSphereForImages, each row here considers the 39 objects inside a sphere (there are 3 rows, 13 * 3 = 39).
        //If a different object is added, change the 13 to better fit the new objects, and change the rowHightDistance, RadiusDifference and scaleFactor. Figure out
        //  by yourself what these values change, as 3D changes are difficult to explain with words.
        var rows = elements / 13;

        if (elements % 13 != 0)
            rows++;

        var sphereController = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity);
        sphereController.transform.parent = transform;
        sphereController.transform.ResetLocal();

        sphereController.elementsToDisplay = elements;
        sphereController.visualizationRow = rows;
        sphereController.rowHightDistance = 0.2f;
        sphereController.rowRadiusDifference = 0.05f;
        sphereController.scaleFactor = new Vector3(0.2f, 0.2f, 0.001f);
        sphereController.sphereRadius = radius;
        sphereController.alpha = alpha;
        sphereController.autoAngleDistance = true;
        sphereController.debugGizmo = false;

        return sphereController;
    }

#region Sphere Methods
    public void MoveSphereHorizontal(float horizontalAxis)
    {
        var sphereTransform = sphereControllers[actualVisualization].transform;

        sphereTransform.Rotate(Vector3.down * horizontalSpeed * horizontalAxis, Space.Self);
    }

    private int _sphereVerticalCounter = 50;
    public void MoveSphereVertical(float verticalAxis)
    {
        if (verticalAxis == 1.0f && _sphereVerticalCounter >= 100)
        {
            _sphereVerticalCounter = 100;
            return;
        }

        if (verticalAxis == -1.0f && _sphereVerticalCounter <= 0)
        {
            _sphereVerticalCounter = 0;
            return;
        }

        var sphereTransform = sphereControllers[actualVisualization].transform;

        sphereTransform.Rotate(Vector3.right * verticalSpeed * verticalAxis, Space.World);

        _sphereVerticalCounter += (int)verticalAxis;
    }

    public void MoveSphereInside(float insideAxis, Action initialAction, Action finalAction)
    {
        Debug.Log("movesphereinside");
        var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstance.actualPitchGrabObject;
        var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstance.zoomingIn;
        var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstance.zoomingOut;
        if (insideAxis == 1.0f && !movingSphere && actualPitchGrabObject == null &&
            !zoomingIn && !zoomingOut && AreAllDioOnSphere)
        {
            StartCoroutine(MoveSphereInside(initialAction, finalAction));
        }
        else
        {
            if (finalAction != null)
                finalAction();
        }
    }

    private IEnumerator MoveSphereInside(Action initialAction, Action finalAction)
    {
        if (movingSphere)
            yield break;

        movingSphere = true;

        var notInZeroSphereControllers =
        sphereControllers.Where(
            sphereController =>
                sphereController.notInZero
            ).ToList();



        if (notInZeroSphereControllers.Count == 1)
        {
            movingSphere = false;

            yield break;
        }

        var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
        for (int i = 0; i < notInZeroSphereControllers.Count; i++)
        {
            radiusAlphaTargetReached.Add(Tuple.New(false, false));
        }

        var actualRadiusFactor = radiusFactor * -1;
        MOTIONSManager.Instance.AddLines("Changing Sphere", (actualVisualization + 2).ToString());

        if (initialAction != null)
            initialAction();

        while (true)
        {
            for (int i = 0; i < notInZeroSphereControllers.Count; i++)
            {
                var sphereController = notInZeroSphereControllers[i];
                var radiusTargetReached = false;
                var alphaTargerReached = false;

                //Radius
                var targetRadius = radiusAlphaVisualizationList[i].First;
                sphereController.sphereRadius += actualRadiusFactor * radiusSpeed;

                if (TargetReached(actualRadiusFactor, sphereController.sphereRadius, targetRadius))
                {
                    radiusTargetReached = true;
                    sphereController.sphereRadius = targetRadius;
                }

                //Alpha
                var actualAlphaFactor = i == 0 ? alphaFactor * -1 : alphaFactor;
                var targetAlpha = radiusAlphaVisualizationList[i].Second;
                sphereController.alpha += actualAlphaFactor * alphaSpeed;

                if (TargetReached(actualAlphaFactor, sphereController.alpha, targetAlpha))
                {
                    alphaTargerReached = true;
                    sphereController.alpha = targetAlpha;
                }

                sphereController.ChangeVisualizationConfiguration(transform.position, sphereController.sphereRadius,
                    sphereController.alpha);
                radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
            }

            if (radiusAlphaTargetReached.All(t => t.First && t.Second))
                break;

            yield return new WaitForFixedUpdate();
        }

        sphereControllers[actualVisualization].notInZero = false;
        sphereControllers[actualVisualization].gameObject.SetActive(false);
        actualVisualization++;

        if (finalAction != null)
            finalAction();

        movingSphere = false;
    }

    public void MoveSphereOutside(float outsideAxis, Action initialAction, Action finalAction)
    {
        var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstance.actualPitchGrabObject;
        var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstance.zoomingIn;
        var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstance.zoomingOut;

        if (outsideAxis == 1.0f && !movingSphere && actualPitchGrabObject == null &&
            !zoomingIn && !zoomingOut && AreAllDioOnSphere)
        {
            StartCoroutine(MoveSphereOutside(initialAction, finalAction));
        }
        else
        {
            if (finalAction != null)
                finalAction();
        }
    }

    private IEnumerator MoveSphereOutside(Action initialAction, Action finalAction)
    {
        if (movingSphere)
            yield break;

        movingSphere = true;

        var notInZeroSphereControllers =
            sphereControllers.Where(
                sphereController =>
                    sphereController.notInZero
                ).ToList();

        var inZeroSphereControllers =
            sphereControllers.Where(
                sphereController =>
                    !sphereController.notInZero
                ).ToList();

        if (inZeroSphereControllers.Count == 0)
        {
            movingSphere = false;

            yield break;
        }

        var sphereControllerList = new List<SphereController> { inZeroSphereControllers.Last() };
        sphereControllerList.AddRange(notInZeroSphereControllers);

        var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
        for (int i = 0; i < sphereControllerList.Count; i++)
        {
            radiusAlphaTargetReached.Add(Tuple.New(false, false));
        }

        sphereControllers[actualVisualization - 1].gameObject.SetActive(true);
        MOTIONSManager.Instance.AddLines("Changing Sphere", actualVisualization.ToString());

        if (initialAction != null)
            initialAction();

        var alphaWaitTimeCounter = 0.0f;
        while (true)
        {
            for (int i = 0; i < sphereControllerList.Count; i++)
            {
                var sphereController = sphereControllerList[i];
                var radiusTargetReached = false;
                var alphaTargerReached = false;

                //Radius
                var targetRadius = radiusAlphaVisualizationList[i + 1].First;
                sphereController.sphereRadius += radiusFactor * radiusSpeed;

                if (TargetReached(radiusFactor, sphereController.sphereRadius, targetRadius))
                {
                    radiusTargetReached = true;
                    sphereController.sphereRadius = targetRadius;
                }

                if (alphaWaitTimeCounter >= alphaWaitTime)
                {
                    //Alpha
                    var actualAlphaFactor = i == 0
                        ? alphaFactor
                        : alphaFactor * -1;
                    var targetAlpha = radiusAlphaVisualizationList[i + 1].Second;
                    sphereController.alpha += actualAlphaFactor * alphaSpeed;

                    if (TargetReached(actualAlphaFactor, sphereController.alpha, targetAlpha))
                    {
                        alphaTargerReached = true;
                        sphereController.alpha = targetAlpha;
                    }
                }
                alphaWaitTimeCounter += Time.fixedDeltaTime;
                sphereController.ChangeVisualizationConfiguration(transform.position, sphereController.sphereRadius, sphereController.alpha);
                radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
            }

            if (radiusAlphaTargetReached.All(t => t.First && t.Second))
                break;

            yield return new WaitForFixedUpdate();
        }

        actualVisualization--;
        sphereControllers[actualVisualization].notInZero = true;

        if (finalAction != null)
            finalAction();

        movingSphere = false;
    }
#endregion

    private bool TargetReached(float factor, float value, float target)
    {
        if (factor >= 0)
        {
            if (value >= target)
            {
                return true;
            }
        }
        else
        {
            if (value <= target)
            {
                return true;
            }
        }

        return false;
    }


}
