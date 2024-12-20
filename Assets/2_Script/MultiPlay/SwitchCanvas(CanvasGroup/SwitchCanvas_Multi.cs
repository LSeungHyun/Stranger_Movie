using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCanvas_Multi : MonoBehaviour
{
    public static SwitchCanvas_Multi instance;
    public GameObject canvasGroup;
    //public GameObject canvasGroup;
    public GameObject titleCanvas_Garo;
    public GameObject titleCanvas_Sero;

    public RawImage rawImage;
    public Image MainBtn;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 Instance가 존재하면 새로 생성된 오브젝트 파괴
        }

#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
        CheckVersion(titleCanvas_Garo);

#endif
        // WebGL이면서 모바일일때
        if (Application.isMobilePlatform)
        {
            CheckVersion(titleCanvas_Sero);
        }

        //WebGL이면서 컴퓨터일때
        else
        {
            CheckVersion(titleCanvas_Garo);
        }
    }

    void CheckVersion(GameObject version)
    {
        if (version.name == "TitleCanvas_Garo")
        {
            titleCanvas_Garo.SetActive(true);
            rawImage = titleCanvas_Garo.transform.Find("LedaScreen").GetComponent<RawImage>();
            MainBtn = titleCanvas_Garo.transform.Find("MainBtn").GetComponent<Image>();
            Destroy(titleCanvas_Sero);
        }
        else
        {
            titleCanvas_Sero.SetActive(true);
            rawImage = titleCanvas_Sero.transform.Find("LedaScreen").GetComponent<RawImage>();
            MainBtn = titleCanvas_Sero.transform.Find("MainBtn").GetComponent<Image>();
            Destroy(titleCanvas_Garo);
        }
    }
}
