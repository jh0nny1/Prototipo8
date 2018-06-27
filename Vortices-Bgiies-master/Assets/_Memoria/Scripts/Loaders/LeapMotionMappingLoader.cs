using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapMotionMappingLoader : MonoBehaviour {

	string interfaceName = "LeapMotion";

	string[] basicLeapGestureName = new string[]
	{
		"Love and Peace",
		"Thumb to the left",
		"Thumb down",
		"Open Hand",
		"Closed Hand"
	};

	int[] actionIndexLeap;



	private void OnEnable()
	{

		LoadActions();

	}

	public void LoadActions()
	{
		actionIndexLeap = new int[basicLeapGestureName.Length];

		for (int i = 0; i < basicLeapGestureName.Length; i++) {
			actionIndexLeap [i] = ActionManager.Instance.GetMappedActionIndex (interfaceName, basicLeapGestureName[i]);

		}

		for (int i = 0; i < basicLeapGestureName.Length; i++) {
		
			AddAction (i);
		}



	}

		void AddAction(int index){

		if (actionIndexLeap [index] == 0) {
			return;
		}


		ActionManager.Instance.updateActionArrayList.Add (() => ActionManager.Instance.ActionPairing (
					ActionManager.Instance.ActionLeap(index), //condicion bool
					ActionManager.Instance.currentActionList [actionIndexLeap [index]]) //accion que se ejecuta
				);

		PrintAddedAction(basicLeapGestureName[index], ActionManager.Instance.currentActionListNames[actionIndexLeap[index]]);
	}



	void PrintAddedAction(string inputName, string pairedActionName)
	{
		Debug.Log("Paired: " +inputName + " to "+pairedActionName);
	}
}
