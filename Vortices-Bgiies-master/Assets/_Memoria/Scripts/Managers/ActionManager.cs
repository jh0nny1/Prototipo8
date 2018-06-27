using Emotiv;
using Memoria;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using Gamelogic;
using Assets.Scripts;

public class ActionManager : MonoBehaviour, IAwake
{
    [HideInInspector]
    public static ActionManager Instance { set; get; }
    [HideInInspector]
    public bool initialized = false;
    public ActionsLimitations actionsLimitations;
    //delete this
    public DIOManager dioManager;
    #region Variable declaration
    //delete this
    [HideInInspector]
    public Action[] vorticesActionList = new Action[8];
    //and this
    [HideInInspector]
    public string[] vorticesActionListNames;
    //dis too
    Action[] bgiiesActionList;
    [HideInInspector]
    //and dis
    public string[] bgiiesActionListNames;
    //[HideInInspector]
    //Current action list is composed of the visualization and object actions lists, same as the array with names.
    //This is to be able to change visualization and objects separately without having to reset the asigned actions.
    public Action[] currentActionList;
    //[HideInInspector]
    public string[] currentActionListNames;
    [HideInInspector]
    public Action[] currentVisualizationActions;
    [HideInInspector]
    public string[] currentVisualizationActionsNames;
    [HideInInspector]
    public Action[] currentObjectActions;
    [HideInInspector]
    public string[] currentObjectActionsNames;
    //delete this
    [HideInInspector]
    public bool bgiiesMode;

    //also delete this, accessed through the interface manager
    public GameObject neuroSkyConfigMenu, emotivConfigMenu, kinectConfigMenu;

    //generic list of actions
    public List<Action> updateActionArrayList = new List<Action>();
    #endregion

    #region Emotiv Variables
    //This list has all the actions that are going to be taken on the update function, so any input pairing with an action you wish
    //  to add should be added to this list using updateActions.Add( ()=> SomeClass.SomeMethod(param1) );
    //EVALUATE DELETE
    [HideInInspector]
    public List<Action> updateActionsVorticesEmotivConfig = new List<Action>();
    [HideInInspector]
    public Action[] updateActionsEmotivInsight;
    [HideInInspector]
    public Action[] updateActionsKinectGestures;
    [HideInInspector]
    public Action[] updateActionsNeuroSky;
    [HideInInspector]
    public float endTime = 1f;
    float tickTimer = 0f;
    float startTime = 0f;
    int emoStateTicks = 0;
    int emoStateTicksMistakes = 0;
	int contador_tap;
	float previus_distance;
	Vector2 firstTouchposs;
	Vector2 secondTouchposs;
	Vector2 currentSwipe;
	float minSwipeLenght = 200f;

    EdkDll.IEE_MentalCommandAction_t currentCommand;
    EdkDll.IEE_MentalCommandAction_t previousCommand;
    int emoStateNeutralTicking = 0;
    #endregion
    // Use this for initialization

    public void Awake()
    {
        Instance = this;
    }

    

    public void Start()
    {
        /*
         * This has all actions in VORTICES, these are:
         * Accept, Inside, Outisde, PossitiveAccept, Negative Accept, zoomin and zoomout
         * Accept is select/deselect image, it works fine.
         * Inside and outside are to change planes, which is also fine         
         * zoom in and out do work, but while an image is zoomed, all the other images can be "pre-selected" with whatever lookpointer option is available
         * possitive accept and nevative accept do not work
         * The null action exist ease the use of the pairing function while taking parameters of dropdown lists of vortices actions, look NeuroSkuConfigMenu and EmotivConfigMenu
         */

        bgiiesActionList = new Action[]
        {
            null,
            () => dioManager.panelBgiies.Inside(),
            () => dioManager.panelBgiies.Outside(),
            () => dioManager.panelBgiies.SelectBt1(),
            () => dioManager.panelBgiies.SelectBt2(),
            () => dioManager.panelBgiies.SelectBt3(),
            () => dioManager.panelBgiies.SelectBt4(),
            () => dioManager.panelBgiies.ZoomIn(),
            () => dioManager.panelBgiies.ZoomOut()
        };

        bgiiesActionListNames = new string[]{
        "No action",
        "Select/Deselect topic 1",
        "Select/Deselect topic 2",
        "Select/Deselect topic 3",
        "Select/Deselect topic 4",
        "Change to next plane",
        "Change to previous plane",
        "Zoom in image",
            "Zoom Out image"

            };

        updateActionsNeuroSky = new Action[3];
        updateActionsEmotivInsight = new Action[9];
        updateActionsKinectGestures = new Action[13];
    }

