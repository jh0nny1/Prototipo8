using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadProfileName : MonoBehaviour {
    public Text currentUserProfileText;
    bool initialized = false;

    private void OnEnable()
    {
        if (!initialized)
            return;
        UpdateCurrentUserProfileText();
        
    }

    // Use this for initialization
    void Start () {
        initialized = true;
        UpdateCurrentUserProfileText();
	}

    public void UpdateCurrentUserProfileText()
    {
        currentUserProfileText.text = ProfileManager.Instance.profileScope;
    }
}
