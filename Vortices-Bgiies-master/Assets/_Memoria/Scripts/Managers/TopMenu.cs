using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class TopMenu : MonoBehaviour {

    public Button emotivButton, eyetribeButton, neuroskyButton, kinectButton;

    private void OnEnable()
    {
        bool useEmotiv = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseEmotivInsight");
        if (!useEmotiv)
        {
            emotivButton.interactable = false;
        }
        else
        {
            emotivButton.interactable = true;
        }

        bool useNeuroSky = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseNeuroSkyMindwave");
        if (!useNeuroSky)
        {
            neuroskyButton.interactable = false;
        }
        else
        {
            neuroskyButton.interactable = true;
        }

        bool useEyeTribe = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "UseTheEyeTribe");
        if (!useEyeTribe)
        {
            eyetribeButton.interactable = false;
        }
        else
        {
            eyetribeButton.interactable = true;
        }

        bool useKinect = GLPlayerPrefs.GetBool(ProfileManager.Instance.currentEvaluationScope, "KinectInput");
        if (!useKinect)
        {
            kinectButton.interactable = false;
        }
        else
        {
            kinectButton.interactable = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
