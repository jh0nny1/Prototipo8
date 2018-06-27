using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationCanvasController : MonoBehaviour
{
    public Dropdown visualizationDropdown;
    public Text currentSelectedVisualizationText;
    public ScrolldownContent scrollDown, popUpScrollDown;
    public PopUpController popUp;
    //whenever you add a new visualization, just add one slot in the array of the inspector
    public GameObject[] visualizationPlanesArray;
    int lastVisualizationUsed = 0;
    [HideInInspector]
    public string availableActionsTitle, availableActionsList;

    private void OnEnable()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        UpdateCurrentSelectedVisualizationText();
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
		Debug.Log ("la visualizacion actual es:" + currentVisualization);
        //Use the case equal to the visualization key and the last visualization used variable as the index in the visualizationPlanesArray where said visualization panel is referenced.
        switch (currentVisualization)
        {
		case "Plane":
			Debug.Log ("Entre al plane del switch");
                visualizationDropdown.value = 0;
                break;
		case "Sphere":
			Debug.Log ("Entre al sphere del switch");
                visualizationDropdown.value = 1;
                break;
		case "TIIES":
			Debug.Log ("Entre al TIIES del switch");
				visualizationDropdown.value = 2;
				break;
            default:
                visualizationDropdown.value = 0;
                break;
        }
        visualizationDropdown.RefreshShownValue();
        UpdateCurrentVisualization();
    }

    public void UpdateCurrentSelectedVisualizationText()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        currentSelectedVisualizationText.text = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        
    }

    public void UpdateCurrentVisualization()
    {
		Debug.Log ("Entre al updateCurrenteVisualization");
        visualizationPlanesArray[lastVisualizationUsed].SetActive(false);
        visualizationPlanesArray[visualizationDropdown.value].SetActive(true);
        lastVisualizationUsed = visualizationDropdown.value;
    }

    public void ViewAvailableActions()
    {
        popUpScrollDown.LaunchScrollDown(availableActionsTitle, availableActionsList);
    }

}
