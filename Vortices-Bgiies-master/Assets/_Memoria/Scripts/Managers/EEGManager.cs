using Emotiv;
using Memoria;
using Gamelogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityCallbacks;
using UnityEngine;

public class EEGManager : MonoBehaviour, IAwake, IFixedUpdate {

    [HideInInspector]
    public static EEGManager Instance { set; get; }
    private string Scope = "Vortices2Config";

    #region Variable declaration
    /*
     * These are the input variables 
     */


    /*
     * 
     * NeuroSky variables
     * 
     */
    public NeuroSkyData neuroSkyControl;
    [HideInInspector]
    public bool useNeuroSky = false; //*NOTE: EQUIVALENTES A LOS VALORES DE ECG, EMG, ACC y EDA 
    [HideInInspector]
    public int blinkStrength;
    [HideInInspector]
    public int blinkStrengthTrigger;
    [HideInInspector]
    public int meditationLevel;
    [HideInInspector]
    public int meditationLevelTrigger;
    [HideInInspector]
    public int attentionLevel;
    [HideInInspector]
    public int attentionLevelTrigger;

    /*
     * 
     * Emotiv Insight variables
     * 
     */
    public EmotivCtrl emotivControl;
    [HideInInspector]
    public bool useEmotivInsight = false;
    [HideInInspector]
    public EdkDll.IEE_MentalCommandAction_t MentalCommandCurrentAction;
    [HideInInspector]
    public float MentalCommandCurrentActionPower;
    [HideInInspector]
    public bool FacialExpressionIsRightEyeWinking;
    [HideInInspector]
    public bool FacialExpressionIsLeftEyeWinking;
    [HideInInspector]
    public bool FacialExpressionIsUserBlinking;
    [HideInInspector]
    public float FacialExpressionUpperFaceActionPower;
    [HideInInspector]
    public float FacialExpressionSmileExtent;
    [HideInInspector]
    public float FacialExpressionLowerFaceActionPower;
    [HideInInspector]
    public EdkDll.IEE_FacialExpressionAlgo_t FacialExpressionLowerFaceAction;
    [HideInInspector]
    public EdkDll.IEE_FacialExpressionAlgo_t FacialExpressionUpperFaceAction;

    #endregion

    #region Initialization
    public void CheckInterfaces() //*NOTE: INICIALIZA LOS SERVICIOS DE LA INTERAFAZ
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        useEmotivInsight = GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight");
        Debug.Log("Use emotiv:"+GLPlayerPrefs.GetBool(Scope, "UseEmotivInsight").ToString());
        if (useEmotivInsight){  //*NOTE: CHEQUEA SI LA INTERFAZ EFECTIVAMENTE SE VA A UTILIZAR
            emotivControl.gameObject.SetActive(true);
            emotivControl.StartEmotivInsight();            
        }else{
            NegateEmotivInsight();
            emotivControl.gameObject.SetActive(false);
        }

        useNeuroSky = GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave");
        Debug.Log("Use neurosky:" + GLPlayerPrefs.GetBool(Scope, "UseNeuroSkyMindwave").ToString());

        if (useNeuroSky)
        {
            neuroSkyControl.gameObject.SetActive(true);
            neuroSkyControl.StartNeuroSkyData();
        }else{
            NegateNeuroSky();
            neuroSkyControl.gameObject.SetActive(false);
        }
    }

    //Use this for configuration
    public void StartEmotivInsight()
    {
        //this check is to avoid re-enable if already started.
        if (useEmotivInsight)
            return;

        emotivControl.StartEmotivInsight();
        useEmotivInsight = true;
    }

    public void StartNeuroSky()
    {
        if (useNeuroSky)
            return;

        neuroSkyControl.StartNeuroSkyData();
        useNeuroSky = true;
    }

    // Use this for initialization
    public void Awake () {
        Instance = this;
    }
    
    #endregion

    #region Update functions
    // Update is called once per frame
    void Update () {

        if (useEmotivInsight)
        {
            emotivControl.UpdateEmotivInsight();

        }

	}

    public void FixedUpdate()
    {
        if (!useNeuroSky)
            return;

        neuroSkyControl.ResetBlink(); //*NOTE: AQUI TOMA LOS VALORES
        blinkStrength = neuroSkyControl.getBlink();
        attentionLevel = neuroSkyControl.getAttention();
        meditationLevel = neuroSkyControl.getMeditation();
    }

    #endregion

    #region Neuro Sky functions
    /*
     * Neuro Sky functions
     * 
     */
    
    void NegateNeuroSky()
    {
        useNeuroSky = false;
        neuroSkyControl.gameObject.SetActive(false);
    }

    #endregion

    #region Emotiv Insight Functions
    void NegateEmotivInsight()
    {
        useEmotivInsight = false;
        emotivControl.gameObject.SetActive(false);
    }

    #endregion
}
