using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPauseBtn : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // VideoPlayer 컴포넌트 연결
    public RawImage rawImage;            // 영상이 표시될 RawImage 연결
    public Button pausePlayButton;       // Pause/Play 버튼 연결

    public GameObject[] activeGroup;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePausePlay();
        }
    }

    public void TogglePausePlay()
    {
        if (isPaused)
        {
            // 일시정지 해제하고 재생
            videoPlayer.Play();

            for(int i = 0; activeGroup.Length > i; i++)
            {
                activeGroup[i].SetActive(false);
            }
        }
        else
        {
            // 비디오 일시정지
            videoPlayer.Pause();

            for (int i = 0; activeGroup.Length > i; i++)
            {
                activeGroup[i].SetActive(true);
            }
        }

        isPaused = !isPaused;
    }
}
