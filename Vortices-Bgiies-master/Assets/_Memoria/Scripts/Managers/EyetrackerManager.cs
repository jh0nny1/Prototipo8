using Gamelogic;
using Memoria;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;

public class EyetrackerManager : MonoBehaviour, IAwake
{

    [HideInInspector]
    public static EyetrackerManager Instance { set; get; }
    [HideInInspector]
    protected DIOManager dioManager;
    private bool initialized = false;
    private string Scope = "Vortices2Config";
    #region Variable declaration
    /*
     * No more than one eye tracker can be active at a time, as they all fulfill the same function.
     * Beware, no regulatory script in this regard have been writen because in the future eye tracking hardware could provide different kind of inputs.
     */

    public bool _useEyetribe ;
    public bool _useMouse;

    /*
     * This variable will hold the ray to the screenpoint.
     */
    public Ray screenPoint;

    #endregion

    #region Initialization
    public void Awake()
    {
        Instance = this;
    }

    public void InitializeManager(DIOManager fatherDioManager)
    {
        dioManager = fatherDioManager;
        Scope = ProfileManager.Instance.currentEvaluationScope;
        _useEyetribe = GLPlayerPrefs.GetBool(Scope, "UseTheEyeTribe");
        Debug.Log("Use eyetribe:" + GLPlayerPrefs.GetBool(Scope, "UseTheEyeTribe").ToString());
        _useMouse = GLPlayerPrefs.GetBool(Scope, "UseMouse");
        Debug.Log("Use mouse:" + GLPlayerPrefs.GetBool(Scope, "UseMouse").ToString());
        if (_useEyetribe)
        {
            GazeCamera.Instance.Initialize();
        }
        else
        {
            NegateEyeTribe();
        }

        initialized = true;
    }


        #endregion

        

        #region Update functions
        // Update is called once per frame
    void Update(){
        if (!initialized)
            return;        
        UpdateEyetribe();
        if (_useMouse)
            screenPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    void UpdateEyetribe () {
        if (_useEyetribe)
        {
            GazeCamera.Instance.UpdateGazeCamera();
            screenPoint = Camera.main.ScreenPointToRay(GazeCamera.Instance.getScreenPoint());
        }

	}

    #endregion

    #region Eye Tribe functions
    void NegateEyeTribe()
    {
        _useEyetribe = false;
        GazeCamera.Instance.useGazeTracker = false;
    }
    #endregion
}
