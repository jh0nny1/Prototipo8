using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotivLogin : MonoBehaviour {

    public ScrolldownContent statusViewScrolldown;
    public InputField userName, password, profileName;
    EmotivCtrl emotivController;

    private void OnEnable()
    {
        emotivController = InterfaceManager.Instance.eegManager.emotivControl;
        emotivController.textOutputFunction = statusViewScrolldown.AddToScrolldown;
        InterfaceManager.Instance.eegManager.StartEmotivInsight();
        emotivController.userName = userName;
        emotivController.password = password;
        emotivController.profileName = profileName;
    }

    public void LoadProfile()
    {
        emotivController.LoadProfile();
    }

    public void SaveProfile()
    {
        emotivController.SaveProfile();
    }
}
