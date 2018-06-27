using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;
using Memoria;
using System;
using Microsoft.Kinect.VisualGestureBuilder;

public class KinectCommandConfigMenu : MonoBehaviour {

    public struct gesturesContinuous
    {
        public string name;
        public float result;

        public gesturesContinuous(string name, float result)
        {
            this.name = name;
            this.result = result;
        }
    }

    public static gesturesContinuous currentGestureContinuous;

    public DIOManager dioManager;

    public Dropdown basicGesturesActionsDropdown, basicGesturesDropdown, dbGesturesActionsDropdown, dbGesturesDropdown;
    public Slider dbGestureTriggerLevel, dbGestureUntriggerLevel;
    float gesture1TriggerLevel, gesture2TriggerLevel, gesture3TriggerLevel, gesture4TriggerLevel, gesture5TriggerLevel, gesture6TriggerLevel, gesture7TriggerLevel;
    public static float gesture1UntriggerLevel, gesture2UntriggerLevel, gesture3UntriggerLevel, gesture4UntriggerLevel, gesture5UntriggerLevel, gesture6UntriggerLevel, gesture7UntriggerLevel;
    int openHandRightAssignedActionIndex, openHandLeftAssignedActionIndex, closeHandRightAssignedActionIndex, closeHandLeftAssignedActionIndex, lassoHandRightAssignedActionIndex, lassoHandLeftAssignedActionIndex, gesture1AssignedActionIndex, gesture2AssignedActionIndex, gesture3AssignedActionIndex, gesture4AssignedActionIndex, gesture5AssignedActionIndex, gesture6AssignedActionIndex, gesture7AssignedActionIndex;
    public Text dbGestureTriggerNumber, dbGestureUntriggerNumber;
    public Text openHandRightAssignedActionText, openHandLeftAssignedActionText, closeHandRightAssignedActionText, closeHandLeftAssignedActionText, lassoHandRightAssignedActionText, lassoHandLeftAssignedActionText, gesture1AssignedActionText, gesture2AssignedActionText, gesture3AssignedActionText, gesture4AssignedActionText, gesture5AssignedActionText, gesture6AssignedActionText, gesture7AssignedActionText;
    string Scope;

    public static string[] gestureNames = {"HandUpProgress", "HandDownProgress", "HandRightProgress", "HandLeftProgress", null, null, null };
    public static bool[] gestureActive = { true, true, true, true, true, true, true };
    void OnEnable()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;

