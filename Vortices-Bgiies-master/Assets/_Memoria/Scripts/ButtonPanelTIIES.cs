using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Memoria{

	public class ButtonPanelTIIES : MonoBehaviour {

		//plane visualization combination
		[HideInInspector]
		public Action initialPlaneAction;
		[HideInInspector]
		public Action finalPlaneAction;

		public Button bt1;
		public Button bt2;
		public Button bt3;
		public Button bt4;

		public Text timer;
		int seconds;

		public GameObject panel1;
		public GameObject panel2;
		public GameObject panel3;
		public GameObject panel4;

		public Color colorCat1;
		public Color colorCat2;
		public Color colorCat3;
		public Color colorCat4;

		public bool cat1;
		public bool cat2;
		public bool cat3;
		public bool cat4;

		public int auxCat1;
		public int auxCat2;
		public int auxCat3;
		public int auxCat4;

		public int panelCat1;
		public int panelCat2;
		public int panelCat3;
		public int panelCat4;

		public int rangeDioCat1;
		public int rangeDioCat2;
		public int rangeDioCat3;
		public int rangeDioCat4;

		public void Awake(){
			cat1 = true;
			cat2 = cat3 = cat4 = false;
			auxCat1 = auxCat2 = auxCat3 = auxCat4 = 1;
			panelCat1 = panelCat2 = panelCat3 = panelCat4 = 1;
			rangeDioCat1 = rangeDioCat2 = rangeDioCat3 = rangeDioCat4 = 0;

		}

		void OnEnable(){
			bt1.onClick.AddListener (selectButton1);
			bt2.onClick.AddListener (SelectButton2);
			bt3.onClick.AddListener (SelectButton3);
			bt4.onClick.AddListener (SelectButton4);
			InitExperiment ();
		}

		public void InitExperiment()
		{
			string Scope = ProfileManager.Instance.currentEvaluationScope;
			seconds = GLPlayerPrefs.GetInt (Scope, "EvaluationTime");
			seconds = seconds * 60;
			StartCoroutine(CalculaTiempo(seconds));
		}

		void Update(){
			TimeSpan span = TimeSpan.FromSeconds (seconds);
			timer.text = span.ToString ();
		}

		IEnumerator CalculaTiempo(int value)
		{
			seconds = value;
			while (seconds > 0) {
				seconds--;
				yield return new WaitForSeconds (1f);

			}

			EndExperiment ();
		}

		public void Inside()
		{
			
			VisualizationManager.Instance.tiiesVisualization.MovePlaneInside (1, initialPlaneAction, finalPlaneAction);

			if (cat1 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat1, true, false, false, false, rangeDioCat1);
			}
			if (cat2 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat2, false, true, false, false, rangeDioCat2);
			}
			if (cat3 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat3, false, false, true, false, rangeDioCat3);
			}
			if (cat4 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat4, false, false, false, true, rangeDioCat4);
			}

		}

		public void Outside()
		{
			
			VisualizationManager.Instance.tiiesVisualization.MovePlaneOutside (1, initialPlaneAction, finalPlaneAction);

			if (cat1 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat1, true, false, false, false, rangeDioCat1);
			}
			if (cat2 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat2, false, true, false, false, rangeDioCat2);
			}
			if (cat3 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat3, false, false, true, false, rangeDioCat3);
			}
			if (cat4 == true) {
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat4, false, false, false, true, rangeDioCat4);
			}

		}

		public void selectButton1(){
			MOTIONSManager.Instance.AddLines ("Selecciona Categoria", bt1.tag.ToString ());
			panel1.SetActive (true);
			panel2.SetActive (false);
			panel3.SetActive (false);
			panel4.SetActive (false);
			cat1 = true;
			cat2 = cat3 = cat4 = false;
			InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat1, true, false, false, false, rangeDioCat1);
		}

		public void SelectButton2(){
			MOTIONSManager.Instance.AddLines ("Selecciona Categoria", bt2.tag.ToString ());
			panel1.SetActive (false);
			panel2.SetActive (true);
			panel3.SetActive (false);
			panel4.SetActive (false);
			cat2 = true;
			cat1 = cat3 = cat4 = false;
			InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat2, false, true, false, false, rangeDioCat2);
		}

		public void SelectButton3(){
			MOTIONSManager.Instance.AddLines ("Selecciona Categoria", bt3.tag.ToString ());
			panel1.SetActive (false);
			panel2.SetActive (false);
			panel3.SetActive (true);
			panel4.SetActive (false);
			cat3 = true;
			cat1 = cat2 = cat4 = false;
			InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat3, false, false, true, false, rangeDioCat3);
		}

		public void SelectButton4(){
			MOTIONSManager.Instance.AddLines ("Selecciona Categoria", bt4.tag.ToString ());
			panel1.SetActive (false);
			panel2.SetActive (false);
			panel3.SetActive (false);
			panel4.SetActive (true);
			cat4 = true;
			cat1 = cat2 = cat3 = false;
			InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat4, false, false, false, true, rangeDioCat4);
		}

		public void InsideCategory1(){
		
			if (auxCat1 == panelCat1) {
				return;
			}

			if(auxCat1 < panelCat1){
				MOTIONSManager.Instance.AddLines ("Move inside Categoria", bt1.tag.ToString ());
				rangeDioCat1 = rangeDioCat1 + 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat1, true, false, false, false, rangeDioCat1);
				auxCat1++;
			}

		}
		public void OutsideCategory1(){
		
			if ((auxCat1-1) == 0) {
				return;
			}
			if ((auxCat1 - 1) < panelCat1 && (auxCat1 - 1) != 0) {
				MOTIONSManager.Instance.AddLines ("Move outside Categoria", bt1.tag.ToString ());
				rangeDioCat1 = rangeDioCat1 - 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat1, true, false, false, false, rangeDioCat1);
				auxCat1--;
			}
		}
		public void InsideCategory2(){
		
			if (auxCat2 == panelCat2) {
				return;
			}
			if (auxCat2 < panelCat2) {
				MOTIONSManager.Instance.AddLines ("Move inside Categoria", bt2.tag.ToString ());
				rangeDioCat2 = rangeDioCat2 + 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat2, false, true, false, false, rangeDioCat2);
				auxCat2++;
			}
		}
		public void OutsideCategory2(){
		
			if ((auxCat2-1) == 0) {
				return;
			}
			if ((auxCat2 - 1) < panelCat2 && (auxCat2 - 1) != 0) {
				MOTIONSManager.Instance.AddLines ("Move outside Categoria", bt2.tag.ToString ());
				rangeDioCat2 = rangeDioCat2 - 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat2, false, true, false, false, rangeDioCat2);
				auxCat2--;
			}
		}
		public void InsideCategory3(){
		
			if (auxCat3 == panelCat3) {
				return;
			}
			if (auxCat3 < panelCat3) {
				MOTIONSManager.Instance.AddLines ("Move inside Categoria", bt3.tag.ToString ());
				rangeDioCat3 = rangeDioCat3 + 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat3, false, false, true, false, rangeDioCat3);
				auxCat3++;
			}
		}
		public void OutsideCategory3(){
		
			if ((auxCat3-1) == 0) {
				return;
			}
			if ((auxCat3 - 1) < panelCat3 && (auxCat3 - 1) != 0) {
				MOTIONSManager.Instance.AddLines ("Move outside Categoria", bt3.tag.ToString ());
				rangeDioCat3 = rangeDioCat3 - 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat3, false, false, true, false, rangeDioCat3);
				auxCat3--;
			}
		}
		public void InsideCategory4(){
		
			if (auxCat4 == panelCat4) {
				return;
			}
			if (auxCat4 < panelCat4) {
				MOTIONSManager.Instance.AddLines ("Move inside Categoria", bt4.tag.ToString ());
				rangeDioCat4 = rangeDioCat4 + 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat4, false, false, false, true, rangeDioCat4);
				auxCat4++;
			}
		}
		public void OutsideCategory4(){
		
			if ((auxCat4-1) == 0) {
				return;
			}
			if ((auxCat4 - 1) < panelCat4 && (auxCat4 - 1) != 0) {
				MOTIONSManager.Instance.AddLines ("Move outside Categoria", bt4.tag.ToString ());
				rangeDioCat4 = rangeDioCat4 - 12;
				InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.DIOClassifier (InformationObjectManager.Instance.planeImages.lookPointerInstanceTIIES.listaCat4, false, false, false, true, rangeDioCat4);
				auxCat4--;
			}
		}

		public void EndExperiment(){

			SceneManager.LoadScene ("ConfigPrototype");
		}

	}

}


