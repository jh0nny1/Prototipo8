using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using Memoria.Core;
using UnityCallbacks;
using System;
using UnityEngine.UI;


namespace Memoria

{
	
	public class LookPointerTIIES : LookPointer, IAwake, IUpdate {

		public List<PitchGrabObject> listaCat1 = null;
		public List<PitchGrabObject> listaCat2 = null;
		public List<PitchGrabObject> listaCat3 = null;
		public List<PitchGrabObject> listaCat4 = null;
		public bool Category1;
		public bool Category2;
		public bool Category3;
		public bool Category4;



		float closeRange = 18f;

		public void Awake()
		{
			
			actualPitchGrabObject = null;
			posibleActualPitchGrabObject = null;

			Category1 = true;
			Category2 = Category3 = Category4 = false;
			zoomingOut = false;
			zoomingIn = false;

			listaCat1 = new List<PitchGrabObject>();
			listaCat2 = new List<PitchGrabObject>();
			listaCat3 = new List<PitchGrabObject>();
			listaCat4 = new List<PitchGrabObject>();



		}

		public void Update () {

		}

		public override void LookPointerStay(PitchGrabObject pitchGrabObject)
		{
			posibleActualPitchGrabObject = pitchGrabObject;
		}

		public override void SetZoomInInitialStatus(PitchGrabObject pitchGrabObject)
		{
			actualPitchGrabObject = pitchGrabObject;
			actualPitchGrabObject.dioController.inVisualizationPosition = false;
			_actualPitchObjectOriginalPosition = pitchGrabObject.transform.position;
			_actualPitchObjectOriginalRotation = pitchGrabObject.transform.rotation;
			_actualPitchObjectOriginalScale = pitchGrabObject.transform.localScale;
		}

		public override void DirectZoomInCall(Action finalAction)
		{
			if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !VisualizationManager.Instance.tiiesVisualization.movingPlane) {
				if (posibleActualPitchGrabObject.isClone == false) {
					StartCoroutine(ZoomingIn(posibleActualPitchGrabObject, finalAction));
				}

			}

			
		}

		public override void DirectZoomInCall(PitchGrabObject pitchGrabObject, Action finalAction)
		{
			if (!zoomingIn && !zoomingOut && actualPitchGrabObject == null && !VisualizationManager.Instance.tiiesVisualization.movingPlane)
			{
				StartCoroutine(ZoomingIn(pitchGrabObject, finalAction));
			}
		}

		public override IEnumerator ZoomingIn(PitchGrabObject pitchGrabObject, Action finalAction)
		{
			zoomingIn = true;
			SetZoomInInitialStatus(pitchGrabObject);
			MOTIONSManager.Instance.AddLines("Zooming In", pitchGrabObject.idName);

			var counter = 0;
			while (true)
			{
				pitchGrabObject.transform.position = new Vector3(0.0005f, 3.6575f, 0.0179f);

				if (counter >= closeRange)
				{
					break;
				}

				counter++;
				yield return new WaitForFixedUpdate();
			}

			if (finalAction != null)
				finalAction();
			zoomingIn = false;

		}

		public override void DirectZoomOutCall(Action finalAction)
		{
			if (!zoomingOut && !zoomingIn && actualPitchGrabObject != null && !VisualizationManager.Instance.tiiesVisualization.movingPlane)
			{
				StartCoroutine(ZoomingOut(finalAction));
			}
		}

		public override IEnumerator ZoomingOut(Action finalAction)
		{
			MOTIONSManager.Instance.AddLines("Zooming Out", actualPitchGrabObject.idName);
			zoomingOut = true;

			var positionTargetReached = false;
			var scaleTargetReaced = false;

			while (true)
			{
				//Position
				actualPitchGrabObject.transform.position =
					Vector3.MoveTowards(actualPitchGrabObject.transform.position,
						_actualPitchObjectOriginalPosition, _positionSteps);

				if (actualPitchGrabObject.transform.position.EqualOrMayorCompareVector(_actualPitchObjectOriginalPosition, -0.0001f) && !positionTargetReached)
				{
					positionTargetReached = true;
					actualPitchGrabObject.transform.position = _actualPitchObjectOriginalPosition;
				}

				//Scale
				actualPitchGrabObject.transform.localScale =
					Vector3.MoveTowards(actualPitchGrabObject.transform.localScale,
						_actualPitchObjectOriginalScale, _scaleSteps);

				if (actualPitchGrabObject.transform.localScale.EqualOrMinorCompareVector(_actualPitchObjectOriginalScale, 0.001f) && !scaleTargetReaced)
				{
					scaleTargetReaced = true;
					actualPitchGrabObject.transform.localScale = _actualPitchObjectOriginalScale;
				}

				if (positionTargetReached && scaleTargetReaced)
					break;

				yield return new WaitForFixedUpdate();

			}

			actualPitchGrabObject.OnUnDetect();
			actualPitchGrabObject.dioController.inVisualizationPosition = true;
			actualPitchGrabObject = null;
			zoomingOut = false;
			if (finalAction != null)
				finalAction();


		}

