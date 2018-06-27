using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Memoria;
using Gamelogic;

public class AudioPreSettings : MonoBehaviour
{
    public void AudioConfiguration(string scope)
    {
        int immersionLevel = GLPlayerPrefs.GetInt(scope, "Auditive Immersion Level");
        AudioConfiguration audioConfig = AudioSettings.GetConfiguration();

        switch (immersionLevel)
        {
            case 0:
                audioConfig.speakerMode = AudioSpeakerMode.Mono;
                audioConfig.sampleRate = 22050;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 1:
                audioConfig.speakerMode = AudioSpeakerMode.Mono;
                audioConfig.sampleRate = 22050;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 2:
                audioConfig.speakerMode = AudioSpeakerMode.Mono;
                audioConfig.sampleRate = 32000;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 3:
                audioConfig.speakerMode = AudioSpeakerMode.Stereo;
                audioConfig.sampleRate = 44100;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 4:

                audioConfig.speakerMode = AudioSpeakerMode.Stereo;
                audioConfig.sampleRate = 48000;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 5:

                audioConfig.speakerMode = AudioSpeakerMode.Stereo;
                audioConfig.sampleRate = 48000;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            case 6:

                audioConfig.speakerMode = AudioSpeakerMode.Stereo;
                audioConfig.sampleRate = 96000;
                audioConfig.dspBufferSize = 4096;
                AudioSettings.Reset(audioConfig);
                break;
            default:
                Debug.Log("Error: Auditive Immersion Level not allowed");
                break;
        }

    }
}
