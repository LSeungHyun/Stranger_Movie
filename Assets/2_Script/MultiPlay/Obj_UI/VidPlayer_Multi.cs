using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class VidPlayer_Multi : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public string videoUrl = "https://yoonsung00.github.io/VideoHostTutorial/Title_Video.mp4";
    public AudioManager audioManager;
    public SwitchCanvas_Multi switchCanvas;

    public RawImage rawImage;
    public Image MainBtn;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        if (rawImage == null)
        {
            rawImage = switchCanvas.rawImage;
        }

        if (MainBtn == null)
        {
            MainBtn = switchCanvas.MainBtn;
        }

        VideoStarted();
        videoPlayer.Pause();
        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = 0f;
            rawImage.color = color;
        }
    }

    public void VideoStarted()
    {
        if (videoPlayer)
        {
            videoPlayer.url = videoUrl;
            videoPlayer.playOnAwake = false; //기존값은 false되어있지만 다른 영상을 사용하게 될 경우 버그 방지
            videoPlayer.Play();

            if (rawImage != null)
            {
                Color color = rawImage.color;
                color.a = 1f;
                rawImage.color = color;
                if (audioManager != null)
                    audioManager.OffBgmSound();
            }

            MainBtn.gameObject.SetActive(true);
            //videoPlayer.prepareCompleted += OnVideoPrepared;
        }
    }

    public void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.Play();
        if (audioManager != null)
            audioManager.OffBgmSound();
        MainBtn.gameObject.SetActive(true);
    }
}
