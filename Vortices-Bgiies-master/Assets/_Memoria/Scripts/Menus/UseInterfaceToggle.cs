using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class UseInterfaceToggle : MonoBehaviour {
    public string interfaceName;
    Toggle instance;
    string Scope, key;

    private void Awake()
    {
        instance = transform.GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        Scope = ProfileManager.Instance.currentEvaluationScope;
        key = "use" + interfaceName;
        instance.isOn = GLPlayerPrefs.GetBool(Scope, key);
    }

    public void UpdateSelectedInterface()
    {
        GLPlayerPrefs.SetBool(Scope, key, instance.isOn);
    }
}