    public void InitializeManager(DIOManager fatherDioManager)
    {
        dioManager = fatherDioManager;
        initialized = true;
    }

    public void InitializeManager()
    {
        initialized = true;
    }

    public void ReloadProfileDropdown(Dropdown actionListDropdown)
    {
        actionListDropdown.ClearOptions();
        if (bgiiesMode)
        {
            foreach (string s in bgiiesActionListNames)
            {
                actionListDropdown.options.Add(new Dropdown.OptionData() { text = s });
            }
            actionListDropdown.RefreshShownValue();
        }
        else
        {
            foreach (string s in vorticesActionListNames)
            {
                actionListDropdown.options.Add(new Dropdown.OptionData() { text = s });
            }
            actionListDropdown.RefreshShownValue();
        }

    }

    public bool ReloadMappingActions()
    {
        //deletes the previous action list and names by forming them again from the visualization and object arrays
        currentActionList = new Action[currentObjectActions.Length + currentVisualizationActions.Length - 1];
        //For this to work propperly, the first action (index 0) of every array of Visualization/Objects actions MUST BE NULL
        currentVisualizationActions.CopyTo(currentActionList, 0);
        int actionListLen = currentVisualizationActions.Length;
        Action[] aux = new Action[currentObjectActions.Length - 1];
        for (int i = 1; i < currentObjectActions.Length; i++)
        {
            aux[i - 1] = currentObjectActions[i];
        }
        aux.CopyTo(currentActionList, actionListLen);
        return true;
    }

    public void ReloadVisualizationActions(Action[] actions)
    {
        Action[] aux = new Action[actions.Length + 1];
        aux[0] = null;
        for (int i = 1; i <= actions.Length; i++)
        {
            aux[i] = actions[i-1];
        }
        currentVisualizationActions = new Action[actions.Length + 1];
        aux.CopyTo(currentVisualizationActions, 0);
    }

    public void ReloadObjectActions(Action[] actions)
    {
        Action[] aux = new Action[actions.Length + 1];
        aux[0] = null;
        for (int i = 1; i <= actions.Length; i++)
        {
            aux[i] = actions[i - 1];
        }
        currentObjectActions = new Action[actions.Length + 1];
        aux.CopyTo(currentObjectActions, 0);
    }

    /// <summary>
    /// Reconstructs the current action list names from the data on the variables, not the playerprefs
    /// </summary>
    public void ReloadMappingActionsNames()
    {
        // The first NAME must NOT be null, it's ok for the actions array to be 1 larger than the name array
        currentActionListNames = new string[currentObjectActionsNames.Length + currentVisualizationActionsNames.Length + 1];
        currentActionListNames[0] = "No Action";
        currentVisualizationActionsNames.CopyTo(currentActionListNames, 1);
        currentObjectActionsNames.CopyTo(currentActionListNames, currentVisualizationActionsNames.Length + 1);
    }

    /// <summary>
    /// Deletes all options in the dropdown and adds available actions as options
    /// </summary>
    /// <param name="availableActionsDropdown"></param>
    public void ReloadMappingActionsDropdown(Dropdown availableActionsDropdown)
    {
        availableActionsDropdown.ClearOptions();
        foreach (string s in currentActionListNames)
        {
            availableActionsDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
    }

    /// <summary>
    /// Updates the currentActionListNames with object actions names given
    /// </summary>
    /// <param name="actionNamesArray"></param>
    public void UpdateObjectActionNames(string[] actionNamesArray)
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        currentObjectActionsNames = new string[actionNamesArray.Length];
        actionNamesArray.CopyTo(currentObjectActionsNames, 0);
        GLPlayerPrefs.SetStringArray(Scope, "CurrentInformationObjectActionsNames", actionNamesArray);
        ReloadMappingActionsNames();
    }

