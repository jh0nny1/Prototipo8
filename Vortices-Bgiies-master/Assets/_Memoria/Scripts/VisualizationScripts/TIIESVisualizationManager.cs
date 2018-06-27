using Memoria;
using Memoria.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using System.Linq;
using System;

public class TIIESVisualizationManager : GLMonoBehaviour {

	//Plane Configuration

	public TIIESController Tiiesprefab;
	public List<TIIESController> tiiesControllers;
	public TIIESVisualizationLoader loader;

	public GameObject marcador;
	public ButtonPanelTIIES buttonTiies;

	//variables
	public int actualVisualization;
	public List<Tuple<float, float>> radiusAlphaVisualizationList;
	public bool movingPlane;

	public float horizontalSpeed = 2.0f;
	public float verticalSpeed = 1.0f;
	public float radiusFactor = 0.5f;
	public float radiusSpeed = 20.0f;
	public float alphaFactor = 0.3f;
	public float alphaSpeed = 2.0f;
	public float alphaWaitTime = 0.0f;

	public bool InLastVisualization
	{
		get { return actualVisualization == (tiiesControllers.Count - 1); }
	}

	Action[] visualizationActions;

	public bool AreAllDioOnSphere
	{
		get
		{
			var fullDioList = tiiesControllers.SelectMany(s => s.dioControllerList);
			return fullDioList.All(dio => dio.inVisualizationPosition);
		}
	}

