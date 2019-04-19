using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SkipBtn : MonoBehaviour
{
    public VideoPlayer player;
    public float readyTime = 31.5f;

    private VideoClip video;
    private double startFrame;

   // private AudioSource _audio; //

    //public AudioClip readySetGo;
    //public AudioClip clockTicTok; //

    public void OnClick()
    {
       // _audio = GetComponent<AudioSource>(); //
       // _audio.PlayOneShot(readySetGo, 1.0f); //
        startFrame = player.frameRate * readyTime;
        player.frame = (long)startFrame;
        gameObject.SetActive(false);
    }

}
