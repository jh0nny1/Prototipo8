using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic;

public class TIIESVisualizationController : MonoBehaviour {

	public VisualizationCanvasController visualizationController;
	string visualizationName = "TIIES";
	string[] TIIESVisualizationActionsNames = new string[]
	{
		"Select/Deselect topic 1",
		"Select/Deselect topic 2",
		"Select/Deselect topic 3",
		"Select/Deselect topic 4",
		"Show/hide topic 1",
		"Show/hide topic 2",
		"Show/hide topic 3",
		"Show/hide topic 4",
		"Change to next plane",
		"Change to previous plane"

	};

	private void OnEnable()
	{
		if (visualizationController != null)
		{
			SelectThisVisualization ();

			visualizationController.scrollDown.LaunchScrollDown ("TIIES visualization description", "this is a test and this is TIIES");
			visualizationController.availableActionsTitle = "TIIES visualizationes actions";
			visualizationController.availableActionsList = "[·]Advance to next plane: bla bla bla";

		}
	
	}

	public void SelectThisVisualization()
	{
		string Scope = ProfileManager.Instance.currentEvaluationScope;
		GLPlayerPrefs.SetString(Scope, "CurrentVisualization", visualizationName);
		visualizationController.UpdateCurrentSelectedVisualizationText();
		ActionManager.Instance.UpdateVisualizationActionNames (TIIESVisualizationActionsNames);

		GLPlayerPrefs.SetBool (Scope, "TIIESMode", true);
		GLPlayerPrefs.SetBool (Scope, "BGIIESMode", false);
		bool wea = GLPlayerPrefs.GetBool (Scope, "BGIIESMode");
		bool wea2 = GLPlayerPrefs.GetBool (Scope, "TIIESMode");
		Debug.Log ("el booleano de bgiiesMode es:" + wea);
		Debug.Log ("el booleano de tiiesmode es:" + wea2);
	}


}
