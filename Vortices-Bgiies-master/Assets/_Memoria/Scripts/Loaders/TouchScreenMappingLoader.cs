using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenMappingLoader : MonoBehaviour {

	string interfaceName = "TouchScreen";

	string[] basicTouchGestureName = new string[]
	{
		"Swipe to the upside",
		"Swipe to the bottom",
		"Swipe to the right",
		"Swipe to the left",
		"Zoom in",
		"Zoom out"

	};



	int[] actionIndexTouch;



	private void OnEnable()
	{
		
		LoadActions();

	}



	//void Update(){
	//	Debug.Log ("Estoy en el update");
	
	//}

	public void LoadActions()
	{
		actionIndexTouch = new int[basicTouchGestureName.Length];

		for (int i = 0; i < basicTouchGestureName.Length; i++) {
			actionIndexTouch [i] = ActionManager.Instance.GetMappedActionIndex (interfaceName, basicTouchGestureName[i]);
			//Debug.Log ("El valor del actionIndexTouch es:" + actionIndexTouch [i]);
		}

		for (int i = 0; i < basicTouchGestureName.Length; i++) {
			AddAction (i);
		}






	}

	//void AddAction(int index, KeyCode key)
	//{
		//Debug.Log("Entre al addAction de TouchScreenMappingLoader");
		//Debug.Log ("El valor del actionIndex es:" + actionIndexTouch [index]);
	//	if (actionIndexTouch [index] == 0) {
			//Debug.Log ("Entre al if AddAction TouchScreenMappingLoader y me retorne");
	//		return;
	//	}
		//Debug.Log ("no entre al if");
	//	ActionManager.Instance.updateActionArrayList.Add( () => ActionManager.Instance.ActionPairing(
	//		ActionManager.Instance.ActionConditionButtons(key), //condicion bool
	//		ActionManager.Instance.currentActionList[actionIndexTouch[index]]) //accion que se ejecuta
	//	);

	//	PrintAddedAction(basicTouchGestureName[index], ActionManager.Instance.currentActionListNames[actionIndexTouch[index]]);
	
	//}

	void AddAction(int index)
	{
		//Debug.Log("Entre al addAction de TouchScreenMappingLoader");
		//Debug.Log ("El valor del actionIndex es:" + actionIndexTouch [index]);
		if (actionIndexTouch [index] == 0) {
			//Debug.Log ("Entre al if AddAction TouchScreenMappingLoader y me retorne");
			return;
		}

		ActionManager.Instance.updateActionArrayList.Add (() => ActionManager.Instance.ActionPairing (
			ActionManager.Instance.ActionTouch(index), //condicion bool
			ActionManager.Instance.currentActionList [actionIndexTouch [index]]) //accion que se ejecuta
		);




		PrintAddedAction(basicTouchGestureName[index], ActionManager.Instance.currentActionListNames[actionIndexTouch[index]]);

	}

	void PrintAddedAction(string inputName, string pairedActionName)
	{
		Debug.Log("Paired: " +inputName + " to "+pairedActionName);
	}


}
