// Copyright (c) 2014, Tokyo University of Science All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met: * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. * Neither the name of the Tokyo Univerity of Science nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BITalinoCtrl : MonoBehaviour
{
    [HideInInspector]
    public static BITalinoCtrl Instance { set; get; }
    public ManagerBITalino manager;
    public BITalinoReader reader;
    public BITalinoSerialPort serial;

    public float ecg, emg, acc, eda;


    // Use this for initialization
    public void InitializeBITalino()
    {
        StartCoroutine(start());
    }

    public void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Initialise the connection
    /// </summary>
    private IEnumerator start()
    {
        while (!manager.IsReady)
            yield return new WaitForSeconds(0.5f);
        Debug.Log("BITalino Connected");
        while ((int)manager.Acquisition_State != 0)
            yield return new WaitForSeconds(0.5f);
        Debug.Log("BITalino Acquisition start");
    }

    /// <summary>
    /// Write the data read from the bitalino
    /// </summary>
    public void UpdateBITalino()
    {
        if (reader.asStart)
        {
            ecg = (float)reader.getBuffer()[reader.BufferSize - 1].GetAnalogValue(2);
            emg = (float)reader.getBuffer()[reader.BufferSize - 1].GetAnalogValue(0);
            acc = (float)reader.getBuffer()[reader.BufferSize - 1].GetAnalogValue(4);
            eda = (float)reader.getBuffer()[reader.BufferSize - 1].GetAnalogValue(1);
        }
    }

    # region Getters 

    public float GetEcg()
    {
        return ecg;
    }
    public float GetEmg() {
        return emg;
    }

    public float GetAcc()
    {
        return acc;
    }
    public float GetEda()
    {
        return eda;
    }

    # endregion
}