		public void SelectCat1(){

			bool unPitchedAccept = false;

			if (actualPitchGrabObject == null) {
				
				if (posibleActualPitchGrabObject == null) {
					return;
				}

				unPitchedAccept = true;
				actualPitchGrabObject = posibleActualPitchGrabObject;
			}


			actualPitchGrabObject.isSelectedCat1 = !actualPitchGrabObject.isSelectedCat1;

			if (actualPitchGrabObject.isSelectedCat1) {

				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.isSelectedCat1 = !posibleActualPitchGrabObject.isSelectedCat1;
				} else {
					CreateClone (actualPitchGrabObject, listaCat1);
					createMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat1, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt1);
				}
			
			} else {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.transform.parent.GetChild (0).GetComponent<PitchGrabObject> ().isSelectedCat1 = false;

				}
				DestroyClone (actualPitchGrabObject, listaCat1);
				DeleteMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat1, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt1);

			}

			var action = actualPitchGrabObject.isSelectedCat1 ? "Select" : "Deselect";
			action = action + VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt1.tag.ToString ();
			MOTIONSManager.Instance.AddLines (action, actualPitchGrabObject.idName);


			if (unPitchedAccept)
				actualPitchGrabObject = null;


			VisualizationManager.Instance.tiiesVisualization.buttonTiies.panelCat1 = ((listaCat1.Count - 1) / 12) + 1; 
			DIOClassifier (listaCat1, true, false, false, false, VisualizationManager.Instance.tiiesVisualization.buttonTiies.rangeDioCat1);

		}

		public void SelectCat2(){
		
			bool unPitchedAccept = false;

			if (actualPitchGrabObject == null) {

				if (posibleActualPitchGrabObject == null) {
					return;
				}

				unPitchedAccept = true;
				actualPitchGrabObject = posibleActualPitchGrabObject;
			}

			actualPitchGrabObject.isSelectedCat2 = !actualPitchGrabObject.isSelectedCat2;

			if (actualPitchGrabObject.isSelectedCat2) {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.isSelectedCat2 = !actualPitchGrabObject.isSelectedCat2;
				} else {
					CreateClone (actualPitchGrabObject, listaCat2);
					createMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat2, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt2);
				}

			} else {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.transform.parent.GetChild (0).GetComponent<PitchGrabObject> ().isSelectedCat2 = false;

				}
				DestroyClone (actualPitchGrabObject, listaCat2);
				DeleteMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat2, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt2);
			}

			var action = actualPitchGrabObject.isSelectedCat2 ? "Select" : "Deselect";
			action = action + VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt2.tag.ToString ();
			MOTIONSManager.Instance.AddLines (action, actualPitchGrabObject.idName);

			if (unPitchedAccept)
				actualPitchGrabObject = null;

			VisualizationManager.Instance.tiiesVisualization.buttonTiies.panelCat2 = ((listaCat2.Count - 1) / 12) + 1;
			DIOClassifier (listaCat2, false, true, false, false, VisualizationManager.Instance.tiiesVisualization.buttonTiies.rangeDioCat2);

		}

		public void SelectCat3(){

			bool unPitchedAccept = false;

			if (actualPitchGrabObject == null) {

				if (posibleActualPitchGrabObject == null) {
					return;
				}

				unPitchedAccept = true;
				actualPitchGrabObject = posibleActualPitchGrabObject;
			}

			actualPitchGrabObject.isSelectedCat3 = !actualPitchGrabObject.isSelectedCat3;

			if (actualPitchGrabObject.isSelectedCat3) {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.isSelectedCat3 = !actualPitchGrabObject.isSelectedCat3;
				} else {
					CreateClone (actualPitchGrabObject, listaCat3);
					createMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat3, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt3);
				}

			} else {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.transform.parent.GetChild (0).GetComponent<PitchGrabObject> ().isSelectedCat3 = false;

				}
				DestroyClone (actualPitchGrabObject, listaCat3);
				DeleteMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat3, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt3);
			}

			var action = actualPitchGrabObject.isSelectedCat3 ? "Select" : "Deselect";
			action = action + VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt3.tag.ToString ();
			MOTIONSManager.Instance.AddLines (action, actualPitchGrabObject.idName);

			if (unPitchedAccept)
				actualPitchGrabObject = null;

			VisualizationManager.Instance.tiiesVisualization.buttonTiies.panelCat3 = ((listaCat3.Count - 1) / 12) + 1;
			DIOClassifier (listaCat3, false, false, true, false, VisualizationManager.Instance.tiiesVisualization.buttonTiies.rangeDioCat3);

		}

		public void SelectCat4(){

			bool unPitchedAccept = false;

			if (actualPitchGrabObject == null) {

				if (posibleActualPitchGrabObject == null) {
					return;
				}

				unPitchedAccept = true;
				actualPitchGrabObject = posibleActualPitchGrabObject;
			}

			actualPitchGrabObject.isSelectedCat4 = !actualPitchGrabObject.isSelectedCat4;

			if (actualPitchGrabObject.isSelectedCat4) {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.isSelectedCat4 = !actualPitchGrabObject.isSelectedCat4;
				} else {
					CreateClone (actualPitchGrabObject, listaCat4);
					createMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat4, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt4);
				}

			} else {
				if (actualPitchGrabObject.isClone == true) {
					actualPitchGrabObject.transform.parent.GetChild (0).GetComponent<PitchGrabObject> ().isSelectedCat4 = false;

				}
				DestroyClone (actualPitchGrabObject, listaCat4);
				DeleteMarcador (VisualizationManager.Instance.tiiesVisualization.buttonTiies.colorCat4, VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt4);
			}

			var action = actualPitchGrabObject.isSelectedCat4 ? "Select" : "Deselect";
			action = action + VisualizationManager.Instance.tiiesVisualization.buttonTiies.bt4.tag.ToString ();
			MOTIONSManager.Instance.AddLines (action, actualPitchGrabObject.idName);

			if (unPitchedAccept)
				actualPitchGrabObject = null;

			VisualizationManager.Instance.tiiesVisualization.buttonTiies.panelCat4 = ((listaCat4.Count - 1) / 12) + 1;
			DIOClassifier (listaCat4, false, false, false, true, VisualizationManager.Instance.tiiesVisualization.buttonTiies.rangeDioCat4);

		}

		public void CreateClone(PitchGrabObject image, List<PitchGrabObject> list){

			PitchGrabObject dio = Instantiate(actualPitchGrabObject);

			dio.transform.parent = actualPitchGrabObject.transform.parent;
			dio.transform.DestroyChildrenImmediate();
			dio.transform.localPosition = _actualPitchObjectOriginalPosition;
			dio.transform.localScale = new Vector3(0.03f, 0.03f, 0.001f);

			dio.idName = image.idName;

			dio.gameObject.SetActive (false);

			dio.isClone = true;

			bool contenedor = false;

			foreach (var digitalObject in list) {

				if (digitalObject.idName == image.idName) {
					contenedor = true;
				}
			}

			if (!contenedor) {
				list.Add (dio);

			}

		}

		public void DestroyClone(PitchGrabObject image, List<PitchGrabObject> list){
		
			PitchGrabObject obj = null;
			List<PitchGrabObject> listaAux = list;
			int i = 0;
			foreach (PitchGrabObject photo in listaAux)
			{
				
				if (photo.idName == image.idName)
				{
					obj = photo;                 
					break;
				}
				i++;
			}
			list.RemoveAt(i);
			if (obj != null) {
				Destroy(obj.gameObject);

			}
				
			
		}

		public void DIOClassifier(List<PitchGrabObject> list, bool cat1, bool cat2, bool cat3, bool cat4, int paneles){
		
			if (!VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat1) {
				foreach (var dio in listaCat1) {
					dio.gameObject.SetActive (false);
				}
			}
			if (!VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat2) {
				foreach (var dio in listaCat2) {
					dio.gameObject.SetActive (false);
				}
			}
			if (!VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat3) {
				foreach (var dio in listaCat3) {
					dio.gameObject.SetActive (false);
				}
			}
			if (!VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat4) {
				foreach (var dio in listaCat4) {
					dio.gameObject.SetActive (false);
				}
			}
				
			int index = 0;

			for (int i = 0; i < list.Count; i++) {

				index++;

				if (index == 13) {
					index = 0;
					index++;

				}

				if (index == 1) {
					list [i].transform.position = new Vector3 (0.021f, 3.692f, 0.05f);
				}

				if (index > 1 && index < 5) {
					list [i].transform.position = new Vector3 (list [i - 1].transform.position.x + 0.035f, 3.692f, 0.05f);
				}
				if (index == 5) {
					list [i].transform.position = new Vector3 (0.021f, list [i - 1].transform.position.y - 0.035f, 0.05f);
				}
				if (index > 5 && index < 9) {
					list [i].transform.position = new Vector3 (list [i - 1].transform.position.x + 0.035f, list [i - 1].transform.position.y, 0.05f);
				}
				if (index == 9) {
					list [i].transform.position = new Vector3 (0.021f, list [i - 1].transform.position.y - 0.035f, 0.05f);
				}
				if (index > 9 && index < 13) {
					list [i].transform.position = new Vector3 (list [i - 1].transform.position.x + 0.035f, list [i - 1].transform.position.y, 0.05f);
				}

				if ((cat1 && VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat1) || (cat2 && VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat2) || (cat3 && VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat3) || (cat4 && VisualizationManager.Instance.tiiesVisualization.buttonTiies.cat4)) {

					if (i >= paneles && i < paneles + 12) {
						list [i].gameObject.SetActive (true);
					} else {
						list [i].gameObject.SetActive (false);
					}

				}
														
			}

		}

		public void createMarcador(Color color, Button button){

			GameObject obj = Instantiate (VisualizationManager.Instance.tiiesVisualization.marcador) as GameObject;
			obj.transform.parent = actualPitchGrabObject.transform;
			obj.transform.localScale = new Vector3 (0.25f, 0.25f, 0.1f);
			int i = actualPitchGrabObject.transform.childCount;

			actualPitchGrabObject.transform.GetChild (i-1).transform.position = new Vector3 (actualPitchGrabObject.transform.position.x - 0.0115f + (i-1) * 0.0075f, actualPitchGrabObject.transform.position.y - 0.01100f, actualPitchGrabObject.transform.position.z - 0.00001f);

			obj.transform.GetComponent<Renderer> ().material.color = color;

			string[] textBt = button.GetComponentInChildren<Text> ().text.ToString ().Split (':');
			int contador = Int32.Parse (textBt [1].Trim ()) + 1;
			button.GetComponentInChildren<Text> ().text = textBt [0] + ": " + contador.ToString ();

		}

		public void DeleteMarcador(Color color, Button button){


			if (actualPitchGrabObject.isClone == false) {
				int child = actualPitchGrabObject.transform.childCount;
				int index = 0;
				for (int i = 0; i < child; i++) {
					if (actualPitchGrabObject.transform.GetChild (i).transform.GetComponent<Renderer> ().material.color == color) {
						Destroy (actualPitchGrabObject.transform.GetChild (i).gameObject);
					} else {
						actualPitchGrabObject.transform.GetChild (i).transform.position = new Vector3 (actualPitchGrabObject.transform.position.x - 0.0115f + index * 0.0075f, actualPitchGrabObject.transform.position.y - 0.01100f, actualPitchGrabObject.transform.position.z - 0.00001f);
						index++;
					}
				}
			} else {

				int child = actualPitchGrabObject.transform.parent.GetChild(0).childCount;
				int index = 0;
				for (int i = 0; i < child; i++) {
					if (actualPitchGrabObject.transform.parent.GetChild (0).GetChild (i).transform.GetComponent<Renderer> ().material.color == color) {
						Destroy (actualPitchGrabObject.transform.parent.GetChild (0).GetChild (i).gameObject);
					} else {
						actualPitchGrabObject.transform.parent.GetChild(0).GetChild (i).transform.position = new Vector3 (actualPitchGrabObject.transform.parent.GetChild(0).position.x - 0.0115f + index * 0.0075f, actualPitchGrabObject.transform.parent.GetChild(0).position.y - 0.01100f, actualPitchGrabObject.transform.parent.GetChild(0).position.z - 0.00001f);
						index++;
					}
				}
			}

			string[] textBt = button.GetComponentInChildren<Text> ().text.ToString ().Split (':');
			int contador = Int32.Parse (textBt [1].Trim ()) - 1;
			button.GetComponentInChildren<Text> ().text = textBt [0] + ": " + contador.ToString ();
		}



	}



}
