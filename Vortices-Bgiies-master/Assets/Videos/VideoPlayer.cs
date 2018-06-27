using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoPlayer : MonoBehaviour {

    public MovieTexture video;
    //AudioSource audio;
	// Use this for initialization
	void Start () {
        GetComponent<RawImage>().texture = video as MovieTexture;
        //audio = GetComponent<AudioSource>();
        //audio.clip = video.audioClip;
	}

    private void OnEnable()
    {
        video.Play();
        //audio.Play();
    }

    private void OnDisable()
    {
        video.Stop();
        //audio.Stop();
    }

    
}
