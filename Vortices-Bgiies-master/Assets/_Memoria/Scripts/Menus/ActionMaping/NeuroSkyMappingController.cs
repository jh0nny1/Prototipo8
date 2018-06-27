using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuroSkyMappingController : MonoBehaviour {

    public ActionMapingController actionMapController;
    public Slider triggerLevelValue;
    public Text triggerLevelText;
    public Dropdown mentalLevelActionsDropdow, mentalLevelsDropdown;
    string interfaceName = "NeuroSky";
    string Scope;
    string currentVisualization;
    string currentObject;
    string[] mentalLevelName = new string[]
    {
        "Attention",
        "Blink",
        "Meditation"
    };

    private void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        Scope = ProfileManager.Instance.currentEvaluationScope;
        AddArrayToDropdown(mentalLevelsDropdown, mentalLevelName);
        ActionManager.Instance.ReloadMappingActionsDropdown(mentalLevelActionsDropdow);
        UpdateMappedActions(mentalLevelName);
        SetMentalLevelsConfigMenuValues();
    }

    public void SetMentalLevelsConfigMenuValues()
    {
        int level = mentalLevelsDropdown.value;
        mentalLevelActionsDropdow.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, mentalLevelName[level]);

        SetTriggerValues(GLPlayerPrefs.GetInt(Scope, interfaceName + mentalLevelName[level] + "TriggerLevel"), triggerLevelValue, triggerLevelText);

        mentalLevelActionsDropdow.RefreshShownValue();
    }

    public void UpdateMentalLevelActionDropdownValues()
    {
        int level = mentalLevelsDropdown.value;
        int action = mentalLevelActionsDropdow.value;
        ActionManager.Instance.SetMappedActionIndex(interfaceName, mentalLevelName[level], action);
        UpdateMappedActions(mentalLevelName);
    }

    public void UpdateMentalLevelTriggerValues()
    {
        int level = mentalLevelsDropdown.value;
        int aux = (int)triggerLevelValue.value;
        GLPlayerPrefs.SetInt(Scope, interfaceName + mentalLevelName[level] + "TriggerLevel", aux);
        triggerLevelText.text = aux.ToString();
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
