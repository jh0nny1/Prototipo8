using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class EmotivMappingController : MonoBehaviour {

    //Ticking scripts
    public ActionMapingController actionMapController;
    public Slider tickSensibilityValue, mentalCommandSensibilityValue, mentalCommandMistakeValue;
    public Text tickSensibilityText, mentalCommandMistakeText, mentalCommandSensibilityText;
    //EEG variables
    public Dropdown mentalCommandActionsDropdow, mentalCommandDropdown, facialExpresionActionsDropdown, facialExpresionDropdown;
    public Slider mentalCommandTriggerLevel, facialExpressionTriggerLevel;
    public Text mentalCommandTriggerNumber, facialExpressionTriggerNumber;
    string interfaceName = "Emotiv";
    string Scope;
    string currentVisualization;
    string currentObject;
    string[] mentalCommandName = new string[]
    {
        "Push",
        "Pull",
        "Lift",
        "Drop",
        "Left"
    };

    string[] facialExpresionName = new string[]
    {
        "LeftWink",
        "RightWink",
        "AnyBlink",
        "Smile"
    };

    bool[] facilExpresionIsUpperFace = new bool[]
    {
        true,
        true,
        true,
        false
    };

    string[] inputNamesForSummary = new string[]
    {
        "Push",
        "Pull",
        "Lift",
        "Drop",
        "Left",
        "LeftWink",
        "RightWink",
        "AnyBlink",
        "Smile"
    };

    /*
    EEGManager.Instance.MentalCommandCurrentActionPower;
    EEGManager.Instance.FacialExpressionIsRightEyeWinking;
    EEGManager.Instance.FacialExpressionIsLeftEyeWinking;
    EEGManager.Instance.FacialExpressionIsUserBlinking;
    EEGManager.Instance.FacialExpressionUpperFaceActionPower;
    EEGManager.Instance.FacialExpressionSmileExtent;
    EEGManager.Instance.FacialExpressionLowerFaceActionPower;
    EEGManager.Instance.FacialExpressionLowerFaceAction;
    EEGManager.Instance.FacialExpressionUpperFaceAction;
    */

    

    void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        Scope = ProfileManager.Instance.currentEvaluationScope;

        float aux = GLPlayerPrefs.GetFloat(Scope, "EmotivTickSensibility") *10;
        SetTriggerValues((int)aux, tickSensibilityValue, tickSensibilityText);

        ActionManager.Instance.ReloadMappingActionsDropdown(facialExpresionActionsDropdown);
        ActionManager.Instance.ReloadMappingActionsDropdown(mentalCommandActionsDropdow);

        UpdateMappedActions(inputNamesForSummary);

        SetMentalCommandConfigMenuValues();
        SetFacialExpressionConfigMenuValues();
        
    }

    #region UI triggers

    public void SetTickConfigMenuValues()
    {        
        ActionManager.Instance.endTime = (tickSensibilityValue.value / 10);
        int aux = (int)tickSensibilityValue.value;
        tickSensibilityText.text = aux.ToString();
        GLPlayerPrefs.SetFloat(Scope, "EmotivTickSensibility", ActionManager.Instance.endTime);
    }

    public void SetMentalCommandConfigMenuValues()
    {
        int command = mentalCommandDropdown.value;
        mentalCommandActionsDropdow.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, mentalCommandName[command]); 

        SetTriggerValues(GLPlayerPrefs.GetInt(Scope, interfaceName + mentalCommandName[command] + "CommandTicks"), mentalCommandSensibilityValue, mentalCommandSensibilityText);
        SetTriggerValues(GLPlayerPrefs.GetInt(Scope, interfaceName + mentalCommandName[command] + "CommandMistakes"), mentalCommandMistakeValue, mentalCommandMistakeText);
        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, interfaceName + mentalCommandName[command] + "CommandTriggerLevel") * 10, mentalCommandTriggerLevel, mentalCommandTriggerNumber);

        mentalCommandActionsDropdow.RefreshShownValue();
    }

    public void SetFacialExpressionConfigMenuValues()
    {
        int facial = facialExpresionDropdown.value;
        facialExpresionActionsDropdown.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, facialExpresionName[facial]);

        SetTriggerValues(GLPlayerPrefs.GetFloat(Scope, "Emotiv" + facialExpresionName[facial] + "TriggerLevel" ) * 10, facialExpressionTriggerLevel, facialExpressionTriggerNumber);

        facialExpresionActionsDropdown.RefreshShownValue();
    }

    public void UpdateMentalCommandActionDropdownValues()
    {
        int command = mentalCommandDropdown.value;
        int action = mentalCommandActionsDropdow.value;
        ActionManager.Instance.SetMappedActionIndex(interfaceName, mentalCommandName[command], action);
        
        UpdateMappedActions(inputNamesForSummary);
    }

    public void UpdateFacialExpressionActionDropdownValues()
    {
        int expresion = facialExpresionDropdown.value;
        int action = facialExpresionActionsDropdown.value;
        ActionManager.Instance.SetMappedActionIndex(interfaceName, facialExpresionName[expresion], action);
        
        UpdateMappedActions(inputNamesForSummary);
    }

    public void UpdateMentalCommandSensibilityValues()
    {
        int command = mentalCommandDropdown.value;
        int aux = (int)mentalCommandSensibilityValue.value;
        GLPlayerPrefs.SetInt(Scope, interfaceName + mentalCommandName[command] + "CommandTicks", aux);
        mentalCommandSensibilityText.text = aux.ToString();
    }

    public void UpdateMentalCommandMistakesValues()
    {
        int command = mentalCommandDropdown.value;
        int aux = (int)mentalCommandMistakeValue.value;
        GLPlayerPrefs.SetInt(Scope, interfaceName + mentalCommandName[command] + "CommandMistakes", aux);
        mentalCommandMistakeText.text = aux.ToString();
    }

    public void UpdateMentalCommandTriggerValues()
    {
        int command = mentalCommandDropdown.value;
        float trigger = (mentalCommandTriggerLevel.value / 10);
        int aux = (int)mentalCommandTriggerLevel.value;
        GLPlayerPrefs.SetFloat(Scope, interfaceName + mentalCommandName[command] + "CommandTriggerLevel", trigger);
        mentalCommandTriggerNumber.text = aux.ToString();
    }

    public void UpdateUpperFacialExpressionTriggerValues()
    {
        int expresion = facialExpresionDropdown.value;
        float trigger = (facialExpressionTriggerLevel.value / 10);
        int aux = (int)facialExpressionTriggerLevel.value;
        GLPlayerPrefs.SetFloat(Scope, "Emotiv" + facialExpresionName[expresion] + "TriggerLevel", trigger);
        facialExpressionTriggerNumber.text = aux.ToString();
    }

    #endregion

    #region update values in UI methods

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
        trigger = (slider.value/10);
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
