using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMapingController : MonoBehaviour {
    public Dropdown currentInterfaceDropdown;
    public ScrolldownContent scrollDown;
    public PopUpController popUp;
    //whenever you add a new interface, just add one slot in the array of the inspector
    public GameObject[] interfacesConfigurationArray;
    int lastInterfaceUsed = 0;
    string[] interfacesName = new string[]
    {
        "EMOTIV: Insight",
        "Microsoft Kinect",
        "NeuroSky Mindwave",
        "Keyboard",
        "BITalino",
		"TouchScreen",
		"LeapMotion"
    };
    /*
     * 0 = Emotiv
     * 1 = Kinect
     * 2 = NeuroSky
     * 4 = Keyboard
     * 5 = BITalino
     * 6 = TouchScreen
     * 7 = LeapMotion
     */

    private void OnEnable()
    {
        scrollDown.LaunchScrollDown("clean", "");
        ActionManager.Instance.LoadMappingActionsNames();
        AddArrayToDropdown(currentInterfaceDropdown, interfacesName);
        UpdateCurrentObject();
    }

    public void UpdateCurrentObject()
    {
		
		Debug.Log ("El valor de currentInterfaceDropdown es:" + currentInterfaceDropdown.value);
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        if (!GLPlayerPrefs.GetBool(Scope, "use" + MOTIONSManager.Instance.interfacesWithInputNames[currentInterfaceDropdown.value]))
        {
            popUp.LaunchPopUpMessage("Interface not active", "Caution: The selected interface is not selected as being active for the evaluation");
        }
        interfacesConfigurationArray[lastInterfaceUsed].SetActive(false);
        interfacesConfigurationArray[currentInterfaceDropdown.value].SetActive(true);
        lastInterfaceUsed = currentInterfaceDropdown.value;
    }

    //The values in the interface dropdown are asigned by code, not in the UI.
    void AddArrayToDropdown(Dropdown availableInputDropdown, string[] actionsNames)
    {
        availableInputDropdown.ClearOptions();
        foreach (string s in actionsNames)
        {
            availableInputDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        availableInputDropdown.RefreshShownValue();
    }
}
