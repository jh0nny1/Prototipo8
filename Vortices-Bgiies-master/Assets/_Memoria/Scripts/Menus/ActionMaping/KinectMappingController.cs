using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinectMappingController : MonoBehaviour {

    public ActionMapingController actionMapController;
    public Slider triggerLevelValue, untriggerLevelValue;
    public Text triggerLevelText, untriggerLevelText;
    public Dropdown dbGestureActionsDropdown, basicGestureActionsDropdown, dbGestureDropdown, basicGestureDropdown;
    string interfaceName = "Kinect";
    string Scope;
    string currentVisualization;
    string currentObject;
    string[] dbGesturesName = new string[]
    {
        "Gesture 1",
        "Gesture 2",
        "Gesture 3",
        "Gesture 4",
        "Gesture 5",
        "Gesture 6",
        "Gesture 7",
		"Gesture 8"
    };

    string[] basicGesturesName = new string[]
    {
        "Open Right Hand",
        "Open Left Hand",
        "Close Right Hand",
        "Close Left Hand",
        "Lasso Right Hand",
        "Lasso Left Hand"
    };

    string[] inputNamesForSummary = new string[]
    {
        "Gesture 1",
        "Gesture 2",
        "Gesture 3",
        "Gesture 4",
        "Gesture 5",
        "Gesture 6",
        "Gesture 7",
		"Gesture 8",
        "Open Right Hand",
        "Open Left Hand",
        "Close Right Hand",
        "Close Left Hand",
        "Lasso Right Hand",
        "Lasso Left Hand"
    };

    private void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        Scope = ProfileManager.Instance.currentEvaluationScope;
        AddArrayToDropdown(dbGestureDropdown, dbGesturesName);
        AddArrayToDropdown(basicGestureDropdown, basicGesturesName);
        ActionManager.Instance.ReloadMappingActionsDropdown(dbGestureActionsDropdown);
        ActionManager.Instance.ReloadMappingActionsDropdown(basicGestureActionsDropdown);
        UpdateMappedActions(inputNamesForSummary);
        SetDbGesturesConfigMenuValues();
        SetBasicGesturesConfigMenuValues();
    }

    public void SetDbGesturesConfigMenuValues()
    {
        int gesture = dbGestureDropdown.value;
        dbGestureActionsDropdown.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, dbGesturesName[gesture]);

        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, interfaceName + dbGesturesName[gesture] + "TriggerLevel") * 10, triggerLevelValue, triggerLevelText);
        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, interfaceName + dbGesturesName[gesture] + "UntriggerLevel") * 10, untriggerLevelValue, untriggerLevelText);

        dbGestureActionsDropdown.RefreshShownValue();
    }

    public void SetBasicGesturesConfigMenuValues()
    {
        int gesture = basicGestureDropdown.value;
        basicGestureActionsDropdown.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, basicGesturesName[gesture]);

        basicGestureActionsDropdown.RefreshShownValue();
    }

    public void UpdateDbGesturesActionDropdownValues()
    {
        int gesture = dbGestureDropdown.value;
        int action = dbGestureActionsDropdown.value;

        ActionManager.Instance.SetMappedActionIndex(interfaceName, dbGesturesName[gesture], action);

        UpdateMappedActions(inputNamesForSummary);
    }

    public void UpdateBasicGesturesActionDropdownValues()
    {
        int gesture = basicGestureDropdown.value;
        int action = basicGestureActionsDropdown.value;

        ActionManager.Instance.SetMappedActionIndex(interfaceName, basicGesturesName[gesture], action);

        UpdateMappedActions(inputNamesForSummary);
    }

    public void UpdateTriggerValues()
    {
        int gesture = dbGestureDropdown.value;
        float trigger = (triggerLevelValue.value / 10);
        int aux = (int)triggerLevelValue.value;
        GLPlayerPrefs.SetFloat(Scope, interfaceName + dbGesturesName[gesture] + "TriggerLevel", trigger);
        triggerLevelText.text = aux.ToString();
    }

    public void UpdateUntriggerValues()
    {
        int gesture = dbGestureDropdown.value;
        float trigger = (untriggerLevelValue.value / 10);
        int aux = (int)untriggerLevelValue.value;
        GLPlayerPrefs.SetFloat(Scope, interfaceName + dbGesturesName[gesture] + "UntriggerLevel", trigger);
        untriggerLevelText.text = aux.ToString();
    }

    #region update values in UI methods

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

    void UpdateTriggerValues(ref int trigger, Slider slider, Text text)
    {
        trigger = (int)slider.value;
        text.text = trigger.ToString();
    }

    void UpdateTriggerValues(int trigger, Slider slider, Text text)
    {
        trigger = (int)slider.value;
        text.text = trigger.ToString();
    }

    void UpdateTriggerValues(ref float trigger, Slider slider, Text text)
    {
        trigger = (slider.value / 10);
        int aux = (int)slider.value;
        text.text = aux.ToString();
    }

    void SetTriggerValues(ref int trigger, Slider slider, Text text)
    {
        slider.value = trigger;
        text.text = trigger.ToString();
    }

    void SetTriggerValues(int trigger, Slider slider, Text text)
    {
        slider.value = trigger;
        text.text = trigger.ToString();
    }

    void SetTriggerValues(float trigger, Slider slider, Text text)
    {
        Debug.Log("trigger: " + trigger.ToString());
        slider.value = trigger;
        int aux = (int)trigger;
        text.text = aux.ToString();
    }

    #endregion
}
