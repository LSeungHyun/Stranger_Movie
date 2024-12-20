using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwtichCanvas_Game : MonoBehaviour
{
    static public SwtichCanvas_Game instance;

    //public GameObject canvasGroup;
    public GameObject titleCanvas_Garo;
    public GameObject titleCanvas_Sero;

//    void Awake()
//    {

//        // 캔버스 그룹을 참조
//        //canvasGroup = GameObject.Find("CanvasGroup");

//#if !(!UNITY_EDITOR && UNITY_WEBGL)
//        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
//        CheckVersion(titleCanvas_Garo);

//#endif
//        // WebGL이면서 모바일일때
//        if (Application.isMobilePlatform)
//        {
//            CheckVersion(titleCanvas_Sero);
//        }

//        //WebGL이면서 컴퓨터일때
//        else
//        {
//            CheckVersion(titleCanvas_Garo);
//        }
//    }
    void Awake()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL이 아닌 환경에서는 조이스틱 off 및 Unity 이동 함수 사용
        CheckVersion(titleCanvas_Garo);
#else
        // WebGL 환경에서 iPad 여부를 확인
        Application.ExternalEval(@"
            if (IsIPad()) {
                SendMessage('YourGameObjectName', 'OnIPadDetected', true);
            } else {
                SendMessage('YourGameObjectName', 'OnIPadDetected', false);
            }
        ");
#endif
    }

    // iPad 여부를 받아오는 메서드
    void OnIPadDetected(bool isIPad)
    {
        if (isIPad)
        {
            // iPad일 경우 실행할 코드
            CheckVersion(titleCanvas_Sero);
        }
        else if (Application.isMobilePlatform)
        {
            // iPad가 아닌 모바일일 경우 실행할 코드
            CheckVersion(titleCanvas_Sero);
        }
        else
        {
            // 컴퓨터일 경우 실행할 코드
            CheckVersion(titleCanvas_Garo);
        }
    }

    void CheckVersion(GameObject version)
    {
        if(version.name == "TitleCanvas_Garo")
        {
            titleCanvas_Garo.SetActive(true);
            Destroy(titleCanvas_Sero);
        }
        else
        {
            titleCanvas_Sero.SetActive(true);
            Destroy(titleCanvas_Garo);
        }
    }
}
