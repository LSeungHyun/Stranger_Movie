using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionQuest : AbstractInteraction
{
    [Header("Managers")]
    public QuestManager questManager;

    //[Header("UI Components")]
    //public GameObject confirmOn;
    //public WebGLBtn webglBtn;

    [Header("Quest Settings")]
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;

    public int questIndex = 0;
    public string correctAnswer = "feline";
    public string temptext = "※ マウスを使ってください。\r\n （モバイルはタッチ。)";
    public Sprite wrongSprite;

    //[Header("Interaction Settings")]
    //private bool isColliding = false;

    [Header("Events")]
    public Action onQuestStartedData;
    public Action onQuestStartedObject;

    //[System.Serializable]
    //public struct SpriteAnimation
    //{
    //    public Sprite sprite;
    //    public RuntimeAnimatorController animatorController;
    //}

    //[System.Serializable]
    //public struct AudioSetting
    //{
    //    public int PageNum;
    //    public int AudioNum;
    //    public bool isLoop;
    //}


    private void Awake()
    {
        FindComponents();
    }

    private void Update()
    {
        //리팩토링 후
        HandleInteractionEvent();
        

        //리팩토링 전
        //if (isColliding && !questManager.isTalking)
        //{
        //    if (isTouch)
        //    {
        //        SendDialogue();
        //    }
        //    else
        //    {
        //        confirmOn.SetActive(true);
        //        bool isButtonClicked = Input.GetKeyDown(KeyCode.F) || (webglBtn?.isClick ?? false);
        //        if (isButtonClicked)
        //        {
        //            SendDialogue();
        //        }
        //        //if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
        //        //{

        //        //    SendDialogue();
        //        //    webglBtn.isClick = false;
        //        //}
        //    }
        //}
    }
    #region OnTrigger
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    isColliding = true;
    //}

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        questManager.isTalking = false;
        //isColliding = false;
        //confirmOn.SetActive(false);
    }
    #endregion

    #region FindComponents
    public override void FindComponents()
    {
        base.FindComponents();
        questManager = FindObjectOfType<QuestManager>();
        //webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        //confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }
    #endregion

    #region Quest Management
    /// <summary>
    /// 퀘스트 상호작용 로직을 처리
    /// </summary>
    public override void HandleInteractionEvent()
    {
        if (!isColliding || questManager.isTalking) return;

        confirmOn.SetActive(true);
        if (IsInteractionTriggered())
        {
            StartQuest();
        }
    }
    /// <summary>
    /// 퀘스트를 시작
    /// </summary>
    private void StartQuest()
    {
        if (questManager == null || dialogueSprite.Count == 0) return;

        ConfigureQuest();
        onQuestStartedData?.Invoke();
        onQuestStartedObject?.Invoke();
        questManager.ShowQuest(dialogueSprite, dialogueAnimation, dialogueAudio);
    }

    /// <summary>
    /// 퀘스트 매니저를 구성
    /// </summary>
    private void ConfigureQuest()
    {
        questManager.questIndex = questIndex;
        questManager.correctAnswer = correctAnswer;
        questManager.wrongWindow.sprite = wrongSprite;
        questManager.text.text = temptext;
    }

    #endregion

    #region InteractionTrigger
    /// <summary>
    /// 입력이나 WebGL 버튼 클릭 상태를 확인
    /// </summary>
    /// <returns>상호작용 트리거 여부</returns>
    //private bool IsInteractionTriggered()
    //{
    //    return Input.GetKeyDown(KeyCode.F) || (webglBtn?.isClick ?? false);
    //}
    #endregion
    //void SendDialogue()
    //{
    //    if (questManager != null && dialogueSprite.Count > 0)
    //    {

    //        questManager.questIndex = questIndex;
    //        questManager.correctAnswer = correctAnswer;
    //        questManager.wrongWindow.sprite = wrongSprite;
    //        questManager.text.text = temptext;
    //        onQuestStartedData?.Invoke();
    //        onQuestStartedObject?.Invoke();

    //        questManager.ShowQuest(dialogueSprite, dialogueAnimation, dialogueAudio);
    //    }
    //}
}