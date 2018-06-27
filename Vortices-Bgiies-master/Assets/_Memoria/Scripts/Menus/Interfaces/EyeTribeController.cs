using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class EyeTribeController : MonoBehaviour {

    public Toggle showGazeIcon;
    string interfaceName = "EyeTribe";
    string Scope;

    private void OnEnable()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        showGazeIcon.isOn = GLPlayerPrefs.GetBool(Scope, interfaceName + "ShowGazeIcon");
    }

    public void UpdateShowGazeIcon()
    {
        GLPlayerPrefs.SetBool(Scope, interfaceName + "ShowGazeIcon", showGazeIcon.isOn);
    }

    public void OpenCalibration()
    {
        SceneAdministrator.Instance.ChangeScene("calib_scene");
    }
}
