using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class LeapMotionController : MonoBehaviour {

	public ActionMapingController actionMapController;
	public Dropdown LeapActionDropdown, LeapDropdown;
	string interfaceName = "LeapMotion";
	string Scope;
	string currentVisualization;
	string currentObject;
	string[] basicLeapGestureName = new string[]
	{
		"Love and Peace",
		"Thumb to the left",
		"Thumb down",
		"Open Hand",
		"Closed Hand"
	};


	private void OnEnable(){

		currentVisualization = GLPlayerPrefs.GetString (Scope, "CurrentVisualization");
		currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
		Scope = ProfileManager.Instance.currentEvaluationScope;
		AddArrayToDropdown (LeapDropdown, basicLeapGestureName);
		ActionManager.Instance.ReloadMappingActionsDropdown (LeapActionDropdown);
		UpdateMappedActions (basicLeapGestureName);
		SetLeapMotionConfigMenuValues ();

	}

	public void SetLeapMotionConfigMenuValues()
	{
		int level = LeapDropdown.value;
		LeapActionDropdown.value = ActionManager.Instance.GetMappedActionIndex (interfaceName, basicLeapGestureName [level]);
		LeapActionDropdown.RefreshShownValue ();


	}

	public void UpdateLeapMotionActionDropdownValues()
	{
		Debug.Log ("Entre al updateTouchActionDropdonwvalues");
		int level = LeapDropdown.value;
		int action = LeapActionDropdown.value;
		ActionManager.Instance.SetMappedActionIndex (interfaceName, basicLeapGestureName [level], action);
		UpdateMappedActions (basicLeapGestureName);
	}

	void AddArrayToDropdown(Dropdown availableInputDropdown, string[] actionsNames)
	{
		availableInputDropdown.ClearOptions();
		foreach (string s in actionsNames)
		{
			availableInputDropdown.options.Add(new Dropdown.OptionData() { text = s });
		}
		availableInputDropdown.RefreshShownValue();
	}

	void UpdateMappedActions(string[] inputNames)
	{
		string aux = "";
		foreach (string s in ActionManager.Instance.GetMappedActionsListNames(interfaceName, inputNames))
		{
			aux = aux + s + "\n";
			//Debug.Log ("los valores del aux del UpdateMappedActtion son:" + aux);
		}
		actionMapController.scrollDown.LaunchScrollDown("Actions Paired", aux);
	}

}
