using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroSkyMappingLoader : MonoBehaviour {

    string interfaceName = "NeuroSky";

    string[] keyName = new string[]
    {
        "Attention",
        "Blink",
        "Meditation"
    };

    int[] actionIndex, triggerLevelIndex;


    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        actionIndex = new int[keyName.Length];
        triggerLevelIndex = new int[keyName.Length];

        for (int i = 0; i < keyName.Length; i++)
        {
            actionIndex[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, keyName[i]);
            triggerLevelIndex[i] = GLPlayerPrefs.GetInt(Scope, interfaceName + keyName[i] + "TriggerLevel");
        }        

        AddAction(0);
        AddAction(1);
        AddAction(2);
    }

    void AddAction(int index)
    {
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndex[index] == 0)
            return;
        
        //They are different because they all hold a reference to a different constantly changing value in the EEG Manager
        if(index == 0)
        {
           //Attention Level
           ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.attentionLevel, triggerLevelIndex[index]), //condicion bool
           ActionManager.Instance.currentActionList[actionIndex[index]]) //accion que se ejecuta
            );
        }else if(index == 1)
        {
           //Blink Strength
           ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.blinkStrength, triggerLevelIndex[index]), //condicion bool
           ActionManager.Instance.currentActionList[actionIndex[index]]) //accion que se ejecuta
            );
        }else if(index == 2)
        {
           //Meditation
           ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionIntValueGreaterThan(ref EEGManager.Instance.meditationLevel, triggerLevelIndex[index]), //condicion bool
           ActionManager.Instance.currentActionList[actionIndex[index]]) //accion que se ejecuta
            );
        }

        //For debug purposes
        PrintAddedAction(index, " threshold: "+triggerLevelIndex[index]);
    }

    /// <summary>
    /// shows input + action paired with
    /// </summary>
    /// <param name="index"></param>
    void PrintAddedAction(int index)
    {
        Debug.Log("Paired: " + keyName[index] + " to " + ActionManager.Instance.currentActionListNames[actionIndex[index]]);
    }

    /// <summary>
    /// input + action + string with extra data like levels or thresholds
    /// </summary>
    /// <param name="index"></param>
    /// <param name="text"></param>
    void PrintAddedAction(int index, string text)
    {
        Debug.Log("Paired: " + keyName[index] + " to " + ActionManager.Instance.currentActionListNames[actionIndex[index]] + text);
    }
}