	// Use this for initialization
	void Start () {

		movingPlane = false;
		string currentObject = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "CurrentInformationObject");
		loader.LoadInstances();
		//plane image behaviour
		if (currentObject.Equals("PlaneImage"))
		{
			var visualizationTextureIndex = 0;
			var visualizationIndex = 0;
			actualVisualization = 0;
			InformationObjectManager.Instance.planeImages.Initialize();
			radiusAlphaVisualizationList = new List<Tuple<float, float>> { Tuple.New(0.0f, 0.0f) };
			AutoTunePlanes(InformationObjectManager.Instance.planeImages.loadImageController.images);



			foreach (var tiiesController in tiiesControllers)
			{
				tiiesController.InitializeDioControllers( visualizationIndex, transform.position, visualizationTextureIndex, true);
				radiusAlphaVisualizationList.Add(Tuple.New(tiiesController.distance, tiiesController.alpha));

				visualizationTextureIndex += tiiesController.elementsToDisplay;
				visualizationIndex += 1;
			}

			InformationObjectManager.Instance.planeImages.LoadLookPointerTIIESActions (radiusAlphaVisualizationList);

			InformationObjectManager.Instance.planeImages.LoadObjects(tiiesControllers.SelectMany(sc => sc.dioControllerList).ToList());

			visualizationActions = new Action[] {
				() => InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.SelectCat1(),
				() => InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.SelectCat2(),
				() => InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.SelectCat3(),
				() => InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.SelectCat4(),
				() => buttonTiies.selectButton1(),
				() => buttonTiies.SelectButton2(),
				() => buttonTiies.SelectButton3(),
				() => buttonTiies.SelectButton4(),
				() => buttonTiies.Inside (),
				() => buttonTiies.Outside ()
				
			};

			ActionManager.Instance.ReloadVisualizationActions (visualizationActions);
			MOTIONSManager.Instance.visualizationInitialized = true;
			MOTIONSManager.Instance.CheckActionManagerInitialization();

		}
		
	}

	private void AutoTunePlanes(int objAmount)
	{


		int planesToShow =  objAmount / 12;
		int extraImages = objAmount % 12;

	

		if (extraImages != 0)
			planesToShow++;
		else
			extraImages = 12;



		if (tiiesControllers != null)
		{
			foreach (var tiiesController in tiiesControllers)
			{
				DestroyImmediate(tiiesController.gameObject);
			}
		}

		tiiesControllers = new List<TIIESController>();

		for (int i = 0; i < planesToShow; i++)
		{
			var newAlpha = 1.0f - 0.3f;
			var newRadius = 0.45f + 0.15f * i;

			if (newAlpha < 0.0f)
				newAlpha = 0.0f;

			var newPlane = IdealPlaneConfiguration(
				i == planesToShow - 1 ? extraImages : 12,
				newRadius,
				newAlpha);

			tiiesControllers.Add(newPlane);
		}
	}

	private TIIESController IdealPlaneConfiguration(int elements, float distance, float alpha)
	{

		var rows = elements / 4;



		if (elements % 4 != 0)
			rows++;



		var tiiesController = Instantiate(Tiiesprefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
		tiiesController.transform.parent = transform;      //hace que transform (DIOmanager) sea el padre de visualizationController (esfera actual)

		tiiesController.transform.ResetLocal();
		tiiesController.elementsToDisplay = elements;

		tiiesController.visualizationRow = rows;

		tiiesController.rowHightDistance = 0.035f;
		tiiesController.rowDistanceDifference = 0f;

		tiiesController.scaleFactor = new Vector3(0.03f, 0.03f, 0.00001f);     //multiplicador para cambiar el tamaño de cada elemento, asegurando que todos se ven iguales.
		tiiesController.distance = distance;
		tiiesController.alpha = alpha;
		tiiesController.autoAngleDistance = true;


		tiiesController.debugGizmo = false;

		return tiiesController;
	}

	#region Plane Methods

	public void MovePlaneInside(float insideAxis, Action initialAction, Action finalAction)
	{
		var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.actualPitchGrabObject;
		var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.zoomingIn;
		var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.zoomingOut;

		if (insideAxis == 1.0f && !movingPlane && actualPitchGrabObject == null &&
			!zoomingIn && !zoomingOut)
		{
			StartCoroutine(MovePlaneInside(initialAction, finalAction));
		}
		else
		{
			if (finalAction != null)
				finalAction();
		}
	}

	private IEnumerator MovePlaneInside(Action initialAction, Action finalAction)
	{
		if (movingPlane)
			yield break;

		movingPlane = true;

		var notInZeroTiiesControllers = tiiesControllers.Where (tiiesController => tiiesController.notInZero).ToList ();

		if (notInZeroTiiesControllers.Count == 1)
		{
			movingPlane = false;

			yield break;
		}

		var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
		for (int i = 0; i < notInZeroTiiesControllers.Count; i++)
		{
			radiusAlphaTargetReached.Add(Tuple.New(false, false));
		}

		var actualRadiusFactor = radiusFactor * -1;
		MOTIONSManager.Instance.AddLines("Changing Plane", (actualVisualization + 2).ToString());

		if (initialAction != null)
			initialAction();

		while (true)
		{
			for (int i = 0; i < notInZeroTiiesControllers.Count; i++)
			{
				var tiiesController = notInZeroTiiesControllers[i];
				var radiusTargetReached = false;
				var alphaTargerReached = false;

				//Radius
				var targetRadius = radiusAlphaVisualizationList[i].First;
				tiiesController.distance += actualRadiusFactor * radiusSpeed;

				if (TargetReached(actualRadiusFactor, tiiesController.distance, targetRadius))
				{
					radiusTargetReached = true;
					tiiesController.distance = targetRadius;
				}

				//Alpha
				//var actualAlphaFactor = i == 0 ? alphaFactor * -1 : alphaFactor;
				var targetAlpha = radiusAlphaVisualizationList[i].Second;
				//tiiesController.alpha += actualAlphaFactor * alphaSpeed;

				//if (TargetReached(actualAlphaFactor, tiiesController.alpha, targetAlpha))
				//{
				//	alphaTargerReached = true;
				//	tiiesController.alpha = targetAlpha;
				//}
				alphaTargerReached = true;
				tiiesController.ChangeVisualizationConfiguration (transform.position, tiiesController.distance, tiiesController.alpha);

				radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
			}

			if (radiusAlphaTargetReached.All(t => t.First && t.Second))
				break;

			yield return new WaitForFixedUpdate();
		}

		tiiesControllers [actualVisualization].notInZero = false;

		actualVisualization++;

		if (finalAction != null)
			finalAction();

		movingPlane = false;
	}


	public void MovePlaneOutside(float outsideAxis, Action initialAction, Action finalAction)
	{
		var actualPitchGrabObject = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.actualPitchGrabObject;
		var zoomingIn = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.zoomingIn;
		var zoomingOut = InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.zoomingOut;
		if (outsideAxis == 1.0f && !movingPlane && actualPitchGrabObject == null &&
			!zoomingIn && !zoomingOut)
		{
			StartCoroutine(MovePlaneOutside(initialAction, finalAction));
		}
		else
		{
			if (finalAction != null)
				finalAction();
		}
	}


	private IEnumerator MovePlaneOutside(Action initialAction, Action finalAction)
	{
		if (movingPlane)
			yield break;

		movingPlane = true;

		var notInZeroTiiesControllers = tiiesControllers.Where (tiiesController => tiiesController.notInZero).ToList ();

		var inZeroTiiesControllers = tiiesControllers.Where (tiiesController => !tiiesController.notInZero).ToList ();

		if (inZeroTiiesControllers.Count == 0)
		{
			movingPlane = false;

			yield break;
		}

		var tiiesControllerList = new List<TIIESController>{ inZeroTiiesControllers.Last () };
		tiiesControllerList.AddRange (notInZeroTiiesControllers);

		var radiusAlphaTargetReached = new List<Tuple<bool, bool>>();
		for (int i = 0; i < tiiesControllerList.Count; i++)
		{
			radiusAlphaTargetReached.Add(Tuple.New(false, false));
		}

		tiiesControllers [actualVisualization - 1].gameObject.SetActive (true);
		MOTIONSManager.Instance.AddLines("Changing Plane", actualVisualization.ToString());

		if (initialAction != null)
			initialAction();

		var alphaWaitTimeCounter = 0.0f;
		while (true)
		{
			for (int i = 0; i < tiiesControllerList.Count; i++)
			{
				var tiiesController = tiiesControllerList[i];
				var radiusTargetReached = false;
				var alphaTargerReached = false;

				//Radius
				var targetRadius = radiusAlphaVisualizationList[i + 1].First;
				tiiesController.distance += radiusFactor * radiusSpeed;

				if (TargetReached(radiusFactor, tiiesController.distance, targetRadius))
				{
					radiusTargetReached = true;
					tiiesController.distance = targetRadius;
				}

				if (alphaWaitTimeCounter >= alphaWaitTime) 
				{
					//Alpha
					var actualAlphaFactor = i == 0? alphaFactor : alphaFactor * -1;
					var targetAlpha = radiusAlphaVisualizationList[i + 1].Second;
					tiiesController.alpha += actualAlphaFactor * alphaSpeed;

					if (TargetReached(actualAlphaFactor, tiiesController.alpha, targetAlpha))
					{
						alphaTargerReached = true;
						tiiesController.alpha = targetAlpha;
					}
				}

				alphaWaitTimeCounter += Time.fixedDeltaTime;
				tiiesController.ChangeVisualizationConfiguration (transform.position, tiiesController.distance, tiiesController.alpha);
				radiusAlphaTargetReached[i] = Tuple.New(radiusTargetReached, alphaTargerReached);
			}

			if (radiusAlphaTargetReached.All(t => t.First && t.Second))
				break;

			yield return new WaitForFixedUpdate();
		}

		actualVisualization--;
		tiiesControllers [actualVisualization].notInZero = true;



		if (finalAction != null)
			finalAction();

		movingPlane = false;
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
