using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereVisualizationController : MonoBehaviour {
    public VisualizationCanvasController visualizationController;
    string visualizationName = "Sphere";
    string[] sphereVisualizationActionsNames = new string[]
    {
        "Change to next plane",
            "Change to previous plane",

    };

    private void OnEnable()
    {
        if (visualizationController != null)
        {
            SelectThisVisualization();
            visualizationController.scrollDown.LaunchScrollDown("Sphere visualization description", "This visualization is designed for immersive virtual reality environments and shows objects distribuited in a spherical configuration around the subject.");
            visualizationController.availableActionsTitle = "Sphere visualization actions";
            visualizationController.availableActionsList = "[·]Advance to next plane: If the amount of objects is too big and can't be show in a single screen, the remaining will be placed in another view, called plane. This action highlights the images in the next Plane." +
                "\n[·]Go back to previous plane: If the advance to the next plane action was taken, this action allows to go back and highlight the objects of the previous Plane";
        }
    }

    public void SelectThisVisualization()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetString(Scope, "CurrentVisualization", visualizationName);
        visualizationController.UpdateCurrentSelectedVisualizationText();
        ActionManager.Instance.UpdateVisualizationActionNames(sphereVisualizationActionsNames);
        //DELETE THIS
        GLPlayerPrefs.SetBool(Scope, "BGIIESMode", false);
		GLPlayerPrefs.SetBool (Scope, "TIIESMode", false);
		bool wea = GLPlayerPrefs.GetBool (Scope, "BGIIESMode");
		bool wea2 = GLPlayerPrefs.GetBool (Scope, "TIIESMode");
		Debug.Log ("el booleano de bgiiesMode es:" + wea);
		Debug.Log ("el booleano de tiiesmode es:" + wea2);
    }
}
