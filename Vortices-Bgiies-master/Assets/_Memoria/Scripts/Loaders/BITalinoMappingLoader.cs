using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BITalinoMappingLoader : MonoBehaviour {

    string interfaceName = "BITalino";

    string[]  triggerNames = new string[]
      {
        "ECG",
        "EMG",
        "ACC",
        "EDA"
      };

    private void OnEnable()
    {
        LoadActions();
    }

    private void LoadActions() {
        /*
        string scope = ProfileManager.Instance.currentEvaluationScope;
        float triggerLevel;
        float lowRangeLevel;
        float highRangeLevel;
        for (int i = 0; i < triggerNames.Length; i++)
        {
            triggerLevel = GLPlayerPrefs.GetFloat(Scope, "Emotiv" + mentalCommandName[i] + "CommandTriggerLevel");
            actionIndex = ActionManager.Instance.GetMappedActionIndex(interfaceName, mentalCommandName[i]);
            ActionManager.Instance.updateActionsEmotivInsight[i] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionEmotiv(mentalCommandCode[i], ticks, mistakes, triggerLevel),
                ActionManager.Instance.currentActionList[actionIndex]);
        }
        */
    }

    void AddAction(int index, double value, double treshold)
    {
        /*
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndex[index] == 0)
            return;

        //CAMBIAR ESTAS DE ABAJITO
        ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionDoubleValueGreaterThan(value, treshold),
           ActionManager.Instance.currentActionList[actionIndex[index]]) //bien
            );
            */
    }


}
