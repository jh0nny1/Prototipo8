using Gamelogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vortices2ConfigMenu : MonoBehaviour {

    public Toggle useEmotiv, useNeuroSky, useMouse, useEyeTribe;
    public InputField emotivDataPath, neuroskyDataPath, eyeTribeDataPath;

    private void Start()
    {
        LoadProfilePreferences();   
    }

    public void LoadProfilePreferences()
    {
        useEmotiv.isOn = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseEmotivInsight");
        useNeuroSky.isOn = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseNeuroSkyMindwave");
        useEyeTribe.isOn = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseTheEyeTribe");
        useMouse.isOn = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseMouse");

        if (useEmotiv.isOn)
            useNeuroSky.interactable = false;

        if (useNeuroSky.isOn)
            useEmotiv.interactable = false;

        if (useEyeTribe.isOn)
            useMouse.interactable = false;

        if (useMouse.isOn)
            useEyeTribe.interactable = false;

        emotivDataPath.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "EmotivInsightDataPath");
        neuroskyDataPath.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "NeuroSkyMindwaveDataPath");
        eyeTribeDataPath.text = GLPlayerPrefs.GetString(ProfileManager.Instance.currentEvaluationScope, "TheEyeTribeDataPath");
    }

    public void toggleEEGEmotiv()
    {
        if (useEmotiv.isOn)
        {
            useNeuroSky.isOn = false;
            useNeuroSky.interactable = false;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseEmotivInsight", true);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseNeuroSkyMindwave", false);
        }
        else
        {
            useNeuroSky.interactable = true;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseEmotivInsight", false);
        }
    }

    public void toggleEEGNeuroSky()
    {
        if (useNeuroSky.isOn)
        {
            useEmotiv.isOn = false;
            useEmotiv.interactable = false;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseEmotivInsight", false);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseNeuroSkyMindwave", true);
        }
        else
        {
            useEmotiv.interactable = true;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseNeuroSkyMindwave", false);
        }
    }

    public void toggleEyeTrackerEyeTribe()
    {
        if (useEyeTribe.isOn)
        {
            useMouse.isOn = false;
            useMouse.interactable = false;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseTheEyeTribe", true);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseMouse", false);
        }
        else
        {
            useMouse.interactable = true;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseTheEyeTribe", false);
        }
    }

    public void toggleEyeTrackerMouse()
    {
        if (useMouse.isOn)
        {
            useEyeTribe.isOn = false;
            useEyeTribe.interactable = false;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseMouse", true);
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseTheEyeTribe", false);
        }
        else
        {
            useEyeTribe.interactable = true;
            GLPlayerPrefs.SetBool(ProfileManager.Instance.currentEvaluationScope, "UseMouse", false);
        }
    }

    public void savePathEmotiv()
    {
        GLPlayerPrefs.SetString(ProfileManager.Instance.currentEvaluationScope, "EmotivInsightDataPath", emotivDataPath.text);
    }

    public void savePathNeuroSky()
    {
        GLPlayerPrefs.SetString(ProfileManager.Instance.currentEvaluationScope, "NeuroSkyMindwaveDataPath", neuroskyDataPath.text);
    }

    public void savePathEyeTribe()
    {
        GLPlayerPrefs.SetString(ProfileManager.Instance.currentEvaluationScope, "TheEyeTribeDataPath", eyeTribeDataPath.text);
    }

}
