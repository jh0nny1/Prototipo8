using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;


public class TouchScreenController : MonoBehaviour {

	public ActionMapingController actionMapController;
	public Dropdown TouchActionDropdown, TouchDropdown;
	string interfaceName = "TouchScreen";
	string Scope;
	string currentVisualization;
	string currentObject;
	string[] basicTouchGestureName = new string[]
	{
		"Swipe to the upside",
		"Swipe to the bottom",
		"Swipe to the right",
		"Swipe to the left",
		"Zoom in",
		"Zoom out"

	};

	private void OnEnable(){
	
		currentVisualization = GLPlayerPrefs.GetString (Scope, "CurrentVisualization");
		currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
		Scope = ProfileManager.Instance.currentEvaluationScope;
		AddArrayToDropdown (TouchDropdown, basicTouchGestureName);
		ActionManager.Instance.ReloadMappingActionsDropdown (TouchActionDropdown);
		UpdateMappedActions (basicTouchGestureName);
		SetTouchScreenConfigMenuValues ();
	
	}

	public void SetTouchScreenConfigMenuValues()
	{
		int level = TouchDropdown.value;
		TouchActionDropdown.value = ActionManager.Instance.GetMappedActionIndex (interfaceName, basicTouchGestureName [level]);
		TouchActionDropdown.RefreshShownValue ();

	}

	public void UpdateTouchScreenActionDropdownValues()
	{
		int level = TouchDropdown.value;
		int action = TouchActionDropdown.value;
		ActionManager.Instance.SetMappedActionIndex (interfaceName, basicTouchGestureName [level], action);
		UpdateMappedActions (basicTouchGestureName);
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

		}
		actionMapController.scrollDown.LaunchScrollDown("Actions Paired", aux);
	}


}
