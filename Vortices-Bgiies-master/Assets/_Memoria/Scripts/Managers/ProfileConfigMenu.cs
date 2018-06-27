using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileConfigMenu : MonoBehaviour {

    public Dropdown profilesDropdown;
    public InputField newProfileInputField;
    public GameObject successPanel, profileAlreadyExistLabel;
    public Vortices2ConfigMenu vortices2ConfigMenu;
    public void Awake()
    {        

    }
    // Use this for initialization
    void Start () {
        ReloadProfileDropdown();
    }
	
	public void UpdateCurrentProfile()
    {
        if (ProfileManager.Instance.UpdateCurrentProfile(profilesDropdown.value))
        {
            vortices2ConfigMenu.LoadProfilePreferences();
        }
    }

    public void AddNewProfile()
    {
        string newProfile = newProfileInputField.text;
        if (ProfileManager.Instance.AddNewProfile(newProfile))
        {
            successPanel.SetActive(true);
            ReloadProfileDropdown();
        }
        else
        {
            profileAlreadyExistLabel.SetActive(true);
        }
    }

    void ReloadProfileDropdown()
    {
        profilesDropdown.ClearOptions();
        foreach (string s in ProfileManager.Instance.profiles)
        {
            profilesDropdown.options.Add(new Dropdown.OptionData() { text = s });
        }
        profilesDropdown.value = ProfileManager.Instance.currentProfile;
        profilesDropdown.RefreshShownValue();
    }

}
