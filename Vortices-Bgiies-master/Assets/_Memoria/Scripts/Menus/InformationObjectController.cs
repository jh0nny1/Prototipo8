using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationObjectController : MonoBehaviour {
    public Dropdown currentObjectDropdown;
    public Text currentSelectedObjectText;
    public ScrolldownContent scrollDown, popUpScrollDown;
    public PopUpController popUp;
    //whenever you add a new object, just add one slot in the array of the inspector
    public GameObject[] objectPlanesArray;
    int lastObjectUsed = 0;

    private void OnEnable()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        UpdateCurrentSelectedObjectText();
        string currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        //Use the case equal to the information object key and the last object used variable as the index in the objectPlanesArray where said information object panel is referenced.
        switch (currentObject)
        {
            case "PlaneImage":
                currentObjectDropdown.value = 0;
                break;
            default:
                currentObjectDropdown.value = 0;
                break;
        }
        currentObjectDropdown.RefreshShownValue();
        UpdateCurrentObject();
    }

    public void UpdateCurrentSelectedObjectText()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        currentSelectedObjectText.text = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
    }

    public void UpdateCurrentObject()
    {
        objectPlanesArray[lastObjectUsed].SetActive(false);
        objectPlanesArray[currentObjectDropdown.value].SetActive(true);
        lastObjectUsed = currentObjectDropdown.value;
    }
    
}
