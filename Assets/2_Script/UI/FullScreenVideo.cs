using UnityEngine;
using UnityEngine.UI;

public class FullScreenVideo : MonoBehaviour
{
    public RawImage videoRawImage;
    public Image underBarImage;
    public Image playImage;
    public Button fullScreenBtn;
    public Button pausePlayBtn;

    private bool isFullScreen = false;
    public bool isSero = false;

    private Vector2 originalSizeDelta;
    private Vector2 originalScale;
    private Vector2 originalAnchoredPosition;

    void Awake()
    {
        // 원래 크기와 위치 저장
        originalSizeDelta = videoRawImage.rectTransform.sizeDelta;
        originalScale = videoRawImage.rectTransform.localScale;
        originalAnchoredPosition = videoRawImage.rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isFullScreen) {
            ToggleFullScreen();
        }
    }

    public void ToggleFullScreen()
    {
        if (isFullScreen)
        {
            // 전체 화면 해제: 원래 크기와 위치로 되돌림
            videoRawImage.rectTransform.sizeDelta = originalSizeDelta;
            videoRawImage.rectTransform.anchoredPosition = originalAnchoredPosition;
            videoRawImage.rectTransform.localScale = originalScale;
            videoRawImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            videoRawImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            if (isSero)
            {
                // 영상 출력되는 RawImage
                videoRawImage.rectTransform.localEulerAngles = Vector3.zero;

                // 전체화면 버튼
                fullScreenBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 41f);
                fullScreenBtn.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);

                // 영상 일시정지 버튼
                pausePlayBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -495.5f);
                pausePlayBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(1920f, 995f);

                // 언더바 이미지
                underBarImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                underBarImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 85f);

                // 플레이 이미지
                playImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(40f, 41f);
                playImage.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
            }
        }
        else
        {
            // 전체 화면 활성화: 카메라 Viewport를 기준으로 확장
            videoRawImage.rectTransform.anchorMin = new Vector2(0, 0);
            videoRawImage.rectTransform.anchorMax = new Vector2(1, 1);
            videoRawImage.rectTransform.sizeDelta = Vector2.zero; // 크기 자동 조정
            videoRawImage.rectTransform.anchoredPosition = Vector2.zero; // 중앙 정렬

            if (isSero)
            {
                // 영상 출력되는 RawImage
                videoRawImage.rectTransform.localScale = new Vector2(1.7088f, 0.5424f);
                videoRawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -90);

                // 전체화면 버튼
                fullScreenBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30f, 75.5f);
                fullScreenBtn.GetComponent<RectTransform>().localScale = new Vector2(0.565f, 1.78f);

                // 영상 일시정지 버튼
                pausePlayBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -875f);
                pausePlayBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(995f, 1750f);

                // 언더바 이미지
                underBarImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1f);
                underBarImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 151.5f);


                // 플레이 이미지
                playImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, 72f);
                playImage.GetComponent<RectTransform>().localScale = new Vector2(0.565f, 1.78f);
            }

            else
            {
                videoRawImage.rectTransform.localScale = Vector2.one;
            }
        }

        isFullScreen = !isFullScreen;
    }
}