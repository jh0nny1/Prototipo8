using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineConnectionMenu : MonoBehaviour {

    private void OnEnable()
    {
        InterfaceManager.Instance.eegManager.emotivControl.CheckUserStorageDataPaths();
    }
}
