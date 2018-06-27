using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotivMappingLoader : MonoBehaviour {

    string interfaceName = "Emotiv";

    string[] mentalCommandName = new string[]
    {
        "Push",
        "Pull",
        "Lift",
        "Drop",
        "Left"
    };

    Emotiv.EdkDll.IEE_MentalCommandAction_t[] mentalCommandCode = new Emotiv.EdkDll.IEE_MentalCommandAction_t[]
    {
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PUSH,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_PULL,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LIFT,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_DROP,
        Emotiv.EdkDll.IEE_MentalCommandAction_t.MC_LEFT
    };

    string[] facialExpresionName = new string[]
    {
        "LeftWink",
        "RightWink",
        "AnyBlink",
        "Smile"
    };

    Emotiv.EdkDll.IEE_FacialExpressionAlgo_t[] facialExpresionCode = new Emotiv.EdkDll.IEE_FacialExpressionAlgo_t[]
    {
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_LEFT,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_WINK_RIGHT,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_BLINK,
        Emotiv.EdkDll.IEE_FacialExpressionAlgo_t.FE_SMILE
    };

    bool[] facilExpresionIsUpperFace = new bool[]
    {
        true,
        true,
        true,
        false
    };

    int[] actionIndexMental, actionIndexFacial, ticksIndex, mistakesIndex;

    float[] commandTriggerLevelIndex, faceTriggerLevelIndex;

    private void OnEnable()
    {
        LoadActions();
    }

    public void LoadActions()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;

        //Obtaining action index
        actionIndexMental = new int[mentalCommandName.Length];
        actionIndexFacial = new int[facialExpresionName.Length];

        for (int i = 0; i < mentalCommandName.Length; i++)
        {
            actionIndexMental[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, mentalCommandName[i]);
        }

        for (int i = 0; i < facialExpresionName.Length; i++)
        {
            actionIndexFacial[i] = ActionManager.Instance.GetMappedActionIndex(interfaceName, facialExpresionName[i]);
        }

        //Obtaining mental commands ticks, mistakes and trigger level
        ticksIndex = new int[mentalCommandName.Length];
        mistakesIndex = new int[mentalCommandName.Length];
        commandTriggerLevelIndex = new float[mentalCommandName.Length];

        for (int i = 0; i < mentalCommandName.Length; i++)
        {
            ticksIndex[i] = GLPlayerPrefs.GetInt(Scope, interfaceName + mentalCommandName[i] + "CommandTicks");
            mistakesIndex[i] = GLPlayerPrefs.GetInt(Scope, interfaceName + mentalCommandName[i] + "CommandMistakes");
            commandTriggerLevelIndex[i] = GLPlayerPrefs.GetFloat(Scope, interfaceName + mentalCommandName[i] + "CommandTriggerLevel");
        }

        //Facial expression trigger level
        faceTriggerLevelIndex = new float[facialExpresionName.Length];

        for (int i = 0; i < facialExpresionName.Length; i++)
        {
            faceTriggerLevelIndex[i] = GLPlayerPrefs.GetFloat(Scope, interfaceName + facialExpresionName[i] + "TriggerLevel");
        }

        //Tie to action manager
        AddAction(0, mentalCommandCode[0]);
        AddAction(1, mentalCommandCode[1]);
        AddAction(2, mentalCommandCode[2]);
        AddAction(3, mentalCommandCode[3]);
        AddAction(4, mentalCommandCode[4]);

        AddAction(0, facialExpresionCode[0]);
        AddAction(1, facialExpresionCode[1]);
        AddAction(2, facialExpresionCode[2]);
        AddAction(3, facialExpresionCode[3]);
    }

    void AddAction(int index, Emotiv.EdkDll.IEE_MentalCommandAction_t key)
    {
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndexMental[index] == 0)
            return;

        ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionEmotiv(key, ticksIndex[index], mistakesIndex[index], commandTriggerLevelIndex[index]), //condicion bool
           ActionManager.Instance.currentActionList[actionIndexMental[index]]) //accion que se ejecuta
            );

        PrintAddedAction(mentalCommandName[index], actionIndexMental[index], " ticks: " + ticksIndex[index] + " mistakes: " + mistakesIndex[index] + " triggerLevel: " + commandTriggerLevelIndex[index]);
    }

    void AddAction(int index, Emotiv.EdkDll.IEE_FacialExpressionAlgo_t key)
    {
        //if the index is 0 it means the action is null, so no need to add it to the update.
        if (actionIndexFacial[index] == 0)
            return;

        ActionManager.Instance.updateActionArrayList.Add(() => ActionManager.Instance.ActionPairing(
           ActionManager.Instance.ActionConditionEmotiv(key, facilExpresionIsUpperFace[index], faceTriggerLevelIndex[index]), //condicion bool
           ActionManager.Instance.currentActionList[actionIndexFacial[index]]) //accion que se ejecuta
            );

        PrintAddedAction(facialExpresionName[index], actionIndexFacial[index], " triggerLevel: " + faceTriggerLevelIndex[index]);
    }

    /// <summary>
    /// input + action + string with extra data like levels or thresholds
    /// </summary>
    /// <param name="index"></param>
    /// <param name="text"></param>
    void PrintAddedAction(string inputName, int index, string text)
    {
        Debug.Log("Paired: " + inputName + " to " + ActionManager.Instance.currentActionListNames[index] + text);
    }


}
