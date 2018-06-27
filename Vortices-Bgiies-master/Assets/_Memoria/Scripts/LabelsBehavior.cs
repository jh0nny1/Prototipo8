using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelsBehavior : MonoBehaviour {
    
    public Text adaptativeLabel;
    public string m_label;

    void Start () {
        SetLabel(m_label);
    }

    public void SetVisualLabel()
    {
        adaptativeLabel.text = "Visual Immersion = " + immersionToString(PlayerPrefs.GetInt("Visual Immersion"));
    }

    public void SetAuditiveLabel()
    {
        adaptativeLabel.text = "Auditive Immersion = " + immersionToString(PlayerPrefs.GetInt("Auditive Immersion"));
    }

    public void SetLabel(string label)
    {
        adaptativeLabel.text = label + " = " + immersionToString(PlayerPrefs.GetInt(label));
    }

    string immersionToString(int intLevel)
    {
        string stringLevel;

        switch (intLevel) { 
            case 0:
                stringLevel = "Min";
                return stringLevel;
            case 1:
                stringLevel = "Very Low"; 
                return stringLevel;
            case 2:
                stringLevel = "Low"; 
                return stringLevel;
            case 3:
                stringLevel = "Medium"; 
                return stringLevel;
            case 4:
                stringLevel = "High"; 
                return stringLevel;
            case 5:
                stringLevel = "Very High";
                return stringLevel;
            case 6:
                stringLevel = "Max";
                return stringLevel;
            default:
                return "Error";
        }
    }
}
