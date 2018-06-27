using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

/*
 * FIX: IF YOU WOULD WANT ASIGN DIFFERENT ACTIONS TO DIFFERENT TRIGGERS ON THE SAME PHYISICAL RESPONSE
 *      YOU WONT BE ABLE, BECAUSE IT ACTIONS ARE RELATED WITH THE PHYSICAL RESPONSE ITSELF AND NOT WITH
 *      THE TRIGGER
*/

public class BITalinoMappingController : MonoBehaviour {

    public ActionMapingController actionMapController;

    private string interfaceName = "BITalino";
    private string currentVisualization;
    private string currentObject;
    private string scope;

    public Dropdown physicalResponseDropdown, triggerWithDropdown, actionDropdown;
    public Slider tresholdSlider, lowestRangeSlider, highestRangeSlider;
    public Text tresholdNumber, lowestRangeNumber, highestRangeNumber;

    static public GameObject tresholdSettings, rangeSettings;

    public GameObject[] triggerWithSettings = new GameObject[] {
        rangeSettings,
        tresholdSettings
    };

    /*  Range Settings = 0
     *  TresholdSettings = 1
     *  etc...
    */

    string[] physicalResponse = new string[]
    {
        "ECG",
        "EMG",
        "ACC",
        "EDA"
    };

    static string[] ecgTriggers = new string[] {
        "Range",
        "Treshold"
    };
    static string[] emgTriggers = new string[] {
        "Range",
        "Treshold"
    };
    static string[] accTriggers = new string[] {
        "Range",
        "Treshold"
    };
    static string[] edaTriggers = new string[] {
        "Range",
        "Treshold"
    };

    List<string[]> triggers = new List<string[]>()
    {
        ecgTriggers,
        emgTriggers,
        accTriggers,
        edaTriggers
    };

    private void OnEnable()
    {
        currentVisualization = GLPlayerPrefs.GetString(scope, "CurrentVisualization");
        currentObject = GLPlayerPrefs.GetString(scope, "CurrentInformationObject");
        scope = ProfileManager.Instance.currentEvaluationScope;

        //Reload Dropdowns
        AddArrayToDropdown(physicalResponseDropdown, physicalResponse);
        AddArrayToDropdown(triggerWithDropdown, triggers[physicalResponseDropdown.value]);
        ActionManager.Instance.ReloadMappingActionsDropdown(actionDropdown);

        SetBITalinoMappingControllerValues();
    }

    #region update values in UI methods

    public void SetBITalinoMappingControllerValues()
    {
        int physicalIndex = physicalResponseDropdown.value;
        actionDropdown.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, physicalResponse[physicalIndex]);
        UpdateMappedActions(physicalResponse);
        UpdateSettingsCanvas();
    }

    public void UpdateTresholdValues()
    {
        float value = (float)tresholdSlider.value;
        int physicalResponseIndex = physicalResponseDropdown.value;

        tresholdNumber.text = value.ToString("0.0");

        GLPlayerPrefs.SetFloat(scope, "BITalino" + physicalResponse[physicalResponseIndex] + "Treshold", value);
    }

    public void UpdateLowestRangeValues()
    {
        float value = (float)lowestRangeSlider.value;
        int physicalResponseIndex = physicalResponseDropdown.value;

        lowestRangeNumber.text = value.ToString("0.0");

        GLPlayerPrefs.SetFloat(scope, "BITalino" + physicalResponse[physicalResponseIndex] + "LowestRange", value);
    }

    public void UpdateHighestRangeValues()
    {
        float value = (float)highestRangeSlider.value;
        int physicalResponseIndex = physicalResponseDropdown.value;

        highestRangeNumber.text = value.ToString("0.0");

        GLPlayerPrefs.SetFloat(scope, "BITalino" + physicalResponse[physicalResponseIndex] + "HighestRange", value);
    }

    void SetTriggerValues(float trigger, Slider slider, Text text)
    {
        slider.value = trigger;
        text.text = trigger.ToString("0.0");
    }    

    public void UpdatePhysicalResponseActionDropdownValues()
    {
        
        int physicalIndex = physicalResponseDropdown.value;
        int action = actionDropdown.value;
        ActionManager.Instance.SetMappedActionIndex(interfaceName, physicalResponse[physicalIndex], action);
        UpdateMappedActions(physicalResponse);
    }

    public void UpdateTriggerWithDropdown()
    {
        int index = physicalResponseDropdown.value;

        AddArrayToDropdown(triggerWithDropdown, triggers[index]);
        actionDropdown.value = ActionManager.Instance.GetMappedActionIndex(interfaceName, physicalResponse[index]);
        //FIX: This update (triggerWithDropdown value) must be implemented by a method based on the actual 
        //physical response, and not a simple int (0, in this case)
        triggerWithDropdown.value = 0;
        UpdateSettingsCanvas();
        actionDropdown.RefreshShownValue();
    }

    public void UpdateSettingsCanvas()
    {
        CleanSettingsCanvas();

        int physicalResponseIndex = physicalResponseDropdown.value;
        int triggerWithIndex = triggerWithDropdown.value;

        string treshold = triggers[physicalResponseIndex][triggerWithIndex];

        switch (treshold)
        {
            case "Range":
                triggerWithSettings[0].SetActive(true);
                SetTriggerValues(GLPlayerPrefs.GetFloat(scope, interfaceName + physicalResponse[physicalResponseIndex] + "LowestRange"), lowestRangeSlider, lowestRangeNumber);
                SetTriggerValues(GLPlayerPrefs.GetFloat(scope, interfaceName + physicalResponse[physicalResponseIndex] + "HighestRange"), highestRangeSlider, highestRangeNumber);
                break;
            case "Treshold":
                triggerWithSettings[1].SetActive(true);
                SetTriggerValues(GLPlayerPrefs.GetFloat(scope, interfaceName + physicalResponse[physicalResponseIndex] + "Treshold"), tresholdSlider, tresholdNumber);
                break;
        }
    }

    public void CleanSettingsCanvas()
    {
        for(int i = 0; i < triggerWithSettings.Length; i++)
        {
            triggerWithSettings[i].SetActive(false);
        }
    }

    void AddArrayToDropdown(Dropdown inputDropdown, string[] labels)
    {
        inputDropdown.ClearOptions();
        foreach (string s in labels)
        {
            inputDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        inputDropdown.RefreshShownValue();
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
    #endregion
}


