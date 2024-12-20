using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [Header("Managers")]
    public DialManager dialManager;
    public QuestManager questManager;

    [Header("TextWindow")]
    public GameObject textWindow;
    public Text textComponent;    
    private void Awake()
    {
        GetManagers();
        
        SubscribeToSceneEvents();

        CloseText();
    }

    private void OnDestroy()
    {
        UnsubscribeFromSceneEvents();
    }

    #region Get ManagerScript
    /// <summary>
    /// Manager 스크립트 찾아와주는 메서드
    /// </summary>
    private void GetManagers()
    {
        dialManager = FindObjectOfType<DialManager>();
        questManager = FindObjectOfType<QuestManager>();
    }
    #endregion

    #region Scene Event
    /// <summary>
    /// Scene 로드 이벤트를 등록
    /// </summary>
    private void SubscribeToSceneEvents()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Scene 로드 이벤트를 해제합니다.
    /// </summary>
    private void UnsubscribeFromSceneEvents()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Scene이 로드될 때 호출
    /// 대화창을 초기화
    /// </summary>
    /// <param name="scene">로드된 씬</param>
    /// <param name="mode">로드 방식</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CloseText();
    }
    #endregion

    #region TextWindow Management
    /// <summary>
    /// TextWindow Management
    /// </summary>
    /// <param name="textbox"></param>
    /// <param name="isShowTextWindow"></param>
    private void DisplayText(string textbox, bool isShowTextWindow)
    {
        textComponent.text = textbox;
        textWindow.SetActive(isShowTextWindow);
    }

    /// <summary>
    /// 센터라벨 띄워주는 메서드
    /// </summary>
    /// <param name="textbox"></param>
    public void ShowText(string textbox)
    {
        if (textbox == null || dialManager.isTalking || questManager.isTalking) return;
        DisplayText(textbox, true);
    }

    /// <summary>
    /// 센터라벨 닫아주는 메서드
    /// </summary>
    public void CloseText()
    {
        DisplayText("", false);
    }
    #endregion
}