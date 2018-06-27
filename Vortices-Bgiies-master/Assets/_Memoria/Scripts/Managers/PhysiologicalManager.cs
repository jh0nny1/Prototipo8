using Memoria;
using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;

public class PhysiologicalManager : MonoBehaviour { //*NOTE: NO HEREDA DE LOS MISMOS QUE EL EEGManager

    [HideInInspector]
    public static PhysiologicalManager Instance { set; get; }
    private bool initialized = false;
    private string scope = "Vortices2Config";

    #region Variable declaration
    /*
     * These are the input variables 
     */

    public bool useBITalino;    
    [HideInInspector]
    public float ecg;
    [HideInInspector]
    public float ecgTrigger;
    [HideInInspector]
    public float emg;
    [HideInInspector]
    public float emgTrigger;
    [HideInInspector]
    public float acc;
    [HideInInspector]
    public float accTrigger;
    [HideInInspector]
    public float eda;
    [HideInInspector]
    public float edaTrigger;

    #endregion

    #region Initialization
    
    public void InitializeManager() 
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        useBITalino = GLPlayerPrefs.GetBool(scope, "UseBITalino");
        Debug.Log("Use BITalino:" + GLPlayerPrefs.GetBool(scope, "UseBITalino").ToString());
        if (useBITalino)
        {  
            BITalinoCtrl.Instance.InitializeBITalino();
        }
        else
        {
            NegateBITalino();
        }

        initialized = true;
    }

    // Use this for initialization
    public void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Update functions
    // Update is called once per frame
    void Update()
    {
        if (!initialized || !useBITalino)
            return;
        else
        {
            BITalinoCtrl.Instance.UpdateBITalino();
            ecg = BITalinoCtrl.Instance.GetEcg();
            emg = BITalinoCtrl.Instance.GetEmg();
            acc = BITalinoCtrl.Instance.GetAcc();
            eda = BITalinoCtrl.Instance.GetEda();
        }
    }

    #endregion


    #region BITalino functions
    /*
     * BITalino functions
     * 
     */

    void NegateBITalino()
    {
        useBITalino = false;
    }

    #endregion
}