    /// <summary>
    /// Updates the currentActionListNames with visualization actions names given
    /// </summary>
    /// <param name="actionNamesArray"></param>
    public void UpdateVisualizationActionNames(string[] actionNamesArray)
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        currentVisualizationActionsNames = new string[actionNamesArray.Length];
        actionNamesArray.CopyTo(currentVisualizationActionsNames, 0);
        GLPlayerPrefs.SetStringArray(Scope, "CurrentVisualizationActionsNames", actionNamesArray);
        ReloadMappingActionsNames();
    }

    /// <summary>
    /// Loads from the playerprefs the current object and visualization action names and asigns them to their variables
    /// </summary>
    public void LoadMappingActionsNames()
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string[] aux = GLPlayerPrefs.GetStringArray(Scope, "CurrentInformationObjectActionsNames");
        currentObjectActionsNames = new string[aux.Length];
        aux.CopyTo(currentObjectActionsNames, 0);
        string[] aux2 = GLPlayerPrefs.GetStringArray(Scope, "CurrentVisualizationActionsNames");
        currentVisualizationActionsNames = new string[aux2.Length];
        aux2.CopyTo(currentVisualizationActionsNames, 0);
        ReloadMappingActionsNames();
    }

    //So, how the logic works. The first action of the array of actions of both the currentVisualization and currentInformationObject arrays is a null.
    //Then if both are zero, there is no action asigned.
    //The current available actions array is a combination of both the visualization and object actions array, always having the visualization first and the
    //      object array copied right after.
    //The first array is always the Visualization one, so his index always matches, from 0 (no action) to whatever limit (say 5, to say an example).
    //Following, the object index is always dependant on the length of the visualization index. For example if the size of the visualization index
    //      is 6 (0 to 5), the first action of the object index will be 6 in the current actions index. For that reason, the object index is transformed
    //      into an action available index by considering the length of the visualization index.
    //In the case someone changes the visualization, the configuration of the object actions will remain, because they're relative to the visualization index
    //      and not dependant of it.

    /// <summary>
    /// Returns the current action list index of the given input, 0 by default.
    /// </summary>
    /// <param name="interfaceName"></param>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public int GetMappedActionIndex(string interfaceName, string inputName)
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        string currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        int visIndex = GLPlayerPrefs.GetInt(Scope, interfaceName + inputName + currentVisualization + "VisualizationIndex");
        int objIndex = GLPlayerPrefs.GetInt(Scope, interfaceName + inputName + currentObject + "ObjectIndex");
        if (visIndex != 0)
        {
            return visIndex;
        }
        else if (objIndex != 0)
        {
            int aux = objIndex + currentVisualizationActionsNames.Length;
            return aux;
        }
        else
        {
            return 0;
        }
    }

    //stores the data following the GetMappedActionIndex logic. The key is separated into interface name, input name and current visualization
    //      and information object to be able to change them without losing the already stored configurations.
    /// <summary>
    /// Sets the current action list index of the given input
    /// </summary>
    /// <param name="interfaceName"></param>
    /// <param name="inputName"></param>
    /// <param name="indexValue"></param>
    /// <returns></returns>
    public int SetMappedActionIndex(string interfaceName, string inputName, int indexValue)
    {
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        string currentVisualization = GLPlayerPrefs.GetString(Scope, "CurrentVisualization");
        string currentObject = GLPlayerPrefs.GetString(Scope, "CurrentInformationObject");
        int aux;
        if (indexValue == 0)
        {
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentVisualization + "VisualizationIndex", 0);
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentObject + "ObjectIndex", 0);
            return 0;
        }
        else if (indexValue <= currentVisualizationActionsNames.Length)
        {
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentVisualization + "VisualizationIndex", indexValue);
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentObject + "ObjectIndex", 0);
            return indexValue;
        }
        else
        {
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentVisualization + "VisualizationIndex", 0);
            aux = indexValue - currentVisualizationActionsNames.Length;
            GLPlayerPrefs.SetInt(Scope, interfaceName + inputName + currentObject + "ObjectIndex", aux);
            return aux;
        }
    }

    /// <summary>
    /// Returns array with input mapped with their corresponding actions, meant to be displayed in the evaluation summary
    /// </summary>
    /// <param name="interfaceName"></param>
    /// <param name="inputActionsNames"></param>
    /// <returns></returns>
    public string[] GetMappedActionsListNames(string interfaceName, string[] inputActionsNames)
    {
        string[] result = new string[inputActionsNames.Length];
        string aux;
        for (int i = 0; i < inputActionsNames.Length; i++)
        {
            aux = currentActionListNames[GetMappedActionIndex(interfaceName, inputActionsNames[i])];
            result[i] = "[·]" + interfaceName + " " + inputActionsNames[i] + " as "+aux;
        }
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetStringArray(Scope, interfaceName + "SummaryActions", result);
        return result;
    }

    /// <summary>
    /// Overload to add strings in between the interface and input name and input name and action.
    /// </summary>
    /// <param name="interfaceName"></param>
    /// <param name="inputActionsNames"></param>
    /// <returns></returns>
    public string[] GetMappedActionsListNames(string interfaceName, string[] inputActionsNames, string beforeInputName, string afterInputName)
    {
        string[] result = new string[inputActionsNames.Length];
        string aux;
        for (int i = 0; i < inputActionsNames.Length; i++)
        {
            aux = currentActionListNames[GetMappedActionIndex(interfaceName, inputActionsNames[i])];
            result[i] = "[·]" + interfaceName + " " + beforeInputName + " " + inputActionsNames[i] + " " + afterInputName + " as " + aux;
        }
        string Scope = ProfileManager.Instance.currentEvaluationScope;
        GLPlayerPrefs.SetStringArray(Scope, interfaceName + "SummaryActions", result);
        return result;
    }

    void Update()
    {
        if (!initialized)
            return;
        foreach (var function in updateActionsVorticesEmotivConfig)
        {
            function();
        }

        foreach(var action in updateActionArrayList)
        {
            action();
        }
        //currentActionList[1]();
        /*
        if (EEGManager.Instance.useNeuroSky)
        {
            foreach (Action function in updateActionsNeuroSky)
            {
                if (function != null)
                    function();
            }
        }

        if (dioManager.kinectInput)
        {
            foreach (Action function in updateActionsKinectGestures)
            {
                if (function != null)
                    function();
            }
        }
        */
    }

    //This function is called every time the emo state is updated. The tickTimer is there so that the functions are not all called every single update, but rather only
    //  after a fixed amount of time. If the ticks are used, it means that each fixed amount of second (1 by defaul, setted by the endTime variable) the current emo state
    //  is compared with the previous one, if it's the same, a emoStateTick is added, if it's different, it resets to 0. So, for example, if you keep the same emo state for
    //  four seconds, you would get four emoStateTicks.
    //  Then, using the overloaded version of ActionConditionEmotiv with the ticks parameter, you can set how many ticks you want the action to be triggered.

    //Say, for example, you set the PUSH command to trigger an action with 2 ticks, so for said action to be triggered the user needs to be focused for 2 seconds on
    //  the same mental command. Then you can use the PULL command to trigger an action with 4 ticks for example, to completely avoid any unwanted triggering of the function.
    //
    //The ticks update very second, but this time is a float value, you can lower (or increase) this as much as you want. At endTime 0.5f for example, you could use 5 ticks so that
    //  the mental command needs to be active for 2.5 seconds.
    //
    //In order to allow for mistakes caused either by lack of concentration, distractions or equipment, a emoStateTicksMistake counter is set. If, for example, the user needs 4 ticks with the
    //  Push command, has 3 ticks and when the 4º arrives the mental command is Lift, the emoStateTick will not reset nor rise, but the emoStateTickMistake will be reduced. So if in the next
    //  tick the user goes back to Push, it'll trigger the fourth tick, otherwise the mistake counter will continue to rise until it reaches the amount specified in the ActiveConditionEmotiv function,
    //  in which case no more mistakes are allowed and the tick counter resets. The mistake counter is reseted once a new command intention is detected
    public void EmoStateUpdate()
    {
        //Debug.Log(EEGManager.Instance.mentalCommand);
        float elapsedTime = Time.unscaledTime - startTime;
        tickTimer = endTime - elapsedTime;
        if (tickTimer <= 0f)
        {            
            if (EEGManager.Instance.MentalCommandCurrentAction == previousCommand)
            {                
                if (emoStateTicks == 0)
                {
                    //new mental command intention detected
                    //If this new command is neutral, it's not stored as current (as it should not have any actions attached)
                    if(EEGManager.Instance.MentalCommandCurrentAction != EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL)
                    {
                        currentCommand = EEGManager.Instance.MentalCommandCurrentAction;
                        emoStateTicks++;
                        emoStateTicksMistakes = 0;
                        emoStateNeutralTicking = 0;
                    }                    
                }
                else if (EEGManager.Instance.MentalCommandCurrentAction == currentCommand)
                {   emoStateTicks++;    }
                else
                {
                    emoStateTicksMistakes++;
                    if (EEGManager.Instance.MentalCommandCurrentAction == EdkDll.IEE_MentalCommandAction_t.MC_NEUTRAL)
                    {
                        //This "if" exist only as a precaution. The reset condition for the tick variable exist only in the ActionConditionEmotiv function. If by any reason there is a
                        //    mental command that IS being detected but it has NOT been included into an ActionConditionEmotiv function, it will trigger an infinite loop that would prevent
                        //    any new mental commands to be triggered. To avoid any such case, the neutral command is used to reset the tick counter, regardless of the active action.
                        emoStateNeutralTicking++;
                        if (emoStateNeutralTicking > 6)
                        {
                            emoStateNeutralTicking = 0;
                            emoStateTicks = 0;
                            emoStateTicksMistakes = 0;
                        }
                    }
                }                
            }
            else
            {
                if (EEGManager.Instance.MentalCommandCurrentAction == currentCommand)
                {   emoStateTicks++;    }
                else
                {   emoStateTicksMistakes++;    }
            }
            
            foreach (var function in updateActionsEmotivInsight)
            {
                function();
            }            
            previousCommand = EEGManager.Instance.MentalCommandCurrentAction;
            startTime = Time.unscaledTime;
            tickTimer = endTime;
        }        
    }

    public void KinectGestureUpdate()
    {
        foreach (Action function in updateActionsKinectGestures)
        {
            if (function != null)
                function();
        }
    }

    //Adds a condition that is a mental command in Emotiv, to be used with ActionPairing function
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition){
        if(EEGManager.Instance.MentalCommandCurrentAction == condition)
            return true;
        return false;
    }

    //Overload version of the Emotiv condition to include the ticks mechanic.
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition, int ticks, int mistakes)
    {
        if (EEGManager.Instance.MentalCommandCurrentAction == condition && emoStateTicks >= ticks)
        {
            emoStateTicks = 0;
            return true;
        }else if(mistakes <= emoStateTicksMistakes && currentCommand == condition){
            emoStateTicks = 0;
        }            
        return false;
    }

    //Another overload to accept mental power too
    public bool ActionConditionEmotiv(EdkDll.IEE_MentalCommandAction_t condition, int ticks, int mistakes, float mentalPower)
    {
        if (EEGManager.Instance.MentalCommandCurrentAction == condition && emoStateTicks >= ticks && mentalPower >= EEGManager.Instance.MentalCommandCurrentActionPower)
        {
            emoStateTicks = 0;
            return true;
        }
        else if (mistakes <= emoStateTicksMistakes && currentCommand == condition)
        {
            emoStateTicks = 0;
        }
        return false;
    }

    //another condition to accept facial expresions with power
    public bool ActionConditionEmotiv(EdkDll.IEE_FacialExpressionAlgo_t facialExpression, bool isUpperFace, float statePower)
    {
        if (EEGManager.Instance.FacialExpressionLowerFaceAction == facialExpression && EEGManager.Instance.FacialExpressionUpperFaceActionPower >= statePower && isUpperFace)
        {
            return true;
        }else if (EEGManager.Instance.FacialExpressionLowerFaceAction == facialExpression && EEGManager.Instance.FacialExpressionLowerFaceActionPower >= statePower && !isUpperFace)
        {
            return true;
        }
        return false;
    }

    //Adds a condition that is an int value that has to be greater than the second value
    public bool ActionConditionIntValueGreaterThan(ref int value, ref int threshold)
    {
        if(value > threshold)
        {
            return true;
        }
        return false;
    }

    //Overload so the treshold isn't a reference
    public bool ActionConditionIntValueGreaterThan(ref int value, int threshold)
    {
        if (value > threshold)
        {
            return true;
        }
        return false;
    }

    //Adds a condition that is an int value that has to be greater than the second value
    public bool ActionConditionDoubleValueGreaterThan(ref double value, ref double threshold)
    {
        if (value > threshold)
        {
            return true;
        }
        return false;
    }

    //Adds a condition that is a key button, to be used with ActionPairing function
    public bool ActionConditionButtons(KeyCode button){
        if (Input.GetKeyDown(button))
        {
            Debug.Log("key pressed");
            return true;
        }            
        return false;
    }

    //kinect hand detection
    public bool ActionConditionKinect(HandState gesture, bool isRightHand)
    {
        if (isRightHand)
        {
            if (gesture == KinectDetectGestures.kinectCurrentRightHandGesture)
            {
                return true;
            }
        }
        else
        {
            if (gesture == KinectDetectGestures.kinectCurrentLeftHandGesture)
            {
                return true;
            }
        }        
        return false;
    }

    public bool ActionConditionKinect(int gestureIndex, float gestureTrigger)
    {
        if (KinectCommandConfigMenu.currentGestureContinuous.name == "")
            return false;
        if (KinectCommandConfigMenu.gestureNames[gestureIndex] == KinectCommandConfigMenu.currentGestureContinuous.name)
        {
            if (KinectCommandConfigMenu.currentGestureContinuous.result >= gestureTrigger && KinectCommandConfigMenu.gestureActive[gestureIndex])
            {
                KinectCommandConfigMenu.gestureActive[gestureIndex] = false;
                return true;
            }
        }
        return false;
    }


	public bool ActionLeap(int index){

		if (InterfaceManager.Instance.leapMotionManager.GestureActive [index]) {
			InterfaceManager.Instance.leapMotionManager.GestureActive [index] = false;
			return true;
		} else {
			return false;
		}
	}

	public bool ActionTouch(int index){

		if (InterfaceManager.Instance.touchScreenManager.GestureActive[index]) {
			InterfaceManager.Instance.touchScreenManager.GestureActive[index] = false;
			return true;
		} else {
			return false;
		}
	}


	//A simple condition->function template to be used with an action condition and any function, meant to be added to any updateActions lists.
    /// <summary>
    /// If condition is met, triggers function
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="function"></param>
    public void ActionPairing(bool condition, Action function){
        if (condition)
            function();
    }

    //An overload to allow the bool to be a reference to a variable
    /// <summary>
    /// if condition is met, triggers function
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="function"></param>
    public void ActionPairing(ref bool condition, Action function)
    {
        if (condition)
            function();
    }

    //An overload of ActionPairing meant to add a function as a consequence of the condition being false, in case it's needed.
    /// <summary>
    /// if condition is met, triggers function, if not, triggers consequence
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="function"></param>
    /// <param name="consequence"></param>
    public void ActionPairing(bool condition, Action function, Action consequence)
    {
        if (condition)
        {
            function();
        }
        else
        {
            consequence();
        }
            
    }

    /// <summary>
    /// If condition and validation are met, triggers function
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="function"></param>
    /// <param name="validation"></param>
    public void ActionPairing(bool condition, Action function, bool validation)
    {
        if (condition)
        {
            if (validation)
            {
                function();
            }
        }
    }

    /// <summary>
    /// If any value of the array is false, returns false. To be used with ActionPairing as validation. 
    /// </summary>
    /// <param name="conditions"></param>
    /// <returns></returns>
    public bool ActionValidation(bool[] conditions)
    {
        bool isValid = true;
        foreach(bool condition in conditions)
        {
            if (!condition)
            {
                isValid = false;
            }
        }
        return isValid;
    }

    /// <summary>
    /// If any value of the list is false, returns false. To be used with ActionPairing as validation. 
    /// </summary>
    /// <param name="conditions"></param>
    /// <returns></returns>
    public bool ActionValidation(List<bool> conditions)
    {
        bool isValid = true;
        foreach (bool condition in conditions)
        {
            if (!condition)
            {
                isValid = false;
            }
        }
        return isValid;
    }
}
