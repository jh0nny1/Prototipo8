using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic;

public class BITalinoConfigCanvasController : MonoBehaviour {

    public InputField m_comPort;
    public InputField m_baudRate;
    public InputField m_samplingRate;
    public InputField m_buffSize;

    private string scope;

    private void OnEnable()
    {
        scope = ProfileManager.Instance.currentEvaluationScope;
        SetBITalinoConfigMenuValues();
        //If there is no configuration, print the default values.
        if (m_comPort.text == "")
        {
            m_comPort.text = "COM4";
            m_baudRate.text = "9600";
            m_samplingRate.text = "1000";
            m_buffSize.text = "100";
        }
    }

    void SetBITalinoConfigMenuValues() {
        m_comPort.text = GLPlayerPrefs.GetString(scope, "BITalino ComPort");
        m_baudRate.text = GLPlayerPrefs.GetInt(scope, "BITalino BaudRate").ToString();
        m_samplingRate.text = GLPlayerPrefs.GetInt(scope, "BITalino SamplingRate").ToString();
        m_buffSize.text = GLPlayerPrefs.GetInt(scope, "BITalino BuffSize").ToString();
    }

    private bool ValidateInput()
    {
        return true;
    }

    #region update values in UI methods
    
    public void UpdateBitalinoConfig()
    {
        //VALIDATE
        if (!ValidateInput())
        {
            return;
        }
        else
        {
            string comPort      = m_comPort.text;
            int baudRate        = int.Parse(m_baudRate.text);
            int samplingRate    = int.Parse(m_samplingRate.text);
            int buffSize        = int.Parse(m_buffSize.text);

            GLPlayerPrefs.SetString(scope, "BITalino ComPort", comPort);
            GLPlayerPrefs.SetInt(scope, "BITalino BaudRate", baudRate);
            GLPlayerPrefs.SetInt(scope, "BITalino SamplingRate", samplingRate);
            GLPlayerPrefs.SetInt(scope, "BITalino BuffSize", buffSize);
        }
    }

    # endregion
}