        if (ActionManager.Instance.bgiiesMode)
        {
            gesture1TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1TriggerLevelBgiies");
            gesture2TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2TriggerLevelBgiies");
            gesture3TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3TriggerLevelBgiies");
            gesture4TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4TriggerLevelBgiies");
            gesture5TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5TriggerLevelBgiies");
            gesture6TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6TriggerLevelBgiies");
            gesture7TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7TriggerLevelBgiies");
            gesture1UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1UntriggerLevelBgiies");
            gesture2UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2UntriggerLevelBgiies");
            gesture3UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3UntriggerLevelBgiies");
            gesture4UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4UntriggerLevelBgiies");
            gesture5UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5UntriggerLevelBgiies");
            gesture6UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6UntriggerLevelBgiies");
            gesture7UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7UntriggerLevelBgiies");

            openHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandRightAssignedActionIndexBgiies");
            openHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandLeftAssignedActionIndexBgiies");
            closeHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandRightAssignedActionIndexBgiies");
            closeHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandLeftAssignedActionIndexBgiies");
            lassoHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandRightAssignedActionIndexBgiies");
            lassoHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandLeftAssignedActionIndexBgiies");
            gesture1AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture1AssignedActionIndexBgiies");
            gesture2AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture2AssignedActionIndexBgiies");
            gesture3AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture3AssignedActionIndexBgiies");
            gesture4AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture4AssignedActionIndexBgiies");
            gesture5AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture5AssignedActionIndexBgiies");
            gesture6AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture6AssignedActionIndexBgiies");
            gesture7AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture7AssignedActionIndexBgiies");

        }
        else
        {
            gesture1TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1TriggerLevelVortices");
            gesture2TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2TriggerLevelVortices");
            gesture3TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3TriggerLevelVortices");
            gesture4TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4TriggerLevelVortices");
            gesture5TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5TriggerLevelVortices");
            gesture6TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6TriggerLevelVortices");
            gesture7TriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7TriggerLevelVortices");
            gesture1UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture1UntriggerLevelVortices");
            gesture2UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture2UntriggerLevelVortices");
            gesture3UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture3UntriggerLevelVortices");
            gesture4UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture4UntriggerLevelVortices");
            gesture5UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture5UntriggerLevelVortices");
            gesture6UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture6UntriggerLevelVortices");
            gesture7UntriggerLevel = GLPlayerPrefs.GetFloat(Scope, "gesture7UntriggerLevelVortices");

            openHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandRightAssignedActionIndexVortices");
            openHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "openHandLeftAssignedActionIndexVortices");
            closeHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandRightAssignedActionIndexVortices");
            closeHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "closeHandLeftAssignedActionIndexVortices");
            lassoHandRightAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandRightAssignedActionIndexVortices");
            lassoHandLeftAssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "lassoHandLeftAssignedActionIndexVortices");
            gesture1AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture1AssignedActionIndexVortices");
            gesture2AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture2AssignedActionIndexVortices");
            gesture3AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture3AssignedActionIndexVortices");
            gesture4AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture4AssignedActionIndexVortices");
            gesture5AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture5AssignedActionIndexVortices");
            gesture6AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture6AssignedActionIndexVortices");
            gesture7AssignedActionIndex = GLPlayerPrefs.GetInt(Scope, "gesture7AssignedActionIndexVortices");
        }
        

        SetActionNameByIndex(gesture1AssignedActionText, gesture1AssignedActionIndex);
        SetActionNameByIndex(gesture2AssignedActionText, gesture2AssignedActionIndex);
        SetActionNameByIndex(gesture3AssignedActionText, gesture3AssignedActionIndex);
        SetActionNameByIndex(gesture4AssignedActionText, gesture4AssignedActionIndex);
        SetActionNameByIndex(gesture5AssignedActionText, gesture5AssignedActionIndex);
        SetActionNameByIndex(gesture6AssignedActionText, gesture6AssignedActionIndex);
        SetActionNameByIndex(gesture7AssignedActionText, gesture7AssignedActionIndex);

        SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
        SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
        SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
        SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
        SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
        SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);

        CleanKinectActions();
        SetBasicGesturesConfigMenuValues();
        SetDbGesturesConfigMenuValues();
        ActionManager.Instance.ReloadProfileDropdown(dbGesturesActionsDropdown);
        ActionManager.Instance.ReloadProfileDropdown(basicGesturesActionsDropdown);
    }

    private void OnDisable()
    {
        UpdateActionsKinect();

        if (ActionManager.Instance.bgiiesMode)
        {
            GLPlayerPrefs.SetInt(Scope, "openHandRightAssignedActionIndexBgiies", openHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "openHandLeftAssignedActionIndexBgiies", openHandLeftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "closeHandRightAssignedActionIndexBgiies", closeHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "closeHandLeftAssignedActionIndexBgiies", closeHandLeftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "lassoHandRightAssignedActionIndexBgiies", lassoHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "lassoHandLeftAssignedActionIndexBgiies", lassoHandLeftAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "gesture1AssignedActionIndexBgiies", gesture1AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture2AssignedActionIndexBgiies", gesture2AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture3AssignedActionIndexBgiies", gesture3AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture4AssignedActionIndexBgiies", gesture4AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture5AssignedActionIndexBgiies", gesture5AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture6AssignedActionIndexBgiies", gesture6AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture7AssignedActionIndexBgiies", gesture7AssignedActionIndex);

            GLPlayerPrefs.SetFloat(Scope, "gesture1TriggerLevelBgiies", gesture1TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture2TriggerLevelBgiies", gesture2TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture3TriggerLevelBgiies", gesture3TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture4TriggerLevelBgiies", gesture4TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture5TriggerLevelBgiies", gesture5TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture6TriggerLevelBgiies", gesture6TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture7TriggerLevelBgiies", gesture7TriggerLevel);

            GLPlayerPrefs.SetFloat(Scope, "gesture1UntriggerLevelBgiies", gesture1UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture2UntriggerLevelBgiies", gesture2UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture3UntriggerLevelBgiies", gesture3UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture4UntriggerLevelBgiies", gesture4UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture5UntriggerLevelBgiies", gesture5UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture6UntriggerLevelBgiies", gesture6UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture7UntriggerLevelBgiies", gesture7UntriggerLevel);
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, "openHandRightAssignedActionIndexVortices", openHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "openHandLeftAssignedActionIndexVortices", openHandLeftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "closeHandRightAssignedActionIndexVortices", closeHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "closeHandLeftAssignedActionIndexVortices", closeHandLeftAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "lassoHandRightAssignedActionIndexVortices", lassoHandRightAssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "lassoHandLeftAssignedActionIndexVortices", lassoHandLeftAssignedActionIndex);

            GLPlayerPrefs.SetInt(Scope, "gesture1AssignedActionIndexVortices", gesture1AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture2AssignedActionIndexVortices", gesture2AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture3AssignedActionIndexVortices", gesture3AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture4AssignedActionIndexVortices", gesture4AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture5AssignedActionIndexVortices", gesture5AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture6AssignedActionIndexVortices", gesture6AssignedActionIndex);
            GLPlayerPrefs.SetInt(Scope, "gesture7AssignedActionIndexVortices", gesture7AssignedActionIndex);

            GLPlayerPrefs.SetFloat(Scope, "gesture1TriggerLevelVortices", gesture1TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture2TriggerLevelVortices", gesture2TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture3TriggerLevelVortices", gesture3TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture4TriggerLevelVortices", gesture4TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture5TriggerLevelVortices", gesture5TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture6TriggerLevelVortices", gesture6TriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture7TriggerLevelVortices", gesture7TriggerLevel);

            GLPlayerPrefs.SetFloat(Scope, "gesture1UntriggerLevelVortices", gesture1UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture2UntriggerLevelVortices", gesture2UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture3UntriggerLevelVortices", gesture3UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture4UntriggerLevelVortices", gesture4UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture5UntriggerLevelVortices", gesture5UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture6UntriggerLevelVortices", gesture6UntriggerLevel);
            GLPlayerPrefs.SetFloat(Scope, "gesture7UntriggerLevelVortices", gesture7UntriggerLevel);
        }

        

    }

    #region Manage actions added

    void CleanKinectActions()
    {
        for (int i = 0; i < ActionManager.Instance.updateActionsKinectGestures.Length; i++)
        {
            ActionManager.Instance.updateActionsKinectGestures[i] = null;
        }
    }

    public void UpdateActionsKinect()
    {

        // First six slots are for the basic gestures
        //
        //
        if (openHandRightAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[0] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Open, true),
                ActionManager.Instance.currentActionList[openHandRightAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[0] = null;
        }

        if (openHandLeftAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[1] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Open, false),
                ActionManager.Instance.currentActionList[openHandLeftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[1] = null;
        }

        if (closeHandRightAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[2] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Closed, true),
                ActionManager.Instance.currentActionList[closeHandRightAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[2] = null;
        }

        if (closeHandLeftAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[3] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Closed, false),
                ActionManager.Instance.currentActionList[closeHandLeftAssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[3] = null;
        }

        if (lassoHandRightAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[4] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Lasso, true),
                //ActionManager.Instance.currentActionList[lassoHandRightAssignedActionIndex]
                ActionManager.Instance.currentActionList[1]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[4] = null;
        }

        if (lassoHandLeftAssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[5] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(Windows.Kinect.HandState.Lasso, false),
                //ActionManager.Instance.currentActionList[lassoHandLeftAssignedActionIndex]
                ActionManager.Instance.currentActionList[2]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[5] = null;
        }

        // Seven slots are for the maximum amount of database read gestures
        //
        //

        if (gesture1AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[6] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(0, gesture1TriggerLevel),
                //ActionManager.Instance.currentActionList[gesture1AssignedActionIndex]
                ActionManager.Instance.currentActionList[3]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[6] = null;
        }

        if (gesture2AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[7] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(1, gesture2TriggerLevel),
                //ActionManager.Instance.currentActionList[gesture1AssignedActionIndex]
                ActionManager.Instance.currentActionList[4]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[7] = null;
        }

        if (gesture3AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[8] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(2, gesture3TriggerLevel),
                //ActionManager.Instance.currentActionList[gesture1AssignedActionIndex]
                ActionManager.Instance.currentActionList[5]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[8] = null;
        }

        if (gesture4AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[9] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(3, gesture4TriggerLevel),
                //ActionManager.Instance.currentActionList[gesture1AssignedActionIndex]
                ActionManager.Instance.currentActionList[6]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[9] = null;
        }

        if (gesture5AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[10] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(4, gesture5TriggerLevel),
                ActionManager.Instance.currentActionList[gesture5AssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[10] = null;
        }

        if (gesture6AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[11] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(5, gesture6TriggerLevel),
                ActionManager.Instance.currentActionList[gesture6AssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[11] = null;
        }

        if (gesture7AssignedActionIndex != 0)
        {
            ActionManager.Instance.updateActionsKinectGestures[12] = () => ActionManager.Instance.ActionPairing(
                ActionManager.Instance.ActionConditionKinect(6, gesture7TriggerLevel),
                ActionManager.Instance.currentActionList[gesture7AssignedActionIndex]
                );
        }
        else
        {
            ActionManager.Instance.updateActionsKinectGestures[12] = null;
        }

        Debug.Log("Action asignation completed");
    }
    /*
    public static void CurrentGestureUpdate(List<GestureContinuous> currentGestures, IDictionary<GestureContinuous, ContinuousGestureResult> continuosResults)
    {
        gesturesContinuous currentGesture = new gesturesContinuous();
        SetCurrentGesture(new gesturesContinuous("", 0));
        float maxGestureTrigger = 0;
        for (int i = 0; i < currentGestures.Count; i++)
        {
            foreach(string name in gestureNames)
            {
                if (name == currentGestures[i].Name)
                {
                    ContinuousGestureResult result = null;
                    continuosResults.TryGetValue(currentGestures[i], out result);
                    switch (Array.IndexOf(gestureNames, name))
                    {                     
                        case 0:

                            if (result.Progress <= gesture1UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                            {
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            }
                            break;
                        case 1:
                            if (result.Progress <= gesture2UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                            {
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            }
                            break;
                        case 2:
                            if (result.Progress <= gesture3UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                            {
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;

                            }
                            break;
                        case 3:
                            if (result.Progress <= gesture4UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                            {
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            }
                            break;
                        case 4:
                            if (result.Progress <= gesture5UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                            {
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            }
                            break;
                        case 5:
                            if (result.Progress <= gesture6UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            break;
                        case 6:
                            if (result.Progress < gesture7UntriggerLevel && !gestureActive[Array.IndexOf(gestureNames, name)])
                                gestureActive[Array.IndexOf(gestureNames, name)] = true;
                            break;
                    }
                    if (result.Progress >= maxGestureTrigger)
                    {
                        maxGestureTrigger = result.Progress;
                        currentGesture = new gesturesContinuous(name, result.Progress);
                    }
                }
            }
            //Debug.Log("current gesture Name " + currentGesture.name + " trigger " + currentGesture.result);
        }
        Debug.Log("**************************************maximun " + currentGesture.name + " trigger " + currentGesture.result);
        if (maxGestureTrigger == 0)
            SetCurrentGesture(new gesturesContinuous("", 0));
        else
            SetCurrentGesture(currentGesture);
    }
    */
    #endregion

    #region UI triggers

    public void SetDbGesturesConfigMenuValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                dbGesturesActionsDropdown.value = gesture1AssignedActionIndex;
                SetTriggerValues(gesture1TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture1UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 1:
                dbGesturesActionsDropdown.value = gesture2AssignedActionIndex;
                SetTriggerValues(gesture2TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture2UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 2:
                dbGesturesActionsDropdown.value = gesture3AssignedActionIndex;
                SetTriggerValues(gesture3TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture3UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 3:
                dbGesturesActionsDropdown.value = gesture4AssignedActionIndex;
                SetTriggerValues(gesture4TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture4UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 4:
                dbGesturesActionsDropdown.value = gesture5AssignedActionIndex;
                SetTriggerValues(gesture5TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture5UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 5:
                dbGesturesActionsDropdown.value = gesture6AssignedActionIndex;
                SetTriggerValues(gesture6TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture6UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 6:
                dbGesturesActionsDropdown.value = gesture7AssignedActionIndex;
                SetTriggerValues(gesture7TriggerLevel * 10, dbGestureTriggerLevel, dbGestureTriggerNumber);
                SetTriggerValues(gesture7UntriggerLevel * 10, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
        }
        dbGesturesActionsDropdown.RefreshShownValue();
    }

    public void SetBasicGesturesConfigMenuValues()
    {
        switch (basicGesturesDropdown.value)
        {
            case 0:
                basicGesturesActionsDropdown.value = openHandRightAssignedActionIndex;
                SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
                break;
            case 1:
                basicGesturesActionsDropdown.value = openHandLeftAssignedActionIndex;
                SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
                break;
            case 2:
                basicGesturesActionsDropdown.value = closeHandRightAssignedActionIndex;
                SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
                break;
            case 3:
                basicGesturesActionsDropdown.value = closeHandLeftAssignedActionIndex;
                SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
                break;
            case 4:
                basicGesturesActionsDropdown.value = lassoHandRightAssignedActionIndex;
                SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
                break;
            case 5:
                basicGesturesActionsDropdown.value = lassoHandLeftAssignedActionIndex;
                SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);
                break;
        }
        basicGesturesActionsDropdown.RefreshShownValue();
    }

    public void UpdateDbGesturesActionDropdownValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                gesture1AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture1AssignedActionText, gesture1AssignedActionIndex);
                break;
            case 1:
                gesture2AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture2AssignedActionText, gesture2AssignedActionIndex);
                break;
            case 2:
                gesture3AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture3AssignedActionText, gesture3AssignedActionIndex);
                break;
            case 3:
                gesture4AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture4AssignedActionText, gesture4AssignedActionIndex);
                break;
            case 4:
                gesture5AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture5AssignedActionText, gesture5AssignedActionIndex);
                break;
            case 5:
                gesture6AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture6AssignedActionText, gesture6AssignedActionIndex);
                break;
            case 6:
                gesture7AssignedActionIndex = dbGesturesActionsDropdown.value;
                SetActionNameByIndex(gesture7AssignedActionText, gesture7AssignedActionIndex);
                break;
        }
    }

    public void UpdateDbGestureTriggerValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref gesture1TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref gesture2TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref gesture3TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref gesture4TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 4:
                UpdateTriggerValues(ref gesture5TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 5:
                UpdateTriggerValues(ref gesture6TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
            case 6:
                UpdateTriggerValues(ref gesture7TriggerLevel, dbGestureTriggerLevel, dbGestureTriggerNumber);
                break;
        }
    }

    public void UpdateDbGestureUntriggerValues()
    {
        switch (dbGesturesDropdown.value)
        {
            case 0:
                UpdateTriggerValues(ref gesture1UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 1:
                UpdateTriggerValues(ref gesture2UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 2:
                UpdateTriggerValues(ref gesture3UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 3:
                UpdateTriggerValues(ref gesture4UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 4:
                UpdateTriggerValues(ref gesture5UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 5:
                UpdateTriggerValues(ref gesture6UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
            case 6:
                UpdateTriggerValues(ref gesture7UntriggerLevel, dbGestureUntriggerLevel, dbGestureUntriggerNumber);
                break;
        }
    }


    public void UpdateBasicGesturesActionDropdownValues()
    {
        switch (basicGesturesDropdown.value)
        {
            case 0:
                openHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(openHandRightAssignedActionText, openHandRightAssignedActionIndex);
                break;
            case 1:
                openHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(openHandLeftAssignedActionText, openHandLeftAssignedActionIndex);
                break;
            case 2:
                closeHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(closeHandRightAssignedActionText, closeHandRightAssignedActionIndex);
                break;
            case 3:
                closeHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(closeHandLeftAssignedActionText, closeHandLeftAssignedActionIndex);
                break;
            case 4:
                lassoHandRightAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(lassoHandRightAssignedActionText, lassoHandRightAssignedActionIndex);
                break;
            case 5:
                lassoHandLeftAssignedActionIndex = basicGesturesActionsDropdown.value;
                SetActionNameByIndex(lassoHandLeftAssignedActionText, lassoHandLeftAssignedActionIndex);
                break;
        }
    }

    #endregion

    #region update values in UI methods

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
        //Debug.Log("trigger: " + trigger.ToString());
        slider.value = trigger;
        int aux = (int)trigger;
        text.text = aux.ToString();
    }

    void SetActionNameByIndex(Text text, int index)
    {
        if (ActionManager.Instance.bgiiesMode)
        {
            text.text = ActionManager.Instance.bgiiesActionListNames[index];
        }
        else
        {
            text.text = ActionManager.Instance.vorticesActionListNames[index];
        }
    }

    public static void SetCurrentGesture(gesturesContinuous gesture)
    {
        currentGestureContinuous = gesture;
    }
    #endregion
}
